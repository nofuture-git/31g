using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;


/*
 --run boilerplate t-sql code to enable CLR types
EXEC sp_configure 'clr enabled';
EXEC sp_configure 'clr enabled' , '1';
RECONFIGURE;
 
--add the assembly
CREATE ASSEMBLY NoFutureTesting
FROM 'C:\Temp\NoFuture.Testing.dll'
WITH PERMISSION_SET = SAFE;

CREATE TYPE dbo.MyUdfType
EXTERNAL NAME NoFutureTesting.[NoFuture.Testing.MyUdfType]
 */
namespace NoFuture.Testing
{
    public static class Settings
    {
        public const int MAX_STRING_LENGTH = 512;
        public const string REFERENCE = "https://msdn.microsoft.com/en-us/library/microsoft.sqlserver.server.ibinaryserialize.read(v=vs.100).aspx";
    }

    [Serializable]
    [Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, MaxByteSize = 8000)]
    public class MyUdfType : Microsoft.SqlServer.Server.IBinarySerialize, System.Data.SqlTypes.INullable
    {
        System.Data.DataTable myDt;
        public string schema_name;
        public string table_name;
        public string column_name;
        public string data_type;
        public string constraint_name;
        public string unique_constraint_name;
        public string unique_constraint_schema;

        public int column_ordinal;
        public int string_length;

        public bool is_nullable;
        public bool is_auto_increment;

        /// <summary>
        /// You must read them in the same order you wrote them with 
        /// the same <see cref="Settings.MAX_STRING_LENGTH"/>
        /// </summary>
        /// <param name="r"></param>
        public void Read(BinaryReader r)
        {

            char[] strBuffer;
            var strLen = 0;

            var fldsStr = GetType().GetFields().Where(x => x.FieldType.FullName == "System.String").ToArray();
            var fldsInt = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Int32").ToArray();
            var fldsBool = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Boolean").ToArray();

            foreach (var strFld in fldsStr)
            {
                strBuffer = r.ReadChars(Settings.MAX_STRING_LENGTH);
                strLen = Array.IndexOf(strBuffer, '\0');
                if (strLen <= 0)
                    continue;
                var strVal = new string(strBuffer, 0, strLen);
                strFld.SetValue(this, strVal);
            }

            foreach (var intFld in fldsInt)
            {
                var valInt = r.ReadInt32();
                intFld.SetValue(this, valInt);
            }

            foreach (var fldBool in fldsBool)
            {
                var valBool = r.ReadBoolean();
                fldBool.SetValue(this, valBool);
            }
        }

        /// <summary>
        /// You must write them in the same order you will read them with 
        /// the same <see cref="Settings.MAX_STRING_LENGTH"/>
        /// </summary>
        /// <param name="w"></param>
        public void Write(BinaryWriter w)
        {
            var fldsStr = GetType().GetFields().Where(x => x.FieldType.FullName == "System.String").ToArray();
            var fldsInt = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Int32").ToArray();
            var fldsBool = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Boolean").ToArray();

            foreach (var fldStr in fldsStr)
            {
                var strVal = fldStr.GetValue(this);
                var paddedStr = strVal == null
                    ? string.Empty.PadRight(Settings.MAX_STRING_LENGTH, '\0')
                    : strVal.ToString().PadRight(Settings.MAX_STRING_LENGTH, '\0');
                for(var i = 0; i< paddedStr.Length; i++)
                    w.Write(i);
            }

            foreach (var fldInt in fldsInt)
            {
                int valIntOut;
                var valIntStr = fldInt.GetValue(this);
                if (valIntStr != null && int.TryParse(valIntStr.ToString(), out valIntOut))
                    w.Write(valIntOut);
                else
                    w.Write(0);
            }

            foreach (var blnFld in fldsBool)
            {
                bool valBlnOut;
                var valBlnStr = blnFld.GetValue(this);
                if(valBlnStr != null && bool.TryParse(valBlnStr.ToString(), out valBlnOut))
                    w.Write(valBlnOut);
                else
                    w.Write(false);
            }
        }


        //this is a required
        public bool IsNull
        {
            get
            {
                var fldsStr = GetType().GetFields().Where(x => x.FieldType.FullName == "System.String").ToArray();
                var fldsInt = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Int32").ToArray();
                var fldsBool = GetType().GetFields().Where(x => x.FieldType.FullName == "System.Boolean").ToArray();

                if (fldsStr.Select(str => str.GetValue(this)).Any(strVal => strVal != null))
                    return false;

                foreach (var fldInt in fldsInt)
                {
                    int valIntOut;
                    if (!int.TryParse(fldInt.GetValue(this).ToString(), out valIntOut))
                        continue;
                    if (valIntOut != 0)
                        return false;
                }
                foreach (var fldBln in fldsBool)
                {
                    bool valBlnOut;
                    if (!bool.TryParse(fldBln.GetValue(this).ToString(), out valBlnOut))
                        continue;
                    if (valBlnOut)
                        return false;
                }
                return true;
            }
        }

        //this is required
        public static MyUdfType Null
        {
            get { return new MyUdfType();}
        }

        //this is required
        [SqlMethod(OnNullCall = false)]
        public static MyUdfType Parse(System.Data.SqlTypes.SqlString s)
        {
            if (s.IsNull || string.IsNullOrEmpty(s.Value))
                return new MyUdfType();

            var ff = new MyUdfType();
            var buffer = new MemoryStream(Encoding.UTF8.GetBytes(s.Value));
            var binRdr = new BinaryReader(buffer);
            ff.Read(binRdr);
            return ff;
        }

        public override string ToString()
        {
            var buffer = new MemoryStream();
            var binWtr = new BinaryWriter(buffer);
            Write(binWtr);
            return Encoding.UTF8.GetString(buffer.ToArray());

        }
    }
}
