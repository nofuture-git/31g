using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Exceptions;

namespace NoFuture.Hbm.SortingContainers
{
    [Serializable]
    public class StoredProcMetadata
    {
        protected internal string schemaName;
        protected internal string procName;
        protected internal List<StoredProcParamItem> parameters;
        protected internal Dictionary<string, List<ColumnMetadata>> returnedData;

        public StoredProcMetadata(string procName) { ProcName = procName; }
        internal StoredProcMetadata() { }

        private const string EXEC = "EXEC ";
        private const int MAX_STR_LEN = 16;

        public string SchemaName { get { return schemaName; } set { schemaName = value; } }
        public string ProcName { get { return procName; }  set { procName = value; }}
        public List<StoredProcParamItem> Parameters { get { return parameters; } set { parameters = value; } }

        /// <summary>
        /// Depending on the xsd file at <see cref="XsdFilePath"/> this function
        /// will parse the dataset into a dictionary having the DataTable.TableName
        /// as keys and each DataColumn in the DataColumnCollection as this lib's 
        /// common <see cref="ColumnMetadata"/>.
        /// </summary>
        public Dictionary<string, List<ColumnMetadata>> ReturnedData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(XsdFilePath))
                    return null;
                if (!File.Exists(XsdFilePath))
                    return null;

                if (returnedData != null)
                    return returnedData;

                var ds = new DataSet();
                ds.ReadXmlSchema(File.OpenRead(XsdFilePath));
                returnedData = new Dictionary<string, List<ColumnMetadata>>();
                for (var i = 0; i < ds.Tables.Count; i++)
                {
                    var tblName = ds.Tables[i].TableName;
                    var columns = new List<ColumnMetadata>();
                    for (var j = 0; j < ds.Tables[i].Columns.Count; j++)
                    {
                        var adoColumn = ds.Tables[i].Columns[j];
                        var cmeta = new ColumnMetadata();
                        cmeta.CopyFrom(adoColumn);
                        columns.Add(cmeta);
                    }
                    if(!returnedData.ContainsKey(tblName))
                        returnedData.Add(tblName,columns);
                    else
                        returnedData[tblName].AddRange(columns);
                }

