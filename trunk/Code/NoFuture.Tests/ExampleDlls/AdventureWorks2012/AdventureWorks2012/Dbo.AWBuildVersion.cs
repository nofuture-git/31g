using System;
using System.Collections.Generic;

namespace AdventureWorks.Dbo
{
    [Serializable]
    public class AWBuildVersion
    {
        #region Id
        public virtual byte Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(AWBuildVersion other)
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
            if (obj.GetType() != typeof (AWBuildVersion)) return false;
            return this.Equals((AWBuildVersion) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string DatabaseVersion { get; set; }

       public virtual System.DateTime VersionDate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end AWBuildVersion
}//end AdventureWorks.Dbo


