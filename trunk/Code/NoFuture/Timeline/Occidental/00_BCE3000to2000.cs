namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE3000to2000()
        {
            var rule = new Rule {StartValue = 3000, EndValue = 1950, RuleLineSpacing = 5};

            var egypt = new Block {Ruler = rule, Title = "Egypt"};
            var mesopotamia = new Block {Ruler = rule, Title = "Mesopotamia"};

            egypt.AddEntry(2600, "III-IV Dynasties");
            egypt.AddEntry(2400, "(The Pyramid Age)");
            egypt.AddEntry(2200, "I Intermediate");
            egypt.AddEntry(2100, 1950, "Middle Kingdom");
            egypt.AddEntry(2900, 2300, "Old Kingdom");

            mesopotamia.AddEntry(2100, "Guti invasion");
            mesopotamia.AddEntry(2360, 2180, "Empire of Akkad");
            mesopotamia.AddEntry(2850, 2360, "Sumerian City States");
            mesopotamia.AddEntry(2060, 1950, "Ur III");

            var plate = new Plate {Name = "Near East 3000-2000 BCE", Ruler = rule};
            plate.AddBlock(egypt);
            plate.AddBlock(mesopotamia);

            return plate;
        }
    }
}
