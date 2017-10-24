using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
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

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var obj = NHibernateUtil.Binary.NullSafeGet(rs, names[0], session);
            if (obj == null)
            {
                return null;
            }
            var sidData = (byte[])obj;
            return new BinaryId(sidData);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                (cmd.Parameters[index] as IDbDataParameter).Value = DBNull.Value;
            }
            else
            {
                var sid = (BinaryId)value;
                (cmd.Parameters[index] as IDbDataParameter).DbType = DbType.Binary;
                (cmd.Parameters[index] as IDbDataParameter).Value = (byte[])sid.Data;
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

        public SqlType[] SqlTypes => SQL_TYPES;
        public Type ReturnedType => typeof(BinaryId);
        public bool IsMutable => false;
    }
}
