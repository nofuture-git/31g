using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// Base type to wrap access to the underlying address data
    /// </summary>
    [Serializable]
    public abstract class GeoBase
    {
        private readonly AddressData _data;

        protected GeoBase(AddressData d)
        {
            _data = d;
        }

        public AddressData GetData()
        {
            return _data ?? new AddressData();
        }

        protected static string[] ReadTextFileData(string name)
        {
            var asm = Assembly.GetExecutingAssembly();

            var data = asm.GetManifestResourceStream($"{asm.GetName().Name}.Data.{name}");
            if (data == null)
                return null;

            var strmRdr = new StreamReader(data);
            var webmailData = strmRdr.ReadToEnd();
            if (string.IsNullOrWhiteSpace(webmailData))
                return null;

            var txtData = webmailData.Split(Constants.LF).ToArray();
            return txtData;
        }
    }
}
