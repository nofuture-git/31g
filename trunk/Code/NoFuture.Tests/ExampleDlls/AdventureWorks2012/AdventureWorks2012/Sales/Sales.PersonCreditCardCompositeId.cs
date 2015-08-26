using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class PersonCreditCardCompositeId
    {

        public virtual AdventureWorks.Sales.CreditCard CreditCard { get; set; }

        public virtual AdventureWorks.Person.Person Person { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (PersonCreditCardCompositeId)obj;

            if(compareTo.CreditCard != this.CreditCard){return false;}

            if(compareTo.Person != this.Person){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (CreditCard != null ? CreditCard.GetHashCode() : 1);

            sumValue += (Person != null ? Person.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end PersonCreditCardCompositeId
}//end AdventureWorks.Sales
