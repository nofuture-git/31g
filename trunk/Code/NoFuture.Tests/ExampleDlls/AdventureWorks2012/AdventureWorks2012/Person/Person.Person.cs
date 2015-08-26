using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class Person
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Person other)
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
            if (obj.GetType() != typeof (Person)) return false;
            return this.Equals((Person) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string PersonType { get; set; }

       public virtual bool NameStyle { get; set; }

       public virtual string Title { get; set; }

       public virtual string FirstName { get; set; }

       public virtual string MiddleName { get; set; }

       public virtual string LastName { get; set; }

       public virtual string Suffix { get; set; }

       public virtual int EmailPromotion { get; set; }

       public virtual string AdditionalContactInfo { get; set; }

       public virtual string Demographics { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.BusinessEntity BusinessEntityByBusinessEntityID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.HumanResources.Employee> Employees { get; set; }

        public virtual IList<AdventureWorks.Person.BusinessEntityContact> BusinessEntityContacts { get; set; }

        public virtual IList<AdventureWorks.Person.EmailAddress> EmailAddress { get; set; }

        public virtual IList<AdventureWorks.Person.Password> Passwords { get; set; }

        public virtual IList<AdventureWorks.Person.PersonPhone> PersonPhones { get; set; }

        public virtual IList<AdventureWorks.Sales.Customer> Customers { get; set; }

        public virtual IList<AdventureWorks.Sales.PersonCreditCard> PersonCreditCards { get; set; }

       #endregion


    }//end Person
}//end AdventureWorks.Person


