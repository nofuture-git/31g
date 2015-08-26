using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NoFuture.Hbm.Sid
{
    public class SidUserType : IUserType
    {
        public static readonly SqlType[] SQL_TYPES = { NHibernateUtil.Binary.SqlType };

        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.Binary.NullSafeGet(rs, names[0]);
            if (obj == null)
            {
                return null;
            }
            var sidData = (byte[])obj;
            return new BinaryId(sidData);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDbDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                var sid = (BinaryId)value;
                ((IDbDataParameter)cmd.Parameters[index]).DbType = DbType.Binary;
                ((IDbDataParameter)cmd.Parameters[index]).Value = (byte[])sid.Data;
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes { get { return SQL_TYPES; } }
        public Type ReturnedType { get { return typeof(BinaryId); } }
        public bool IsMutable { get { return false; } }
    }
}
