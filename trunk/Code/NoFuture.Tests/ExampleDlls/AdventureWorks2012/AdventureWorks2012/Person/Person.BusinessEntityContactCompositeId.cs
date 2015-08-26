using System;

namespace AdventureWorks.Person
{
    [Serializable]
    public class BusinessEntityContactCompositeId
    {

        public virtual AdventureWorks.Person.BusinessEntity BusinessEntity { get; set; }

        public virtual AdventureWorks.Person.ContactType ContactType { get; set; }

        public virtual AdventureWorks.Person.Person Person { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (BusinessEntityContactCompositeId)obj;

            if(compareTo.BusinessEntity != this.BusinessEntity){return false;}

            if(compareTo.ContactType != this.ContactType){return false;}

            if(compareTo.Person != this.Person){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (BusinessEntity != null ? BusinessEntity.GetHashCode() : 1);

            sumValue += (ContactType != null ? ContactType.GetHashCode() : 1);

            sumValue += (Person != null ? Person.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end BusinessEntityContactCompositeId
}//end AdventureWorks.Person
