using System;

namespace AdventureWorks.Person
{
    [Serializable]
    public class EmailAddressCompositeId
    {

        public virtual AdventureWorks.Person.Person Person { get; set; }

        public virtual int EmailAddressID { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (EmailAddressCompositeId)obj;

            if(compareTo.Person != this.Person){return false;}

            if(compareTo.EmailAddressID != this.EmailAddressID){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Person != null ? Person.GetHashCode() : 1);

            sumValue += (EmailAddressID != null ? EmailAddressID.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end EmailAddressCompositeId
}//end AdventureWorks.Person
