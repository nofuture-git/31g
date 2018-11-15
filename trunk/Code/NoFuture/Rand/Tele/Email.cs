using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc cref="NetUri" />
    /// <summary>
    /// NoFuture Rand container for the general use Email entity
    /// </summary>
    [Serializable]
    public class Email : NetUri
    {
        public override string Abbrev => "Email";

        public override Uri ToUri()
        {
            return string.IsNullOrWhiteSpace(Value) ? null : new Uri($"emailto:{Value}");
        }

        /// <summary>
        /// Creates a random email address in a typical format
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static Email RandomEmail()
        {
            return RandomEmail(null, true);
        }

        /// <summary>
        /// Gets a random email address that appears to be something a 12 year old would pick for a username
        /// </summary>
        /// <param name="usCommonOnly"></param>
        /// <returns></returns>
        [RandomFactory]
        public static Email RandomChildishEmail(bool usCommonOnly = true)
        {
            var shortWords = Etx.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
            var shortWordList = new List<string>();
            var otherWords = new[]
            {
                "FART","Butt","K1tty","Turb0","T4ashy","Ch1ck","Dud3","P00p",
                "Peepee","RaZZ","C00l","D0PE","Sa7age","Techno","Kn0cka","Turd",
                "D00D00","D00M","LEET","D0lla","PWND","Twitch","Tank","Fr1es",
                "P1mp","Punk","Bla5e","Blunt","H0tty","PRO","Babe","Fatty","Candy", "Cand1e",
                "Mas33a","Brut1l","BRO","P1zza","P0ny", "D0ll","Ball3r","P1mpin",
                "R0cka","Wanka","Sanp1e","DewWWW","W1cked","B0gus","M1lki","Pan_ts",
                "L0aded","T1pp1n","Get1n","Bomba","Suck","Nasta","Toot","Prancin","Princess",
                "K1ng","S1ck","SlaMMa","P1rate","B0ss","Dirty","Naked","Pay33d","P1pe",
                "Aws0me","Mortie","Sacce","Darkness","Lips","Kickin","Killa","Digg",
                "DaMMe","Villan", "V1llen","Fasta","Doggo","Dang3r","Vap3", "Skull", "SmOke",
                "Smokin","Holla","Rasta","B0ner","SIffie","Bang1n","Br1ngIt","GH0SST"
            };

            var withUcase = Etc.CapWords(Etx.RandomPickOne(shortWords), ' ');
            shortWordList.Add(withUcase);
            shortWordList.Add(Etx.RandomPickOne(otherWords));

            shortWordList = Etx.RandomShuffle(shortWordList.ToArray()).ToList();

            shortWordList.Add((Etx.RandomCoinToss() ? "_" : "") + Etx.RandomInteger(100, 9999));
            return RandomEmail(string.Join("", shortWordList), usCommonOnly);
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static Email RandomEmail(string username, bool usCommonOnly = false)
        {
            var host = NetUri.RandomUriHost(false, usCommonOnly);
            username = username ?? NetUri.RandomUsername();
            return new Email { Value = String.Join("@", username, host) };
        }
    }
}
