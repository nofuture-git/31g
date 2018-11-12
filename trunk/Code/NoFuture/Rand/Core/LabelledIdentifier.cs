using System;
using System.Collections.Generic;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Core
{
    /// <inheritdoc cref="IObviate" />
    /// <inheritdoc cref="Identifier" />
    /// <summary>
    /// Joins a label, a name (if needed) to the identifying <see cref="Identifier.Value"/>
    /// </summary>
    public abstract class LabelledIdentifier : Identifier, IObviate
    {
        public virtual string Name { get; set; }
        public KindsOfLabels? Descriptor { get; set; }

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            var value = Value;
            if (string.IsNullOrWhiteSpace(value))
                return itemData;

            var label = Descriptor?.ToString().Replace(", ", "");
            if (!string.IsNullOrWhiteSpace(Name))
                label = Name + label;
            itemData.Add(textFormat(label + GetType().Name), value);
            return itemData;
        }
    }
}
