namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE2000to1500()
        {
            var rule = new Rule {StartValue = 2100, EndValue = 1500,RuleLineSpacing = 5};
            var egypt = new Block {Ruler = rule, Title = "Egypt", Width = 22};
            egypt.AddEntry(2040,"XI Dynasty");
            egypt.AddEntry(1980,"XII Dynasty");
            egypt.AddEntry(1710,"The Hykos");
            egypt.AddEntry(1550,"New Kingdom (Empire)");
            egypt.AddEntry(1530,"XVIII Dynasty");
            egypt.AddEntry(2060,1800,"Middle Kingdom");
            egypt.AddEntry(1750,1580,"II Intermediate");

            var palestine = new Block {Ruler = rule, Title = "Palestine", Width = 16};
            palestine.AddEntry(1900,1700,"The Patriarchs");

            var hittities = new Block {Ruler = rule, Title = "Hittites", Width = 20};
            hittities.AddEntry(1700,"Hittite Old Kingdom");
            hittities.AddEntry(1650,"Labarna I");
            hittities.AddEntry(1600,"Hattusili I");
            hittities.AddEntry(1550,"Mursili I");
            hittities.AddEntry(1900,1600,"City States");

            var mesopotamia = new Block {Ruler = rule, Title = "Mesopotamia"};

            var assyria = new Block {Ruler = rule, Title = "Assyria",StartValue = 1950};
            assyria.AddEntry(1900,"Cappadocian Colonies");
            assyria.AddEntry(1750,"Shamshi-Adad I");

            var mari = new Block {Ruler = rule, Title = "Mari", StartValue = 1950, EndValue = 1700, Width = 16};
            mari.AddEntry(1750,"The 'Mari Age'");

            var isin = new Block {Ruler = rule, Title = "Isin", StartValue = 1950, EndValue = 1750, Width = 8};

            var larsa = new Block {Ruler = rule, Title = "Larsa", StartValue = 1950, EndValue = 1700, Width = 16};
            larsa.AddEntry(1758,"Rim-Sin I");

            var babylon = new Block {Ruler = rule, Title = "Babylon", StartValue = 1830, EndValue = 1550, Width = 20};
            babylon.AddEntry(1700, "Hammurabi");
            babylon.AddEntry(1630, "Kassite Dyn." );

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
            mesopotamia.AddEntry(new Entry { StartValue = 2060, EndValue = 1950, Text = "Ur III" });

            var plate = new Plate {Ruler = rule, Name = "Near East 2000-1500 BCE"};

            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(hittities);
            plate.AddBlock(mesopotamia);

            plate.AddArrow(new Arrow(hittities, babylon){StartValue = 1550, Text = "Babylon razed"});

            return plate;
        }
    }
}
