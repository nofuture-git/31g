namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1865to1914()
        {
            var rule = new Rule {StartValue = 1864, EndValue = 1914, RuleLineSpacing = 7};

            //united states
            var usa = new Block {Ruler = rule, Title = "United States"};
            usa.AddEntry(new TerritoryEntry("NE") {StartValue = 1867});
            usa.AddEntry(new TerritoryEntry("CO") {StartValue = 1876});
            usa.AddEntry(new TerritoryEntry("ND,SD,MT,WA") {StartValue = 1889});
            usa.AddEntry(new TerritoryEntry("ID,WY") {StartValue = 1890});
            usa.AddEntry(new TerritoryEntry("UT") {StartValue = 1896});
            usa.AddEntry(new TerritoryEntry("OK") {StartValue = 1907});
            usa.AddEntry(new TerritoryEntry("NM,AZ") {StartValue = 1912});

            usa.AddEntry(new LeaderEntry("A.Johnson", new int?[,] {{null, 1869}}) {StartValue = 1869});
            usa.AddEntry(new LeaderEntry("U.Grant", new int?[,] {{null, 1877}}) {StartValue = 1871});
            usa.AddEntry(new LeaderEntry("R.Hayes", new int?[,] {{null, 1881}}) {StartValue = 1877});
            usa.AddEntry(new LeaderEntry("J.Garfield", new int?[,] {{null, 1881}}) {StartValue = 1881});
            usa.AddEntry(new LeaderEntry("C.Arthur", new int?[,] {{null, 1885}}) {StartValue = 1883});
            usa.AddEntry(new LeaderEntry("G.Cleveland", new int?[,] {{null, 1889}, {1893, 1897}}) {StartValue = 1888});
            usa.AddEntry(new LeaderEntry("B.Harrison", new int?[,] {{null, 1893}}) {StartValue = 1891});
            usa.AddEntry(new LeaderEntry("W.McKinley", new int?[,] {{1897, 1901}}) {StartValue = 1899});
            usa.AddEntry(new LeaderEntry("T.Roosevelt", new int?[,] {{null, 1909}}) {StartValue = 1905});
            usa.AddEntry(new LeaderEntry("W.Taft", new int?[,] {{null, 1913}}) {StartValue = 1910});

            usa.AddEntry(1870, "Transcontinental Rail(1869)", PrintLocation.Right);
            usa.AddEntry(1866, 1877, "Reconstruction(1866-1877)", PrintLocation.Right);
            usa.AddEntry(1882, "Chinese Exclusion Act(1882)", PrintLocation.Right);
            usa.AddEntry(1886, "Statue of Liberty(1886)", PrintLocation.Right);
            usa.AddEntry(1898, "Spanish-American War(1898)", PrintLocation.Right);
            usa.AddEntry(1900, "Bison nearly extinct", PrintLocation.Right);

            usa.AddEntry(new ScienceAdvEntry("airplane", "Wright bros.") {StartValue = 1903});
            usa.AddEntry(new ScienceAdvEntry("lightbulb", "Edison") {StartValue = 1879});
            usa.AddEntry(new ScienceAdvEntry("telephone", "Bell") {StartValue = 1874});
            usa.AddEntry(new LiteraryWorkEntry("Huckleberry Finn", "M.Twain") {StartValue = 1884});
            usa.AddEntry(new LiteraryWorkEntry("The Jungle", "Sinclair") {StartValue = 1906});
            usa.AddEntry(new LiteraryWorkEntry("The Call of the Wild", "London") {StartValue = 1904});

            //england
            var engd = new Block {Ruler = rule, Title = "United Kingdom", Width = 44};
            engd.AddEntry(new LeaderEntry("J.Russel", new int?[,] {{1865, 1866}}) {StartValue = 1866});
            engd.AddEntry(new LeaderEntry("E.Smith-Stanley", new int?[,] {{1866, 1868}}) {StartValue = 1867});
            engd.AddEntry(new LeaderEntry("B.Disraeli", new int?[,] {{null, 1868}, {1874, 1880}}) {StartValue = 1868});
            engd.AddEntry(new LeaderEntry("W.E.Gladstone",
                new int?[,] {{1868, 1874}, {1880, 1885}, {null, 1886}, {1892, 1894}}) {StartValue = 1869});
            engd.AddEntry(new LeaderEntry("R.Cecil", new int?[,] {{1885, 1886}, {1886, 1892}, {1895, 1902}})
            {
                StartValue = 1885
            });
            engd.AddEntry(new LeaderEntry("A.Primrose", new int?[,] {{1894, 1895}}) {StartValue = 1894});
            engd.AddEntry(new LeaderEntry("A.Balfour", new int?[,] {{1902, 1905}}) {StartValue = 1902});
            engd.AddEntry(new LeaderEntry("H.C.Bannerman", new int?[,] {{1905, 1908}}) {StartValue = 1905});
            engd.AddEntry(new LeaderEntry("H.Asquith", new int?[,] {{1908, 1916}}) {StartValue = 1908});

            engd.AddEntry(new ScienceAdvEntry("cathode rays", "Thomson") {StartValue = 1897});

            engd.AddEntry(1870, "Suez Canel(1869)");
            engd.AddEntry(1901, "Boxer Rebellion");

            engd.AddEntry(1879, "Anglo-Zulu War(1879)", PrintLocation.Left);
            engd.AddEntry(1881, "First Boer War(1880\\81)", PrintLocation.Left);
            engd.AddEntry(1899, 1902, "Second Boer War(1899-1902)", PrintLocation.Left);
            engd.AddEntry(1865, 1901, "Queen Victoria (1837-1901)", PrintLocation.Right);

            var fr = new Block {Ruler = rule, Title = "France", Width = 44};
            fr.AddEntry(new ScienceAdvEntry("radioactivity", "Becquerel") {StartValue = 1896});
            fr.AddEntry(1911, "Second Moroccan Crisis (1911)");
            fr.AddEntry(new LeaderEntry("Adolphe Thiers", new int?[,] {{1871, 1873}}) {StartValue = 1871});
            fr.AddEntry(new LeaderEntry("Patrice de MacMahon", new int?[,] {{1873, 1879}}) {StartValue = 1873});
            fr.AddEntry(new LeaderEntry("Jules Grevy", new int?[,] {{1879, 1887}}) {StartValue = 1879});
            fr.AddEntry(new LeaderEntry("M.F.S. Carnot", new int?[,] {{1887, 1894}}) {StartValue = 1887});
            fr.AddEntry(new LeaderEntry("Jean Casimir-Perier", new int?[,] {{1894, 1985}}) {StartValue = 1893});
            fr.AddEntry(new LeaderEntry("Felix Faure", new int?[,] {{1895, 1899}}) {StartValue = 1898});
            fr.AddEntry(new LeaderEntry("Emile Loubet", new int?[,] {{1899, 1906}}) {StartValue = 1900});
            fr.AddEntry(new LeaderEntry("Armand Fallieres", new int?[,] {{1906, 1913}}) {StartValue = 1906});

            var de = new Block {Ruler = rule, Title = "Germany", Width = 44};
            de.AddEntry(new ScienceAdvEntry("x-rays", "Rontgen") {StartValue = 1895});
            de.AddEntry(new ScienceAdvEntry("special relativity", "Einstein") {StartValue = 1905});
            de.AddEntry(new ScienceAdvEntry("continental drift", "Wegener") {StartValue = 1912});
            de.AddEntry(new LeaderEntry("Wilhem I (Monarch)", new int?[,] {{1861, 1888}}) {StartValue = 1867});
            de.AddEntry(new LeaderEntry("Otto von Bismarck", new int?[,] {{1871, 1890}}) {StartValue = 1872});
            de.AddEntry(new LeaderEntry("Wilhem II (Monarch)", new int?[,] {{1888, 1918}}) {StartValue = 1888});
            de.AddEntry(new LeaderEntry("Leo von Caprivi", new int?[,] {{1890, 1892}}) {StartValue = 1890});
            de.AddEntry(new LeaderEntry("B. zu Eulenburg", new int?[,] {{1892, 1894}}) {StartValue = 1892});
            de.AddEntry(new LeaderEntry("Chlodwig Carl Viktor", new int?[,] {{1894, 1900}}) {StartValue = 1894});
            de.AddEntry(new LeaderEntry("Bernhard von Bulow", new int?[,] {{1900, 1909}}) {StartValue = 1900});
            de.AddEntry(new LeaderEntry("T. Bethmann-Hollweg", new int?[,] {{1909, 1917}}) {StartValue = 1909});

            de.AddEntry(1884, "Berlin Conference(1884\\85)");

            de.AddEntry(1906, "First Moroccan Crisis (1905-06)");

            var aus = new Block {Ruler = rule, Title = "Austrian Empire"};
            aus.AddEntry(new LeaderEntry("Francis Joseph I", new int?[,] {{1867, 1916}}) {StartValue = 1869});
            aus.AddEntry(1908, "Bosnian Crisis (1908)");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire"};
            ott.AddEntry(1911, "Italo-Turkish War(1911-12)");
            ott.AddEntry(1912, "Balkan Wars (1912-13)");
            ott.AddEntry(new LeaderEntry("Abdulaziz", new int?[,] {{1861, 1876}}) {StartValue = 1866});
            ott.AddEntry(new LeaderEntry("Mehmend Murad V", new int?[,] {{null, 1876}}) {StartValue = 1876});
            ott.AddEntry(new LeaderEntry("Abdulhamid II", new int?[,] {{1876, 1909}}) {StartValue = 1880});
            ott.AddEntry(new LeaderEntry("Mehmed V", new int?[,] {{1909, 1918}}) {StartValue = 1909});

            var rus = new Block {Ruler = rule, Title = "Imperial Russia"};
            rus.AddEntry(new ScienceAdvEntry("perodic table", "Mendeleev") {StartValue = 1869});
            rus.AddEntry(1904, 1905, "Russo-Japanese War (1904-05)");
            rus.AddEntry(new LeaderEntry("Alexander II", new int?[,] {{1855, 1881}}) {StartValue = 1867});
            rus.AddEntry(new LeaderEntry("Alexander III", new int?[,] {{1881, 1894}}) {StartValue = 1881});
            rus.AddEntry(new LeaderEntry("Nicholas II", new int?[,] {{1894, 1917}}) {StartValue = 1894});

            var plate = new Plate {Ruler = rule, Name = "New Imperialism"};
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);

            plate.AddArrow(new Arrow(fr, de) {StartValue = 1870, Text = "Franco-Prussian War"});
            plate.AddArrow(new Arrow(aus, de)
            {
                StartValue = 1879,
                Text = "Dual Alliance (1879)**",
                FromRightToLeftArrowHead = "+````",
                ArrowShaft = '`',
                ArrowTail = "````+"
            });

            plate.AddArrow(new Arrow(rus, fr)
            {
                StartValue = 1892,
                Text = "Franco-Russian Alliance (1892)",
                FromRightToLeftArrowHead = "+``",
                ArrowShaft = '`',
                ArrowTail = "``+"
            });

            plate.AddArrow(new Arrow(fr, engd)
            {
                StartValue = 1904,
                Text = "Entente Cordiale (1904)",
                FromRightToLeftArrowHead = "+```````",
                ArrowShaft = '`',
                ArrowTail = "```````+"
            });

            plate.AddArrow(new Arrow(rus, engd)
            {
                StartValue = 1907,
                Text = "Anglo-Russian Entente (1907)",
                FromRightToLeftArrowHead = "+``",
                ArrowShaft = '`',
                ArrowTail = "``+"
            });
            plate.AddArrow(new Arrow(rus, engd)
            {
                StartValue = 1880,
                Text = "Europe-wide Arms Race begins ",
                ArrowTail = "............]",
                FromRightToLeftArrowHead = "[.."
            });
            plate.Notes.Add(" ** Italy is added in 1882 making it the 'Triple Alliance'.");

            return plate;
        }
    }
}
