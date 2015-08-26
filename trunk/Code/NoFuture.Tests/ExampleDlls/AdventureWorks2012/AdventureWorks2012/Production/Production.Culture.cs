using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class Culture
    {
        #region Id
        public virtual string Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Culture other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
            if (System.Object.ReferenceEquals(this, other)) return true;
			if (System.String.IsNullOrEmpty(Id) || System.String.IsNullOrEmpty(other.Id)) return false;			
			return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Culture)) return false;
            return this.Equals((Culture) obj);
        }
        
        public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return System.String.IsNullOrEmpty(Id) ? newRandom.Next() : Id.GetHashCode();			
		}

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }

       #endregion


    }//end Culture
}//end AdventureWorks.Production


