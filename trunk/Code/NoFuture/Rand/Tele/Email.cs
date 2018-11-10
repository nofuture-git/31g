using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc cref="Identifier" />
    /// <inheritdoc cref="IObviate" />
    /// <summary>
    /// NoFuture Rand container for the general use Email entity
    /// </summary>
    [Serializable]
    public class Email : Identifier, IObviate
    {
        public override string Abbrev => "Email";

        public KindsOfLabels? Descriptor { get; set; }

        public Uri ToUri()
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
        /// Gets a randome email address that appears to be something a 12 year old would pick for a username
        /// </summary>
        /// <param name="usCommonOnly"></param>
        /// <returns></returns>
        [RandomFactory]
        public static Email RandomChildishEmail(bool usCommonOnly = true)
        {
            var shortWords = Etx.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
            var shortWordList = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var withUcase = Etc.CapWords(Etx.RandomPickOne(shortWords), ' ');
                shortWordList.Add(withUcase);
            }
            shortWordList.Add((Etx.RandomCoinToss() ? "_" : "") + Etx.RandomInteger(100, 9999));
            return RandomEmail(String.Join("", shortWordList), usCommonOnly);
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static Email RandomEmail(string username, bool usCommonOnly = false)
        {
            var host = Net.RandomUriHost(false, usCommonOnly);
            username = username ?? Net.RandomUsername();
            return new Email { Value = String.Join("@", username, host) };
        }

        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(Value))
                return itemData;

            var label = Descriptor?.ToString();
            itemData.Add(textFormat(label + Abbrev), Value);
            return itemData;
        }
    }
}
