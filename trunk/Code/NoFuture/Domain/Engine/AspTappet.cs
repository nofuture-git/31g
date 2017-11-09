using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NoFuture.Util;

namespace NoFuture.Domain
{
    /// <summary>
    /// Got this from 'http://msdn.microsoft.com/en-us/magazine/cc164734.aspx?code=true&level=root%2cSimpleHostListenerASMX&file=HttpListenerLibrary.cs'
    /// </summary>
    public class AspTappet : System.Web.HttpWorkerRequest
    {
        #region fields
        private readonly System.Net.HttpListenerContext _context;
        private readonly List<string> _restrictedResponseHeaders = new List<string>
                                                              {
                                                                  "date",
                                                                  "host",
                                                                  "connection",
                                                                  "content-length",
                                                                  "if-modified-since",
                                                                  "range",
                                                                  "transfer-encoding"
                                                              };
        private readonly string _virtualDir;
        private readonly string _physicalDir;
        private readonly Guid _traceId;
        private static List<string> _aspPipelineExtensions;
        private readonly bool _enableLogging;
        #endregion

        #region statics
        /// <summary>
        /// Give a uri, each segment is concat'ed together and  upon the 
        /// first segment matching one the the extensions from 
        /// <see cref="GetGlobalWebConfigAspExtensions"/> the 
        /// the concat'ed string is returned
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetFilePathSegment(Uri uri)
        {
            var fp = new System.Text.StringBuilder();
            foreach(var segment in uri.Segments)
            {
                if (_aspPipelineExtensions == null)
                    _aspPipelineExtensions = GetGlobalWebConfigAspExtensions();

                var segmentContents = segment.Replace("/", string.Empty);

                if(_aspPipelineExtensions.Any(ext => System.Text.RegularExpressions.Regex.IsMatch(segmentContents,ext)))
                {
                    fp.Append(segmentContents);

                    return fp.ToString();
                }
                fp.Append(segment);
            }
            return uri.LocalPath;
        }

        /// <summary>
        /// Gets a list of extensions, each being a regex pattern, from this
        /// machine's web.config.  The extensions are sourced from the 
        /// httpHandlers section.
        /// </summary>
        /// <returns></returns>
        public static List<string>  GetGlobalWebConfigAspExtensions()
        {
            var extensions = new List<string>();
            var configFile = SysCfg.GetAspNetWebCfg();

            var httpHandlersNodes = configFile.SelectNodes("//httpHandlers/add");
            if(httpHandlersNodes == null || httpHandlersNodes.Count == 0)
                return null;

            foreach (System.Xml.XmlNode httpHandlersNode in httpHandlersNodes)
            {
                if(httpHandlersNode.Attributes == null)
                    continue;
                if (httpHandlersNode.Attributes["path"] == null)
                    continue;
                if(string.IsNullOrWhiteSpace(httpHandlersNode.Attributes["path"].Value))
                    continue;
                if(httpHandlersNode.Attributes["path"].Value == "*")
                    continue;

                extensions.Add(httpHandlersNode.Attributes["path"].Value.Replace(".",@"\.").Replace("*",".*"));
            }

            return extensions;
        }
        #endregion

