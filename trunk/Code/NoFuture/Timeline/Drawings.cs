using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Shared;

namespace NoFuture.Timeline
{
    public interface IDrawing
    {
        Rule Ruler { get; set; }
    }

    public interface IRuleEntry
    {
        double StartValue { get; set; }
        double EndValue { get; set; }
        int Width { get; set; }
        string Id { get; }
    }//end IRuleEntry

    [Serializable]
    public class TextRange
    {
        public string Id { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
    }//end TextRange

    //end Extensions

    #region kinds of entries
    [Serializable]
    public class TerritoryEntry : Entry
    {
        private const string REGEX_PATTERN = @"\+([^\(]*?)\(([0-9]*)\)";

        private string _name;

        public TerritoryEntry(string name)
        {
            _name = name;
            Location = PrintLocation.Left;
        }

        public string Name => _name;

        public override string Text
        {
            get
            {
                return string.Format("+{0}({1})", Name, (int)Math.Round(StartValue));
            }
            set
            {
                var tValue = value;
                string tName;
                string tYear;
                double iYear;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out tName, 1))
                {
                    _name = tName;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out tYear, 2) && double.TryParse(tYear, out iYear))
                {
                    StartValue = iYear;
                }
            }
        }
    }//end TerritoryEntry

    [Serializable]
    public class LeaderEntry : Entry
    {
        private const string REGEX_PATTERN = @"\[([a-zA-Z\.\x20\x5B\x5D]+?)\x20([0-9\x5C\x2d]+?)]";

        private string _name;
        private IEnumerable<Tuple<double?, double?>> _years;
        public LeaderEntry(string name, int?[,] years)
        {
            var yy = new List<Tuple<double?, double?>>();
            for (var i = 0; i < years.GetLongLength(0); i++)
            {
                yy.Add(new Tuple<double?, double?>(years[i,0], years[i,1]));
            }
            _name = name;
            _years = yy;
        }

        public LeaderEntry(string name, int year):this(name, new int?[,] { {null, year } })
        {
            StartValue = year;
        }

        public string Name => _name;
        public IEnumerable<Tuple<double?, double?>> Years => _years;

        public override string Text
        {
            get
            {
                return string.Format("[{0} {1}]", Name, Ruler.PrintYearsRange(Years));
            }
            set
            {
                string s1;
                var tValue = value;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    _name = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    _years = Ruler.ParseYearsRange(s1);
                }
            }
        }
    }//end LeaderEntry

    [Serializable]
    public class ScienceAdvEntry : Entry
    {
        private const string REGEX_PATTERN = @"([a-zA-Z\.\x20]+?)\[([a-zA-Z\.\x20\x2C]+?)\]\(([0-9]+?)\)";

        private string _discoveredBy;
        private string _name;

        public ScienceAdvEntry(string name, string discoveredBy)
        {
            _discoveredBy = discoveredBy;
            _name = name;
        }

        public string DiscoveredBy => _discoveredBy;
        public string Name => _name;

        public override string Text
        {
            get
            {
                return $"{DiscoveredBy}[{Name}]({(int) Math.Round(StartValue)})";
            }
            set
            {
                var tValue = value;
                string s1;
                double y1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    _discoveredBy = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    _name = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 3) && double.TryParse(s1, out y1))
                {
                    StartValue = y1;
                }
            }
        }
    }//end ScienceAdvEntry

    [Serializable]
    public class LiteraryWorkEntry : Entry
    {
        private const string REGEX_PATTERN = @"\'([a-zA-Z\.\x20\x2D\x2C]+?)\'([a-zA-Z\.\x20]+?)\(([0-9]+?)\)";

        private string _title;
        private string _author;

        public LiteraryWorkEntry(string title, string author)
        {
            _title = title;
            _author = author;
        }

        public string Title => _title;
        public string Author => _author;

        public override string Text
        {
            get
            {
                return $"'{Title}'{Author}({(int) Math.Round(StartValue)})";
            }
            set
            {
                var tValue = value;
                string s1;
                double y1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    _title = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    _author = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 3) && double.TryParse(s1, out y1))
                {
                    StartValue = y1;
                }
            }
        }
    }//end LiteraryWorkEntry

    [Serializable]
    public class ExplorerEntry : Entry
    {
        private const string REGEX_PATTERN = @"\x5B([a-zA-Z\.\x20]+?)\x5D\x2D([a-zA-Z\x2E\x26\x20\x2C]+)\x28([0-9\x2F\x2D\x5C]+)\x29";

        public string ExplorerName { get; set; }
        public string Area { get; set; }

        public ExplorerEntry(string explorerName, string area)
        {
            ExplorerName = explorerName;
            Area = area;
        }

        public override string Text
        {
            get
            {
                var dkYear = PrintStartEndValue();
                var txt = $"[{ExplorerName}]-{Area}";
                if (!string.IsNullOrWhiteSpace(dkYear))
                    txt += $"({dkYear})";
                return txt;
            }
            set
            {
                var tValue = value;
                string s1;
                double y1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    ExplorerName = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    Area = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 3))
                {

                    AssignStartEndValueByString(s1);
                }
            }
        }
    }//end ExplorerEntry

    [Serializable]
    public class HeresyEntry : Entry
    {
        private const string REGEX_PATTERN =
            @"\x5BH\x5D\x20([a-zA-Z\x2E\x26\x20\x2C]+)\x20\x28([a-zA-Z\x2E\x26\x20\x2C]+)\x29";

        public HeresyEntry() { }

        public HeresyEntry(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public override string Text
        {
            get
            {
                return $"[H] {Name} ({Description})";
            }
            set
            {
                var tValue = value;
                string s1;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 1))
                {
                    Name = s1;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out s1, 2))
                {
                    Description = s1;
                }
            }
        }
    }//end HeresyEntry

    [Serializable]
    public class ProphetEntry : Entry
    {
        private const string REGEX_PATTERN = @"\x28([A-Za-z\x20\x2C\x3F]+)\x29";
        public string Name { get; set; }

        public ProphetEntry() { }

        public ProphetEntry(string name)
        {
            Name = name;
        }

        public override string Text
        {
            get { return $"({Name})"; }
            set
            {
                var tValue = value;
                string name;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out name, 1))
                {
                    Name = name;
                }
            }
        }
    }

    [Serializable]
    public class PhilosopherEntry : Entry
    {
        private const string REGEX_PATTERN = @"\x28([A-Za-z\x20\x2C]+)\x20([0-9]+\x2D[0-9]+)?\x29";

        public string Name { get; set; }
        public int? Yob { get; set; }
        public int? Yod { get; set; }

        public PhilosopherEntry() { }

        public PhilosopherEntry(string name, int? yob, int? yod)
        {
            Name = name;
            Yob = yob;
            Yod = yod;
        }

        public override string Text
        {
            get
            {
                var v = new StringBuilder();
                v.Append($"({Name}");
                var ls = $" {Yob}-{Yod}";
                if (ls.Length > 2)
                    v.Append(ls);
                v.Append(")");
                return v.ToString();
            }
            set
            {
                var tValue = value;
                string name;
                string ls;
                int yob;
                int yod;
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out name, 1))
                {
                    Name = name;
                }
                if (RegexCatalog.IsRegexMatch(tValue, REGEX_PATTERN, out ls, 2) &&
                    int.TryParse(ls.Split('-')[0], out yob) && int.TryParse(ls.Split('-')[1], out yod))
                {
                    Yob = yob;
                    Yod = yod;
                }
            }
        }
    }

    [Serializable]
    public class PeriodEntry : Entry { } 

    [Serializable]
    public class SimpleFactEntry : Entry { }
    #endregion

    //end Arrow

    [Serializable]
    public enum PrintLocation
    {
        Center,
        Left,
        Right

    }//end PrintLocation


}//end NoFuture.Timeline