                return returnedData;
            }
        }

        /// <summary>
        /// The cdata inner-text of the sql-query hbm.xml node.
        /// </summary>
        /// <returns></returns>
        public string ToHbmSql()
        {
            var hbmSql = new StringBuilder();
            hbmSql.AppendLine();
            hbmSql.Append(EXEC);
            if (ProcName.Contains("."))
            {
                //schema names may have periods within them looking like [my.schema].[MyStoredProc]
                //so put braces around everything to the left and right of the last period
                var procNameParts = ProcName.Split('.').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (procNameParts.Count == 1)
                    hbmSql.AppendFormat("[{0}]", procNameParts[0]);
                else if (procNameParts.Count == 2)
                {
                    hbmSql.AppendFormat("[{0}].[{1}]", procNameParts[0], procNameParts[1]);
                }
                else
                {
                    var leftSide = string.Format("[{0}]", string.Join(".", procNameParts.Take(procNameParts.Count - 1)));
                    var rightSide = string.Format("[{0}]", procNameParts[(procNameParts.Count - 1)]);
                    hbmSql.AppendFormat("{0}.{1}", leftSide, rightSide);
                }
            }
            else
            {
                hbmSql.AppendFormat("[{0}]", ProcName);
            }
            hbmSql.AppendLine();
            if(Parameters != null && Parameters.Count > 0)
                hbmSql.AppendLine(string.Join(",\n", Parameters.Select(x => x.ParamName.Replace("@", "\t:"))));
            return hbmSql.ToString();
        }

        /// <summary>
        /// Simply the path at <see cref="Settings.HbmStoredProcsDirectory"/> and the <see cref="ProcName"/> with
        /// a '.xsd' extension. This is expected to be used in PowerShell as a path where to dump the returned
        /// dataset as an xsd file.
        /// </summary>
        public string XsdFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProcName))
                    throw new RahRowRagee("This instance does not have a ProcName assigned.");
                
                return Path.Combine(Settings.HbmStoredProcsDirectory, string.Format("{0}.xsd", ProcName));
            }
        }

        /// <summary>
        /// Gets the <see cref="XsdFilePath"/> with the .bin extension.
        /// </summary>
        public string BinFilePath
        {
            get { return Path.ChangeExtension(XsdFilePath, "bin"); }
        }

        /// <summary>
        /// With a MSSQL Stored Proc there is no way of knowing what, if any, kind of dataset
        /// will be returned.  This is inteneded as a heuristic solution and is expected to 
        /// be called prior to actually invoking the stored proc on a SqlClient connection\command.
        /// </summary>
        /// <param name="sqlParams"></param>
        public void AssignSpParams(SqlParameterCollection sqlParams)
        {
            if(string.IsNullOrWhiteSpace(ProcName))
                throw new RahRowRagee("This instance does not have a ProcName assigned.");
            var myRand = new Random(Convert.ToInt32(string.Format("{0:ffffff}", DateTime.Now)));
            foreach (var param in Parameters)
            {
                if (param.IsDataTable())
                {
                    sqlParams.Add(new SqlParameter(param.ParamName, null));
                    continue;
                }
                var randStr = new StringBuilder();
                var llen = param.StringLength > MAX_STR_LEN || param.StringLength < 0
                    ? MAX_STR_LEN
                    : param.StringLength;

                //assign some lower-case letters
                for (var i = 0; i < llen; i++)
                {
                    randStr.Append(Convert.ToChar((byte)myRand.Next(0x61, 0x7A)));
                }

                if (param.IsOpenToSqlInj)
                {
                    randStr.Clear();
                    //we just want the schema not the data...
                    randStr.Append(" AND 1 = 0");
                }

                SqlParameter sqlParam;
                var sqlParamType = param.GetSqlDataType();
                switch (sqlParamType)
                {
                    case SqlDbType.NText:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Text:
                    case SqlDbType.VarChar:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = randStr.ToString()
                        };
                        break;
                    case SqlDbType.Xml:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = "<my-node>" + randStr + "</my-node>"
                        };
                        break;
                    case SqlDbType.NChar:
                    case SqlDbType.Char:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = Convert.ToChar((byte)myRand.Next(0x61, 0x7A))
                        };
                        break;
                    case SqlDbType.BigInt:
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                    case SqlDbType.Int:
                    case SqlDbType.Money:
                    case SqlDbType.Real:
                    case SqlDbType.SmallInt:
                    case SqlDbType.SmallMoney:
                    case SqlDbType.TinyInt:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = myRand.Next(1,255)//should fit all types
                        };
                        break;
                    case SqlDbType.Bit:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = 1
                        };
                        break;
                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.Time:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = DateTime.Now
                        };
                        break;

                    default:
                        sqlParam = new SqlParameter(param.ParamName, param.GetSqlDataType())
                        {
                            IsNullable = param.IsNullable,
                            Direction = param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input,
                            Value = DBNull.Value
                        };
                        break;
                }


                if (param.IsUserDefinedType && Util.NfTypeName.IsAssemblyFullName(param.DataType))
                    sqlParam.UdtTypeName = param.SqlUdtTypeName;

                sqlParams.Add(sqlParam);
            }
        }

        /// <summary>
        /// Writes this object to file as binary serialization for use by Hbm.InvokeStoredProc
        /// </summary>
        public void SerializeToDisk()
        {
            var binSer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                binSer.Serialize(ms, this);
                using (var binWrt = new BinaryWriter(File.Open(BinFilePath, FileMode.Create)))
                {
                    binWrt.Write(ms.GetBuffer());
                }
            }
        }

        /// <summary>
        /// This is the counterpart to <see cref="StoredProcMetadata.SerializeToDisk"/>
        /// which turns the file's contents back into the object.   
        /// </summary>
        /// <param name="path"></param>
        /// <param name="spmOut"></param>
        /// <returns>False if the object could not be deserialized.</returns>
        public static bool TryDeserializeFromDisk(string path, out StoredProcMetadata spmOut)
        {
            spmOut = null;
            if (!File.Exists(path))
                return false;
            try
            {
                var binSer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (var binRdr = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    var spmBin = binSer.Deserialize(binRdr.BaseStream);
                    spmOut = spmBin as StoredProcMetadata;
                    if (spmOut == null)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Settings.WriteToStoredProcLog(ex, string.Format("Could not deserialize file at '{0}'", path));
                return false;
            }
        }

        /// <summary>
        /// This is expected to break down the contents of whatever <see cref="ToHbmSql"/> returned
        /// </summary>
        /// <param name="sqlSyntax">The contents returned from <see cref="ToHbmSql"/></param>
        /// <param name="spDbName">The name of the stored proc in the database itself (e.g. [dbo].[MyStoredProc])</param>
        /// <param name="spParamNames">
        /// A list of strings used by NHibernate to assign parameters to the stored proc.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// This is only intended to parse the contents from <see cref="ToHbmSql"/> expecting its patterns and shape.
        /// </remarks>
        public static bool TryParseToHbmSql(string sqlSyntax, out string spDbName, out string[] spParamNames)
        {
            spDbName = null;
            spParamNames = null;

            if (string.IsNullOrWhiteSpace(sqlSyntax))
                return false;

            var spParts = sqlSyntax.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (spParts.Count <= 0)
                return false;
            if (!spParts[0].Contains(EXEC) || spParts[0].Split(' ').Length <= 1)
                return false;
            spDbName = spParts[0].Split(' ')[1];
            if (string.IsNullOrWhiteSpace(spDbName))
                return false;
            spDbName = spDbName.Trim();

            if (spParts.Count == 1)
                return true;

            var tempBuffer = new List<string>();
            for (var i = 1; i < spParts.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(spParts[i]))
                    continue;

                var par = spParts[i];
                par = par.Trim().Replace(",", string.Empty).Replace(":", string.Empty);
                tempBuffer.Add(par);
            }
            if (tempBuffer.Count > 0)
                spParamNames = tempBuffer.ToArray();

            return true;
        }
    }
}
