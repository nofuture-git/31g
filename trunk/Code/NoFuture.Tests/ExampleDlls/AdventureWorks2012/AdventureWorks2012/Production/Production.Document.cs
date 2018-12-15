using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class Document
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Document other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (Document) && Equals((Document) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual string Title { get; set; }

       public virtual bool FolderFlag { get; set; }

       public virtual string FileName { get; set; }

       public virtual string FileExtension { get; set; }

       public virtual string Revision { get; set; }

       public virtual int ChangeNumber { get; set; }

       public virtual byte Status { get; set; }

       public virtual string DocumentSummary { get; set; }

       public virtual string Document00 { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.HumanResources.Employee EmployeeByOwner { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.ProductDocument> ProductDocuments { get; set; }

       #endregion


    }//end Document
}//end AdventureWorks.Production


