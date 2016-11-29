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
            var usa = new Block { Ruler = rule, Title = "United States", Width = 50};
            usa.AddEntry(1917, "enters WWI", PrintLocation.Center);
            usa.AddEntry(new LeaderEntry("Woodrow Wilson", new int?[,] { { 1913, 1921 } }) { StartValue = 1914.6 });
            usa.AddEntry(new LeaderEntry("Warren G.Harding", new int?[,] { { 1921, 1923 } }) { StartValue = 1921 });
            usa.AddEntry(new LeaderEntry("Calvin Coolidge", new int?[,] { { 1923, 1929 } }) { StartValue = 1923 });
            usa.AddEntry(new LeaderEntry("Herbert Hoover", new int?[,] { { 1929, 1933 } }) { StartValue = 1929 });
            usa.AddEntry(new LeaderEntry("F.D. Roosevelt", new int?[,] { { 1933, 1945 } }) { StartValue = 1933 });
            usa.AddEntry(1920,"Women gain voting rights(1920)");
            usa.AddEntry(1920.6,1933,"Prohibition(1920/33)", PrintLocation.Left);
            usa.AddEntry(1924,"Immigration Act(1924)[4]");
            usa.AddEntry(1929.6, "Stock Market Crash(1929)", PrintLocation.Right);
            usa.AddEntry(1930,1935,"Great Depression", PrintLocation.Right);
            usa.AddEntry(1933.3,1935,"Dust Bowl(1933/35)", PrintLocation.Left);
            usa.AddEntry(1935, "Social Security Act(1935)");
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
            rus.AddEntry(1922,"total devastation[2]");

            var ussr = new Block {Ruler = rule, Title = "Soviet Union", StartValue = 1917};
            ussr.AddEntry(1917.6,"Bolshevik Revolution(1917)");
            ussr.AddEntry(1918,"Brest-Litovsk treaty(1918)");
            ussr.AddEntry(1919.6, "Polish-Soviet War(1919\\20)");
            ussr.AddEntry(new LeaderEntry("Lenin", new int?[,] { {null, 1924} }) {StartValue = 1922});
            ussr.AddEntry(new LeaderEntry("Stalin", new int?[,] { { 1924, 1953 } }) { StartValue = 1924 });
            ussr.AddEntry(1928,1932,"First Five Year Plan", PrintLocation.Right);
            ussr.AddEntry(1932,1933,"Soviet famine", PrintLocation.Left);
            ussr.AddEntry(1933,1937,"Second Five Year Plan", PrintLocation.Right);
            ussr.AddEntry(1936,1938,"Great Purge[3]", PrintLocation.Left);

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
            plate.AddArrow(new Arrow(de,fr) {StartValue = 1915, Text = "2nd Battle of Ypres(1915)[1]"});
            
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
            plate.Notes.Add(" [1] First lethal use of chemical weapons.");
            plate.Notes.Add(" [2] war and famine, millions dead, industry ruined, skilled and edu workers fled");
            plate.Notes.Add(" [3] 600K-1.2M deaths estimated.");
            plate.Notes.Add(" [4] East & South European: limited, African: severely limited, Asian and Arab: banned, Latin American: no restriction");
            return plate;
        }
    }
}
