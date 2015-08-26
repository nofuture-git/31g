namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE3000to2000()
        {
            var rule = new Rule() {StartValue = 3000, EndValue = 1950, RuleLineSpacing = 5};

            var egypt = new Block() {Ruler = rule, Title = "Egypt"};
            var mesopotamia = new Block() {Ruler = rule, Title = "Mesopotamia"};

            egypt.AddEntry(new Entry(){StartValue = 2600, Text = "III-IV Dynasties"});
            egypt.AddEntry(new Entry(){StartValue = 2400, Text = "(The Pyramid Age)"});
            egypt.AddEntry(new Entry(){StartValue = 2200, Text = "I Intermediate"});
            egypt.AddEntry(new Entry(){StartValue = 2100, EndValue = 1950,Text = "Middle Kingdom"});
            egypt.AddEntry(new Entry() { StartValue = 2900, EndValue = 2300, Text = "Old Kingdom" });

            mesopotamia.AddEntry(new Entry(){StartValue = 2100, Text = "Guti invasion"});
            mesopotamia.AddEntry(new Entry() { StartValue = 2360, EndValue = 2180, Text = "Empire of Akkad" });
            mesopotamia.AddEntry(new Entry() { StartValue = 2850, EndValue = 2360, Text = "Sumerian City States" });
            mesopotamia.AddEntry(new Entry() { StartValue = 2060, EndValue = 1950, Text = "Ur III" });

            var plate = new Plate() {Name = "Near East 3000-2000 BCE", Ruler = rule};
            plate.AddBlock(egypt);
            plate.AddBlock(mesopotamia);

            return plate;
        }
    }
}
