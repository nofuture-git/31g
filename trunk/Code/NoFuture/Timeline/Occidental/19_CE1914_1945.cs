using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1914to1945()
        {
            var rule = new Rule { StartValue = 1914, EndValue = 1946, RuleLineSpacing = 7 };
            var usa = new Block { Ruler = rule, Title = "United States" };
            

            var engd = new Block { Ruler = rule, Title = "United Kingdom"};

            var fr = new Block { Ruler = rule, Title = "France"};

            var de = new Block { Ruler = rule, Title = "Germany"};

            var aus = new Block { Ruler = rule, Title = "Austrian Empire", EndValue = 1918};

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire", EndValue = 1922};

            var rus = new Block { Ruler = rule, Title = "Imperial Russia", EndValue = 1917};

            var ussr = new Block {Ruler = rule, Title = "Soviet Union", StartValue = 1922};

            var zho = new Block {Ruler = rule, Title = "Republic of China"};

            var jpn = new Block {Ruler = rule, Title = "Japan"};

            var plate = new Plate { Ruler = rule, Name = "World Wars" };

            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ussr);
            plate.AddBlock(ott);
            plate.AddBlock(rus);
            plate.AddBlock(zho);
            plate.AddBlock(jpn);

            plate.AddArrow(new Arrow(fr, de) { StartValue = 1916, Text = "Battle of Verdun(1916)" });
            plate.AddArrow(new Arrow(engd, de) {StartValue = 1917, Text = "Battle of Passchendaele(1917)"});

            //Battle of Arras
            //Battle of Tannenberg
            //The Battle of Verdun
            //The Battle of Passchendaele
            //Battle of the Somme
            return plate;
        }
    }
}
