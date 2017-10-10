using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Gov
{
    //Jurisprudence: the science of law
    //Jurisdiction: authority given to a court to rule over some region
    //Precedent: rule established in prev. legal case
    [Serializable]
    public abstract class Statute
    {
        public enum StatuteType
        {
            Federal,
            State
        }
        public abstract string Name { get; }
        public abstract StatuteType Type { get; }
        public abstract int Year { get; }
    }

    [Serializable]
    public abstract class FederalStatute : Statute
    {
        public override StatuteType Type
        {
            get { return StatuteType.Federal; }
        }

        public override string ToString()
        {
            return Name;
        }

        internal static List<FederalStatute> statutes;

        public static FederalStatute GetFederalStatue(string namedLike)
        {
            if (statutes != null && statutes.Count > 0)
                return statutes.FirstOrDefault(s => System.Text.RegularExpressions.Regex.IsMatch(s.Name, namedLike));
            statutes = new List<FederalStatute>
                       {
                           new DoddFrankAct(),
                           new InvestmentAdvisorsAct(),
                           new InvestmentCompanyAct(),
                           new JOBSAct(),
                           new SarbanesOxley(),
                           new SecuritiesAct(),
                           new SecuritiesExchangeAct(),
                           new TrustIndentureAct(),
                           new TaxReformAct()
                       };
            return statutes.FirstOrDefault(s => System.Text.RegularExpressions.Regex.IsMatch(s.Name, namedLike));
        }
    }
    [Serializable]
    public class DoddFrankAct : FederalStatute
    {
        public override string Name => "Dodd-Frank Act of 2010";
        public override int Year => 2010;
    }
    [Serializable]
    public class InvestmentAdvisorsAct : FederalStatute
    {
        public override string Name => "Investment Advisers Act of 1940";
        public override int Year => 1940;
    }
    [Serializable]
    public class InvestmentCompanyAct : FederalStatute
    {
        public override string Name => "Investment Company Act of 1940";
        public override int Year => 1940;
        public Uri FullForm => new Uri("https://www.sec.gov/about/laws/ica40.pdf");
    }
    [Serializable]
    public class JOBSAct : FederalStatute
    {
        public override string Name => "JOBS Act of 2012";
        public override int Year => 2012;
    }
    [Serializable]
    public class SarbanesOxley : FederalStatute
    {
        public override string Name => "Sarbanes-Oxley Act of 2002";
        public override int Year => 2002;
    }
    [Serializable]
    public class SecuritiesAct : FederalStatute
    {
        public override string Name => "Securities Act of 1933";
        public override int Year => 1933;
    }
    [Serializable]
    public class SecuritiesExchangeAct : FederalStatute
    {
        public override string Name => "Securities Exchange Act of 1934";
        public override int Year => 1934;
    }
    [Serializable]
    public class TrustIndentureAct : FederalStatute
    {
        public override string Name => "Trust Indenture Act of 1939";
        public override int Year => 1939;
    }
    [Serializable]
    public class TaxReformAct : FederalStatute
    {
        public override string Name => "Tax Reform Act of 1986";
        public override int Year => 1986;
    }
    [Serializable]
    public class BankSecrecyAct : FederalStatute
    {
        public override string Name => "Bank Secrecy Act of 1970";
        public override int Year => 1970;
    }
    
}