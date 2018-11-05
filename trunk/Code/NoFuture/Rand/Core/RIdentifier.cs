using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Base type for identifiers whose value is derived 
    /// from an array of <see cref="Rchar"/>(s).
    /// </summary>
    /// <remarks>
    /// Given the format as an ordered-array of <see cref="Rchar"/>
    /// this type can both create random values and validate them.
    /// </remarks>
    [Serializable]
    public abstract class RIdentifier : Identifier
    {
        #region fields
        protected Rchar[] format;
        protected string _value;
        #endregion

        #region methods
        public virtual string GetRandom()
        {
            if (format == null)
                format = Etx.RandomRChars();
            var dl = new char[format.Length];
            for (var i = 0; i < format.Length; i++)
            {
                dl[i] = format[i].Rand;
            }
            return new string(dl);
        }
        public override string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_value))
                {
                    _value = GetRandom();
                }
                return _value;
            }
            set => _value = value;
        }

        public virtual bool Validate(string value = null)
        {
            value = value ?? _value;
            return format.All(rc => rc.Valid(value));
        }

        public static Rchar[] DeriveFromValue(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            var rchars = new List<Rchar>();
            var valueChars = id.ToCharArray();
            for (var i = 0; i < valueChars.Length; i++)
            {
                var c = valueChars[i];
                rchars.Add(Rchar.DeriveFromValue(i, c));
            }

            return rchars.ToArray();
        }
        #endregion
    }
}