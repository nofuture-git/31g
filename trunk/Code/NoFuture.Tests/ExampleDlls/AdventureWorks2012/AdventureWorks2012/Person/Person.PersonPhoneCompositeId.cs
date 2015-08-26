using System;

namespace AdventureWorks.Person
{
    [Serializable]
    public class PersonPhoneCompositeId
    {

        public virtual AdventureWorks.Person.Person Person { get; set; }

        public virtual AdventureWorks.Person.PhoneNumberType PhoneNumberType { get; set; }

        public virtual string PhoneNumber { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (PersonPhoneCompositeId)obj;

            if(compareTo.Person != this.Person){return false;}

            if(compareTo.PhoneNumberType != this.PhoneNumberType){return false;}

            if(compareTo.PhoneNumber != this.PhoneNumber){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Person != null ? Person.GetHashCode() : 1);

            sumValue += (PhoneNumberType != null ? PhoneNumberType.GetHashCode() : 1);

            sumValue += (PhoneNumber != null ? PhoneNumber.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end PersonPhoneCompositeId
}//end AdventureWorks.Person
