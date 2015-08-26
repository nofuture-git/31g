using System;
using System.Collections.Generic;

namespace AdventureWorks.Dbo
{
    [Serializable]
    public class DatabaseLog
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(DatabaseLog other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
            if (System.Object.ReferenceEquals(this, other)) return true;
			if (Id == 0 || other.Id == 0) return false;			
			return other.Id == Id;
        }

        public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == 0 ? newRandom.Next() : Id.GetHashCode();			
		}


        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (DatabaseLog)) return false;
            return this.Equals((DatabaseLog) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual System.DateTime PostTime { get; set; }

       public virtual string DatabaseUser { get; set; }

       public virtual string Event { get; set; }

       public virtual string Schema { get; set; }

       public virtual string Object { get; set; }

       public virtual string Tsql { get; set; }

       public virtual string XmlEvent { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end DatabaseLog
}//end AdventureWorks.Dbo


