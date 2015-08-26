using System;

namespace AdventureWorks.Person
{
    [Serializable]
    public class BusinessEntityAddressCompositeId
    {

        public virtual AdventureWorks.Person.Address Address { get; set; }

        public virtual AdventureWorks.Person.AddressType AddressType { get; set; }

        public virtual AdventureWorks.Person.BusinessEntity BusinessEntity { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (BusinessEntityAddressCompositeId)obj;

            if(compareTo.Address != this.Address){return false;}

            if(compareTo.AddressType != this.AddressType){return false;}

            if(compareTo.BusinessEntity != this.BusinessEntity){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Address != null ? Address.GetHashCode() : 1);

            sumValue += (AddressType != null ? AddressType.GetHashCode() : 1);

            sumValue += (BusinessEntity != null ? BusinessEntity.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end BusinessEntityAddressCompositeId
}//end AdventureWorks.Person
