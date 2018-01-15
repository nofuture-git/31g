using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tele
{
    [Serializable]
    public class Email : Identifier
    {
        public override string Abbrev => "Email";

        public Uri ToUri()
        {
            return string.IsNullOrWhiteSpace(Value) ? null : new Uri($"emailto:{Value}");
        }

        /// <summary>
        /// Creates a random email address in a typical format
        /// </summary>
        /// <param name="names"></param>
        /// <param name="usCommonOnly">
        /// true uses <see cref="Net.UsWebmailDomains"/>
        /// false uses <see cref="Net.WebmailDomains"/>
        /// </param>
        /// <returns></returns>
        public static Email GetRandomEmail(bool usCommonOnly, params string[] names)
        {
            if (names == null || !names.Any())
                return GetRandomEmail(null);

            var fname = names.First().ToLower();
            var lname = names.Last().ToLower();
            string mi = null;
            if (names.Length > 2)
            {
                mi = names[1].ToLower();
                mi = Etx.CoinToss ? mi.First().ToString() : mi;
            }

            var unParts = new List<string> { Etx.CoinToss ? fname : fname.First().ToString(), mi, lname };
            var totalLength = unParts.Sum(x => x.Length);
            if (totalLength <= 7)
                return GetRandomEmail(String.Join(Etx.CoinToss ? "" : "_", String.Join(Etx.CoinToss ? "." : "_", unParts),
                    Etx.IntNumber(100, 9999)), usCommonOnly);
            return GetRandomEmail(totalLength > 20
                ? String.Join(Etx.CoinToss ? "." : "_", unParts.Take(2))
                : String.Join(Etx.CoinToss ? "." : "_", unParts), usCommonOnly);
        }

        public static Email GetChildishEmailAddress(bool usCommonOnly = true)
        {
            var shortWords = Etx.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
            var shortWordList = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var withUcase = Etc.CapWords(Etx.DiscreteRange(shortWords), ' ');
                shortWordList.Add(withUcase);
            }
            shortWordList.Add((Etx.CoinToss ? "_" : "") + Etx.IntNumber(100, 9999));
            return GetRandomEmail(String.Join("", shortWordList), usCommonOnly);
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        public static Email GetRandomEmail(string username, bool usCommonOnly = false)
        {
            var host = Net.RandomUriHost(false, usCommonOnly);
            if (!String.IsNullOrWhiteSpace(username))
                return new Email {Value = String.Join("@", username, host)};
            var bunchOfWords = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                bunchOfWords.Add(Etc.CapWords(Etx.Word(), ' '));
            }
            username = String.Join((Etx.CoinToss ? "." : "_"), Etx.DiscreteRange(bunchOfWords.ToArray()),
                Etx.DiscreteRange(bunchOfWords.ToArray()));
            return new Email {Value = String.Join("@", username, host)};
        }


    }
}
