namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1865to1914()
        {
            var rule = new Rule { StartValue = 1864, EndValue = 1914, RuleLineSpacing = 7 };

            //united states
            var usa = new Block { Ruler = rule, Title = "United States" };
            usa.AddEntry(new TerritoryEntry("NE") { StartValue = 1867 });
            usa.AddEntry(new TerritoryEntry("CO") { StartValue = 1876 });
            usa.AddEntry(new TerritoryEntry("ND,SD,MT,WA") { StartValue = 1889 });
            usa.AddEntry(new TerritoryEntry("ID,WY") { StartValue = 1890 });
            usa.AddEntry(new TerritoryEntry("UT") { StartValue = 1896 });
            usa.AddEntry(new TerritoryEntry("OK") { StartValue = 1907 });
            usa.AddEntry(new TerritoryEntry("NM,AZ") { StartValue = 1912 });

            usa.AddEntry(new LeaderEntry("A.Johnson", new int?[,] { { null, 1869 } } ) {StartValue = 1869});
            usa.AddEntry(new LeaderEntry("U.Grant", new int?[,] { { null, 1877} }) { StartValue = 1871 });
            usa.AddEntry(new LeaderEntry("R.Hayes", new int?[,] { { null, 1881 } }) { StartValue = 1877 });
            usa.AddEntry(new LeaderEntry("J.Garfield", new int?[,] { { null, 1881 } }) { StartValue = 1881 });
            usa.AddEntry(new LeaderEntry("C.Arthur", new int?[,] { { null, 1885 } }) { StartValue = 1883 });
            usa.AddEntry(new LeaderEntry("G.Cleveland", new int?[,] { { null, 1889 }, {1893,1897} }) { StartValue = 1888 });
            usa.AddEntry(new LeaderEntry("B.Harrison", new int?[,] { { null, 1893 } }) { StartValue = 1891 });
            usa.AddEntry(new LeaderEntry("W.McKinley", new int?[,] { { 1897, 1901 } }) { StartValue = 1899 });
            usa.AddEntry(new LeaderEntry("T.Roosevelt", new int?[,] { {null, 1909} }) {StartValue = 1905});
            usa.AddEntry(new LeaderEntry("W.Taft", new int?[,] { { null, 1913 } } ) {StartValue = 1910});

            usa.AddEntry(1870, "Transcontinental Rail(1869)", PrintLocation.Right);
            usa.AddEntry(1866,1877, "Reconstruction(1866-1877)", PrintLocation.Right);
            usa.AddEntry(1882,"Chinese Exclusion Act(1882)", PrintLocation.Right);
            usa.AddEntry(1886, "Statue of Liberty(1886)", PrintLocation.Right);
            usa.AddEntry(1898, "Spanish-American War(1898)", PrintLocation.Right);
            usa.AddEntry(1900, "Bison nearly extinct", PrintLocation.Right);

            usa.AddEntry(new ScienceAdvEntry("airplane", "Wright bros.") {StartValue = 1903});
            usa.AddEntry(new ScienceAdvEntry("lightbulb", "Edison") {StartValue = 1879});
            usa.AddEntry(new ScienceAdvEntry("telephone", "Bell"){StartValue = 1874});
            usa.AddEntry(new LiteraryWorkEntry("Huckleberry Finn", "M.Twain") {StartValue = 1884});
            usa.AddEntry(new LiteraryWorkEntry("The Jungle", "Sinclair") {StartValue = 1906});
            usa.AddEntry(new LiteraryWorkEntry("The Call of the Wild", "London") {StartValue = 1904});

            //england
            var engd = new Block { Ruler = rule, Title = "United Kingdom", Width = 44};
            engd.AddEntry(new LeaderEntry("J.Russel", new int?[,] { { 1865, 1866 } }) { StartValue = 1866 });
            engd.AddEntry(new LeaderEntry("E.Smith-Stanley", new int?[,] { { 1866, 1868 } }) { StartValue = 1867 });
            engd.AddEntry(new LeaderEntry("B.Disraeli", new int?[,] { { null, 1868 }, {1874, 1880} }) { StartValue = 1868 });
            engd.AddEntry(new LeaderEntry("W.E.Gladstone", new int?[,] { { 1868, 1874 }, {1880,1885}, {null,1886}, {1892,1894} }) { StartValue = 1869 });
            engd.AddEntry(new LeaderEntry("R.Cecil", new int?[,] { { 1885, 1886 }, { 1886, 1892 }, {1895,1902} }) { StartValue = 1885 });
            engd.AddEntry(new LeaderEntry("A.Primrose", new int?[,] { { 1894, 1895 } }) { StartValue = 1894 });
            engd.AddEntry(new LeaderEntry("A.Balfour", new int?[,] { { 1902, 1905 } }) { StartValue = 1902 });
            engd.AddEntry(new LeaderEntry("H.C.Bannerman", new int?[,] { { 1905, 1908 } }) { StartValue = 1905 });
            engd.AddEntry(new LeaderEntry("H.Asquith", new int?[,] { { 1908, 1916 } }) { StartValue = 1908 });

            engd.AddEntry(new ScienceAdvEntry("cathode rays","Thomson") {StartValue = 1897});

            engd.AddEntry(1870, "Suez Canel(1869)");

            engd.AddEntry(1879, "Anglo-Zulu War(1879)", PrintLocation.Left);
            engd.AddEntry(1880,"First Boer War(1880\\81)", PrintLocation.Left);
            engd.AddEntry(1899, 1902, "Second Boer War(1899-1902)", PrintLocation.Left);
            engd.AddEntry(1865, 1901, "Queen Victoria (1837-1901)", PrintLocation.Right);

            var fr = new Block { Ruler = rule, Title = "France" };
            fr.AddEntry(new ScienceAdvEntry("radioactivity", "Becquerel") {StartValue = 1896});

            var de = new Block { Ruler = rule, Title = "Germany" };
            de.AddEntry(new ScienceAdvEntry("x-rays", "Rontgen") {StartValue = 1895});
            de.AddEntry(new ScienceAdvEntry("special relativity","Einstein") {StartValue = 1905});
            de.AddEntry(new ScienceAdvEntry("continental drift", "Wegener") {StartValue = 1912});
            de.AddEntry(1884,"Berlin Conference(1884\\85)");

            var aus = new Block { Ruler = rule, Title = "Austrian Empire" };

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire" };

            var rus = new Block { Ruler = rule, Title = "Imperial Russia" };
            rus.AddEntry(new ScienceAdvEntry("perodic table", "Mendeleev") {StartValue = 1869});

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

            plate.AddArrow(new Arrow(fr, de) { StartValue = 1870, Text = "Franco-Prussian War"});

            return plate;
        }
    }
}
