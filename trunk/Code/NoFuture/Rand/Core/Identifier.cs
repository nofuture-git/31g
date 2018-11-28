using System;
using System.Text;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Base type for string identity values
    /// </summary>
    [Serializable]
    public abstract class Identifier : IIdentifier<string>
    {
        private string _value;

        public abstract string Abbrev { get; }

        public virtual string Value
        {
            get => _value;
            set => _value = value;
        }

        public virtual string Src { get; set; }
        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public virtual bool Equals(Identifier obj)
        {
            return string.Equals(obj.Value, Value);
        }

        public override bool Equals(object obj)
        {
            var id = obj as Identifier;
            return id != null && Equals(id);
        }

        public virtual bool Equals(string idValue)
        {
            return string.Equals(idValue, _value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Gets the value where only the last four chars are displayed
        /// the other chars are replaced with &apos;X&apos;
        /// </summary>
        /// <returns></returns>
        public virtual string ValueLastFour()
        {
            var bldr = new StringBuilder();
            var val = Value;
            if (string.IsNullOrWhiteSpace(val))
                return ToString();

            for (var i = 0; i < val.Length - 4; i++)
            {
                bldr.Append("X");
            }
            var lastFour = val.Substring(val.Length - 4, 4);
            bldr.Append(lastFour);
            return bldr.ToString();
        }
    }
}

/*
 Other APIs
https://www.data.gov/developers/apis
http://www.irs.gov/uac/Tax-Stats-2
http://projects.propublica.org/nonprofits/api
https://www.pacer.gov/
https://nccd.cdc.gov/NPAO_DTM/

this looks fun
https://www.treasury.gov/resource-center/sanctions/SDN-List/Pages/default.aspx

#search LOINC
https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=2093-3&knowledgeResponseType=application/javascript

#another drug search 
https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.88?mainSearchCriteria.v.c=637188&knowledgeResponseType=application/javascript

#api docx
https://rxnav.nlm.nih.gov/RxNormAPIREST.html

#get a list of a bunch of drugs
https://rxnav.nlm.nih.gov/REST/allconcepts?tty=BN+BPCK

#some kind of standard used to send Rx to pharmacy - requires a membership
http://www.ncpdp.org/Standards-Development/Standards-Information

#way to lookup the ICD-10 codes - requires a API Key
# this api is strange - use the api-key to get another token, then use 
# that token to get yet another token
https://documentation.uts.nlm.nih.gov/rest/home.html

#.gov domains
https://inventory.data.gov/dataset/b2c31002-8a5e-4cd0-85bd-6971934f4e59/resource/02706c7b-98e3-4267-8dd8-e3bb2d8c7ce3/download/fed-domains-04212017.csv

#money laundering
http://www.opensanctions.org/
 */
