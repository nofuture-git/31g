using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoFuture.Util.Etymological.Biz
{
    public class CardinalOrdinal : NomenclatureBase
    {
        private static readonly string[] CARDINAL = {
            "First", "Second", "Third", "Forth", "Fifth", "Sixth", "Primary", "Secondary", "Tertiary", "Once", "Twice",
            "Thrice", "Preferred", "Alternative"
        };

        public CardinalOrdinal()
        {
            Synonyms.AddRange(CARDINAL);
        }
    }

    public class Demonyms : NomenclatureBase
    {
        private static readonly string[] DEMONYMS = 
        {
            "African", "American", "Australian", "Austrian", "Canadian",
            "English", "European",
            "German", "Indian", "Irish", "Italian", "Polish",
            "Portuguese",
            "Russian", "Spanish", "Swiss", "Mexican",
            "French", "Dutch", "Chinese", "Japanese", "Korean"
        };
        public Demonyms()
        {
            Synonyms.AddRange(DEMONYMS);
        }
    }

    public class Toponyms : NomenclatureBase
    {
        public const string ADDRESS = "Addr(ess|esses)?";

        public static readonly string[] ADDR_PARTS = { "City", "Postalcode", "Zip(code(s)?)?", "(State|Providence)", "Street", "Country", };

        private static readonly string[] TOPONYMS = 
        {
            "Altitude", "District",
            "(Latitude|Lat)", "(Longitude|Lng)",
            "Region",  "Location",
            "Building", "Suite", "Room", 
            "Timezone", "Site", "District", 
            "Headquarters", "Landmark"
        };

        public Toponyms()
        {
            Synonyms.AddRange(TOPONYMS);
            Synonyms.Add(ADDRESS);
            Synonyms.AddRange(ADDR_PARTS);
        }

        public override bool HasSemblance(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 1)
                return base.HasSemblance(variousStrings);

            var nextToLast = GetNextToLast(variousStrings);
            if (Regex.IsMatch(nextToLast, ADDRESS, RegexOptions.IgnoreCase))
                return true;

            var lastEntry = variousStrings.Last();
            if (ADDR_PARTS.Any(x => Regex.IsMatch(lastEntry, x, RegexOptions.IgnoreCase)) ||
                ADDR_PARTS.Any(x => Regex.IsMatch(nextToLast, x, RegexOptions.IgnoreCase)))
                return true;

            return TOPONYMS.Any(x => Regex.IsMatch(lastEntry, x, RegexOptions.IgnoreCase)) ||
                   TOPONYMS.Any(x => Regex.IsMatch(nextToLast, x, RegexOptions.IgnoreCase));
        }
    }

    public class Chronos : NomenclatureBase
    {
        private static readonly string[] DEFINITIVE = {"Date", "Time"};
        private static readonly string[] CHRONOS = 
        {
            "Age", "Annual", "Daily", "Season", "Week",  "Period", "Phase", "Deadline", 
        };

        private static readonly string[] PREFIX_SUFFIX_DATE_TIME =
        {
            "Start", "End", "Finish", "Create", "Begin", "Birth", "Death", "Arriv(al|e)", "Depart(ure)?", "Deliver(y)?", "Schedule(d)?"
        };

        public Chronos()
        {
            Synonyms.AddRange(DEFINITIVE);
            Synonyms.AddRange(CHRONOS);
            Synonyms.AddRange(PREFIX_SUFFIX_DATE_TIME);
        }

        public override bool HasSemblance(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 1)
                return base.HasSemblance(variousStrings);

            var lastEntry = variousStrings.Last();
            var nextToLast = GetNextToLast(variousStrings);

            var oneIsDateTime = DEFINITIVE.Any(x => Regex.IsMatch(lastEntry, x, RegexOptions.IgnoreCase)) ||
                                DEFINITIVE.Any(x => Regex.IsMatch(nextToLast, x, RegexOptions.IgnoreCase));

            var oneIsPreSuff = PREFIX_SUFFIX_DATE_TIME.Any(x => Regex.IsMatch(lastEntry, x, RegexOptions.IgnoreCase)) ||
                                PREFIX_SUFFIX_DATE_TIME.Any(x => Regex.IsMatch(nextToLast, x, RegexOptions.IgnoreCase));

            return oneIsDateTime
                ? oneIsPreSuff
                : CHRONOS.Any(x => string.Equals(x, lastEntry, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class Monetary : NomenclatureBase
    {
        private static readonly string[] MONEY = 
        {
            "Amount", "Balance", "Charge", "Cost", "Currency", "Debit",
            "Deductible", "Deposit", "Discount", "Estimate", "Interest", "Pay",
            "Paid", "Price", "Subtotal", "Total", "Bill", "Fee", 
            "Loss", "Gross", "Money", "Monetary", "Prepaid", "Award",
            "Funds", "Advance", "Credit", "Dollar", "Euro", "Yen"
        };

        public Monetary()
        {
            Synonyms.AddRange(MONEY);
        }
    }

    public class Numeric : NomenclatureBase
    {
        private static readonly string[] NUMERIC = 
        {
            "Coefficient", "Coordinate", "Dimension", "Number", "Size",
            "Weight", "Width", "Yield", "Distance", "Variance", "Variant", 
            "Volume", "Capacity", "Accuracy", "Count", "Percent", "Score", 
            "Viscosity", "Range", "Rate", "Actual", "Max", "Maximum", "Min", 
            "Minimum", "Limit", "Digit", "Length", "Quantity", "Temperature", 
            "Minus", "Pressure", "Point", "Height", "Plus", "Factor", "Occupancy",
        };

        public Numeric()
        {
            Synonyms.AddRange(NUMERIC);
        }
    }

    public class BizStrings : NomenclatureBase
    {

        private static readonly string[] STRINGS = new[]
        {
            "Note", "Token", "Text", "Description", "Content", "Value", "Comment",
            "Symbol", "Password", "Remarks", "Message", "Descriptor", "Alias",
            "Barcode", "Tag", "Title", "Display"
        };
        public BizStrings()
        {
            Synonyms.AddRange(STRINGS);
        }

        public override bool HasSemblance(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 1)
                return base.HasSemblance(variousStrings);

            return STRINGS.Any(x => Regex.IsMatch(variousStrings.Last(), x, RegexOptions.IgnoreCase));
        }
    }

    public class NetworkResource : NomenclatureBase
    {
        private static readonly string[] URIS = 
        {
             "Uri", "Url", "Website", "Path", "File", "Email", "Ip", "Ipv4", "Ipv6"
        };

        public NetworkResource()
        {
            Synonyms.AddRange(URIS);
        }
        
    }

    public class TelecoResource : NomenclatureBase
    {
        private static readonly string[] TELECO = { "Phone", "Fax", "Mobile", "Cell" };

        public TelecoResource()
        {
            Synonyms.AddRange(TELECO);
        }
        
    }

    public class Metrix : NomenclatureBase
    {
        private static readonly string[] METRIX =
        {
            "Gram", "Meter", "Liter", "Inch", "Feet", "Yard", "Mile",
            "Acre", "Pint", "Quart", "Gallon", "Ounce", "Ton", "Pound", "Bushel",
            "Day", "Minute", "Second", "Hour", "Month", "Year", "Century", "Watts", "Ohms",
            "Amps", "Horsepower", "Celsius", "Fahrenheit", "Rpm", "Mph"
        };

        public Metrix()
        {
            Synonyms.AddRange(METRIX);
        }
    }

    public class Identity : NomenclatureBase
    {
        private static readonly string[] IDENTITY = 
        {
            "Code", "Name", "Type", "Id", "Identity", "Index", "Key",
            "Class", "Mode", "Uid", "Guid"
        };

        public Identity()
        {
            Synonyms.AddRange(IDENTITY);
        }

        public override bool HasSemblance(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 1)
                return base.HasSemblance(variousStrings);

            var lastEntry = variousStrings.Last();
            return IDENTITY.Any(x => Regex.IsMatch(lastEntry, x, RegexOptions.IgnoreCase));
        }
    }
}
