namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE2000to1500()
        {
            var rule = new Rule() {StartValue = 2100, EndValue = 1500,RuleLineSpacing = 5};
            var egypt = new Block() {Ruler = rule, Title = "Egypt", Width = 22};
            egypt.AddEntry(new Entry(){StartValue = 2040,Text = "XI Dynasty"});
            egypt.AddEntry(new Entry(){StartValue = 1980, Text = "XII Dynasty"});
            egypt.AddEntry(new Entry(){StartValue = 1710, Text = "The Hykos"});
            egypt.AddEntry(new Entry(){StartValue = 1550,Text = "New Kingdom (Empire)"});
            egypt.AddEntry(new Entry(){StartValue = 1530, Text = "XVIII Dynasty"});
            egypt.AddEntry(new Entry() { StartValue = 2060, EndValue = 1800, Text = "Middle Kingdom" });
            egypt.AddEntry(new Entry() { StartValue = 1750, EndValue = 1580, Text = "II Intermediate" });

            var palestine = new Block() {Ruler = rule, Title = "Palestine", Width = 16};
            palestine.AddEntry(new Entry(){StartValue = 1900, EndValue = 1700, Text = "The Patriarchs"});

            var hittities = new Block() {Ruler = rule, Title = "Hittites", Width = 20};
            hittities.AddEntry(new Entry(){StartValue = 1700,Text = "Hittite Old Kingdom"});
            hittities.AddEntry(new Entry(){StartValue = 1650, Text = "Labarna I"});
            hittities.AddEntry(new Entry(){StartValue = 1600, Text = "Hattusili I"});
            hittities.AddEntry(new Entry(){StartValue = 1550, Text = "Mursili I"});
            hittities.AddEntry(new Entry() { StartValue = 1900, EndValue = 1600, Text = "City States" });

            var mesopotamia = new Block() {Ruler = rule, Title = "Mesopotamia"};

            var assyria = new Block() {Ruler = rule, Title = "Assyria",StartValue = 1950};
            assyria.AddEntry(new Entry(){StartValue = 1900, Text = "Cappadocian Colonies"});
            assyria.AddEntry(new Entry(){StartValue = 1750, Text="Shamshi-Adad I"});

            var mari = new Block() {Ruler = rule, Title = "Mari", StartValue = 1950, EndValue = 1700, Width = 16};
            mari.AddEntry(new Entry(){StartValue = 1750,Text = "The 'Mari Age'"});

            var isin = new Block() {Ruler = rule, Title = "Isin", StartValue = 1950, EndValue = 1750, Width = 8};

            var larsa = new Block() {Ruler = rule, Title = "Larsa", StartValue = 1950, EndValue = 1700, Width = 16};
            larsa.AddEntry(new Entry(){StartValue = 1758, Text = "Rim-Sin I"});

            var babylon = new Block() {Ruler = rule, Title = "Babylon", StartValue = 1830, EndValue = 1550, Width = 20};
            babylon.AddEntry(new Entry(){StartValue = 1700, Text = "Hammurabi"});
            babylon.AddEntry(new Entry() { StartValue = 1630, Text = "Kassite Dyn." });

            mesopotamia.AddInnerBlock(assyria);
            mesopotamia.AddInnerBlock(mari);
            mesopotamia.AddInnerBlock(isin);
            mesopotamia.AddInnerBlock(larsa);
            mesopotamia.AddInnerBlock(babylon);

            mesopotamia.AddArrow(new Arrow(larsa, isin) { StartValue = 1750, ArrowTail = ".." });
            mesopotamia.AddArrow(new Arrow(assyria, mari) { StartValue = 1720, ArrowTail = ".." });
            mesopotamia.AddArrow(new Arrow(babylon, assyria) { StartValue = 1720, ArrowTail = ".." });
            mesopotamia.AddArrow(new Arrow(babylon,mari){StartValue = 1700, ArrowTail = ".."});
            mesopotamia.AddArrow(new Arrow(babylon,larsa){StartValue = 1700, ArrowTail = ".."});
            mesopotamia.AddEntry(new Entry() { StartValue = 2060, EndValue = 1950, Text = "Ur III" });

            var plate = new Plate() {Ruler = rule, Name = "Near East 2000-1500 BCE"};

            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(hittities);
            plate.AddBlock(mesopotamia);

            plate.AddArrow(new Arrow(hittities, babylon){StartValue = 1550, Text = "Babylon razed"});

            return plate;
        }
    }
}
