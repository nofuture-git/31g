using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1865to1914()
        {
            var rule = new Rule { StartValue = 1865, EndValue = 1914, RuleLineSpacing = 7 };

            var usa = new Block { Ruler = rule, Title = "United States" };
            usa.AddEntry(new TerritoryEntry { Name = "NE", StartValue = 1867 });
            usa.AddEntry(new TerritoryEntry { Name = "CO", StartValue = 1876 });
            usa.AddEntry(new TerritoryEntry { Name = "ND,SD,MT,WA", StartValue = 1889 });
            usa.AddEntry(new TerritoryEntry { Name = "ID,WY", StartValue = 1890 });
            usa.AddEntry(new TerritoryEntry { Name = "UT", StartValue = 1896 });
            usa.AddEntry(new TerritoryEntry { Name = "OK", StartValue = 1907 });
            usa.AddEntry(new TerritoryEntry { Name = "NM,AZ", StartValue = 1912 });

            var engd = new Block { Ruler = rule, Title = "United Kingdom" };
            engd.AddEntry(1899,1902, "Second Boer War");

            var fr = new Block { Ruler = rule, Title = "France" };

            var de = new Block { Ruler = rule, Title = "Germany" };

            var aus = new Block { Ruler = rule, Title = "Austrian Empire" };

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire" };

            var rus = new Block { Ruler = rule, Title = "Imperial Russia" };

            var jpn = new Block {Ruler = rule, Title = "Japan"};

            var zho = new Block {Ruler = rule, Title = "China"};



            var plate = new Plate { Ruler = rule, Name = "New Imperialism" };
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);
            plate.AddBlock(jpn);
            plate.AddBlock(zho);

            throw new NotImplementedException();
        }
    }
}
