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
            usa.AddEntry(1917, "enters WWI", PrintLocation.Center);

            var engd = new Block { Ruler = rule, Title = "United Kingdom"};

            var fr = new Block { Ruler = rule, Title = "France"};
            fr.AddEntry(1919, "Treaty of Versailles(1919)");

            var de = new Block { Ruler = rule, Title = "Germany"};

            var aus = new Block { Ruler = rule, Title = "Austrian Empire", EndValue = 1918};

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire", EndValue = 1922};

            var rus = new Block { Ruler = rule, Title = "Imperial Russia", EndValue = 1922};
            rus.AddEntry(1918,"Tsar & family executed(1918)");
            rus.AddEntry(1918.6, "Red Terror");
            rus.AddEntry(1920,"Whites retreat east(1920)");
            rus.AddEntry(1922,"total devastation **");

            var ussr = new Block {Ruler = rule, Title = "Soviet Union", StartValue = 1917};
            ussr.AddEntry(1917.6,"Bolshevik Revolution(1917)");
            ussr.AddEntry(1918,"Brest-Litovsk treaty(1918)");
            ussr.AddEntry(1919.6, "Polish-Soviet War(1919\\20)");
            ussr.AddEntry(new LeaderEntry("Lenin", new int?[,] { {null, 1924} }) {StartValue = 1922});
            ussr.AddEntry(new LeaderEntry("Stalin", new int?[,] { { 1924, 1953 } }) { StartValue = 1924 });
            ussr.AddEntry(1928,1932,"First Five Year Plan");
            ussr.AddEntry(1933,1937,"Second Five Year Plan");

            var zho = new Block {Ruler = rule, Title = "Republic of China"};

            var jpn = new Block {Ruler = rule, Title = "Japan"};

            var plate = new Plate { Ruler = rule, Name = "The World Wars" };

            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ussr);
            plate.AddBlock(rus);
            plate.AddBlock(ott);
            plate.AddBlock(zho);
            plate.AddBlock(jpn);

            plate.AddArrow(new Arrow(de, rus) { StartValue = 1914.6, Text = "Battle of Tannenberg(1914)" });
            plate.AddArrow(new Arrow(de,fr) {StartValue = 1915, Text = "2nd Battle of Ypres(1915)*"});
            
            plate.AddArrow(new Arrow(fr, de) { StartValue = 1916, Text = "Battle of Verdun(1916)" });
            plate.AddArrow(new Arrow(fr, de) { StartValue = 1916.6, Text = "Battle of Somme(1916)" });
            plate.AddArrow(new Arrow(engd, de) {StartValue = 1917, Text = "Battle of Passchendaele(1917)"});
            plate.AddArrow(new Arrow(engd, de) { StartValue = 1917.6, Text = "Battle of Arras(1917)" });
            plate.AddArrow(new Arrow(engd,de) {StartValue = 1918, Text = "The Spring Offensive(1918)"});
            plate.AddArrow(new Arrow(usa,de) {StartValue = 1918.6, Text = "Hundred Days Offensive(1918)"});
            plate.AddArrow(new Arrow(ussr,rus) {StartValue = 1919, Text ="Keiv captured(1919)"});
            //Battle of Arras
            //Battle of Tannenberg
            //The Battle of Verdun
            //The Battle of Passchendaele
            //Battle of the Somme
            plate.Notes.Add(" * First lethal use of chemical weapons.");
            plate.Notes.Add(" ** war and famine, millions dead, industry ruined, skilled and edu workers fled");
            return plate;
        }
    }
}