        #region ctor
        public AspTappet(System.Net.HttpListenerContext context, string vdir, string pdir, bool enableLogging = true)
        {
            if (null == context)
                throw new ArgumentNullException("context");
            if (string.IsNullOrWhiteSpace(vdir))
                throw new ArgumentException("vdir");
            if (string.IsNullOrWhiteSpace(pdir))
                throw new ArgumentException("pdir");

            _context = context;
            _virtualDir = vdir;
            _physicalDir = pdir;
            //Path.Combine don't work in these here parts...
            if (_physicalDir.EndsWith(@"\"))
                _physicalDir = _physicalDir.Substring(0, _physicalDir.Length - 1);
            _traceId = Guid.NewGuid();
            _enableLogging = enableLogging;

            try
            {
                if(_aspPipelineExtensions == null)
                    _aspPipelineExtensions = GetGlobalWebConfigAspExtensions();
            }
            catch (Exception ex)
            {
                Log.Write("error while trying to get machine-level web config");
                Log.ExceptionLogging(ex);
            }

        }
        #endregion

        #region Request Typical
        public override string RootWebConfigPath
        {
            get { return Path.Combine(_physicalDir, @"web.config"); }
        }
        public override string GetRawUrl()
        {
            return _context.Request.RawUrl;
        }
        public override string GetHttpVerbName()
        {
            return _context.Request.HttpMethod;
        }
        public override string GetFilePath()
        {
            return GetFilePathSegment(_context.Request.Url);
        }
        public override string GetPathInfo()
        {
            var s1 = GetFilePathSegment(_context.Request.Url);
            var s2 = _context.Request.Url.LocalPath;
            return s1.Length == s2.Length ? string.Empty : s2.Substring(s1.Length);
        }
        public override Guid RequestTraceIdentifier
        {
            get { return _traceId; }
        }
        public override string GetUriPath()
        {
            return _context.Request.Url.LocalPath;
        }
        public override string GetQueryString()
        {
            if (!Uri.IsWellFormedUriString(_context.Request.RawUrl, UriKind.RelativeOrAbsolute))
                return string.Empty;

            var uri = _context.Request.Url;

            var queryString = uri.Query;
            if (queryString.StartsWith("?"))
                queryString = queryString.Substring(1);

            return queryString;
        }
        public override string GetFilePathTranslated()
        {
            var s = GetFilePathSegment(_context.Request.Url);
            s = s.Replace("/", @"\");
            var filePath = _physicalDir + s;//do not use Path.Combine...
            if (_enableLogging)
            {
                Log.Write(string.Format("GetFilePathTranslated()={0}", filePath));
            }
            return filePath;
        }
        public override string GetKnownRequestHeader(int index)
        {
            switch (index)
            {
                case HeaderUserAgent:
                    return _context.Request.UserAgent;
                default:
                    return _context.Request.Headers[GetKnownRequestHeaderName(index)];
            }
        }
        public override string GetUnknownRequestHeader(string name)
        {
            return _context.Request.Headers[name];
        }
        public override string[][] GetUnknownRequestHeaders()
        {
            var headers = _context.Request.Headers;
            var headerPairs = new List<string[]>(headers.Count);
            for (var i = 0; i < headers.Count; i++)
            {
                var headerName = headers.GetKey(i);
                if (GetKnownRequestHeaderIndex(headerName) != -1)
                    continue;

                var headerValue = headers.Get(i);
                headerPairs.Add(new[] { headerName, headerValue });
            }
            return headerPairs.ToArray();

        }
        #endregion

        #region Response Typical
        public override void SendStatus(int statusCode, string statusDescription)
        {
            _context.Response.StatusCode = statusCode;
            _context.Response.StatusDescription = statusDescription;
            if (_enableLogging) { Log.Write(string.Format("SendStatus({0},'{1})", statusCode, statusDescription )); }
        }
        public override void SendKnownResponseHeader(int index, string value)
        {
            var rspnHeader = GetKnownResponseHeaderName(index);
            if(!_restrictedResponseHeaders.Contains(rspnHeader.ToLower()))
            {
                _context.Response.Headers[GetKnownResponseHeaderName(index)] = value;    
            }
        }
        public override void SendUnknownResponseHeader(string name, string value)
        {
            if(!_restrictedResponseHeaders.Contains(name.ToLower()))
            {
                _context.Response.Headers[name] = value;    
            }
        }
        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {

        }
        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            _context.Response.OutputStream.Write(File.ReadAllBytes(filename), (int)offset, (int)length);
            if (_enableLogging)
            {
                Log.Write(string.Format("SendResponseFromFile('{0},{1},{2}')", filename, offset, length));
            }
        }
        public override void SendResponseFromMemory(byte[] data, int length)
        {
            _context.Response.OutputStream.Write(data, 0, length);
        }
        public override void FlushResponse(bool finalFlush)
        {
            _context.Response.OutputStream.Flush();
        }
        public override void EndOfRequest()
        {
            if (_context.Response.OutputStream != null)
                if (_context.Response.OutputStream.CanWrite)
                    _context.Response.OutputStream.Close();
            _context.Response.Close();
        }
        #endregion

        #region additional overrides
        public override string GetAppPath()
        {
            return _virtualDir;
        }
        public override string GetAppPathTranslated()
        {
            return _physicalDir;
        }
        public override int GetRemotePort()
        {
            var port = 0;
            if (_context.Request.RemoteEndPoint != null)
                port = _context.Request.RemoteEndPoint.Port;
            return port;
        }
        public override int ReadEntityBody(byte[] buffer, int size)
        {
            return _context.Request.InputStream.Read(buffer, 0, size);
        }
        public override string GetServerVariable(string name)
        {
            switch (name)
            {
                case "HTTPS":
                    return _context.Request.IsSecureConnection ? "on" : "off";
                case "HTTP_USER_AGENT":
                    return _context.Request.Headers["UserAgent"];
                default:
                    return null;
            }
        }
        public override string GetHttpVersion()
        {

            var rtrn = string.Format("HTTP/{0}.{1}", _context.Request.ProtocolVersion.Major, _context.Request.ProtocolVersion.Minor);
            return rtrn;
        }
        public override string GetLocalAddress()
        {
            if (_context.Request.LocalEndPoint != null)
            {
                return _context.Request.LocalEndPoint.Address.ToString();
            }
            return null;
        }
        public override int GetLocalPort()
        {
            if (_context.Request.LocalEndPoint != null)
            {
                return _context.Request.LocalEndPoint.Port;
            }
            return 0;
        }
        public override string GetRemoteAddress()
        {
            if (_context.Request.RemoteEndPoint != null)
            {
                return _context.Request.RemoteEndPoint.Address.ToString();
            }
            return null;

        }
        public override void CloseConnection()
        {

        }
        #endregion
    }
}
