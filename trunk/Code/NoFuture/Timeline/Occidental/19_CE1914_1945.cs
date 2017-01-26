namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1914to1945()
        {
            var rule = new Rule {StartValue = 1914, EndValue = 1946, RuleLineSpacing = 7};

            var usa = new Block {Ruler = rule, Title = "United States", Width = 36};
            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            var fr = new Block {Ruler = rule, Title = "France", Width = 36};
            var de = new Block {Ruler = rule, Title = "Germany"};
            var ott = new Block {Ruler = rule, Title = "Ottoman Empire", EndValue = 1922};
            var rus = new Block {Ruler = rule, Title = "Imperial Russia", EndValue = 1922};
            var ussr = new Block {Ruler = rule, Title = "Soviet Union", StartValue = 1917};
            var zho = new Block {Ruler = rule, Title = "Republic of China"};
            var jpn = new Block {Ruler = rule, Title = "Japan"};

            var plate = new Plate {Ruler = rule, Name = "The World Wars"};

            plate.AddBlock(zho);
            plate.AddBlock(jpn);
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(ussr);
            plate.AddBlock(rus);
            plate.AddBlock(ott);
            plate.FileName = "19_CE1914_1945";

            usa.AddEntry(new LeaderEntry("Woodrow Wilson", new int?[,] {{1913, 1921}}) {StartValue = 1914.6});
            usa.AddEntry(1917, "enters WWI", PrintLocation.Center);
            usa.AddEntry(1920, "Women gain voting rights(1920)");
            usa.AddEntry(new LeaderEntry("Warren G.Harding", new int?[,] {{1921, 1923}}));
            usa.AddEntry(new ScienceAdvEntry("insulin", "F.Banting", 1922));
            usa.AddEntry(new LeaderEntry("Calvin Coolidge", new int?[,] {{1923, 1929}}));
            usa.AddEntry(1924, "Immigration Act(1924)[4]");
            usa.AddEntry(new ScienceAdvEntry("intergalatic", "Hubble") {StartValue = 1924.6});
            usa.AddEntry(new LiteraryWorkEntry("The Great Gatsby", "F.S.Fitzgerald", 1925));
            usa.AddEntry(new LeaderEntry("Herbert Hoover", new int?[,] {{1929, 1933}}));
            usa.AddEntry(1929.6, "Stock Market Crash(1929)", PrintLocation.Right);
            usa.AddEntry(1930, 1935, "Great Depression", PrintLocation.Right);
            usa.AddEntry(1932, "Bonus Army Conflict(1932)");
            usa.AddEntry(new LeaderEntry("F.D. Roosevelt", new int?[,] {{1933, 1945}}) {Location = PrintLocation.Center});
            usa.AddEntry(1933.3, 1935, "Dust Bowl(1933/35)", PrintLocation.Left);
            usa.AddEntry(1935, "Social Security Act(1935)");
            usa.AddEntry(new LiteraryWorkEntry("Gone\\w.the Wind", "M.Mitchell",1936));
            usa.AddEntry(new LiteraryWorkEntry("Of Mice and Men", "J.Steinbeck",1937));
            usa.AddEntry(new LiteraryWorkEntry("Grapes of Wrath", "J.Steinbeck",1939));
            usa.AddEntry(new LiteraryWorkEntry("For Whom the Bell Tolls", "E.Hemingway",1940));
            usa.AddEntry(1942.6,1945, "Internment Japanese Amer.");
            usa.AddEntry(1920.6, 1933, "Prohibition(1920/33)", PrintLocation.Left);
            plate.AddArrow(new Arrow(usa, jpn) {StartValue = 1943, Text = "Battle of Midway"});
            plate.AddArrow(new Arrow(usa,jpn) {StartValue = 1945.6, Text = "atomic bombs dropped"});
            plate.AddArrow(new Arrow(usa,de) {StartValue = 1944, Text = "Normandy invasion"});

            engd.AddEntry(new LeaderEntry("David L. George", new int?[,] {{1916, 1922}}));
            engd.AddEntry(new LeaderEntry("Bonar Law", new int?[,] {{1922, 1923}}));
            engd.AddEntry(new LeaderEntry("Stanley Baldwin", new int?[,] {{1923, 1924}, {1934, 1929}, {1935, 1937}}));
            engd.AddEntry(new LeaderEntry("Ramsay MacDonald", new int?[,] {{null, 1924}, {1929, 1935}})
            {
                StartValue = 1924
            });
            engd.AddEntry(new ScienceAdvEntry("Penicillin", "A.Fleming",1928));
            engd.AddEntry(new LiteraryWorkEntry("Brave New World", "A.Huxley", 1932));
            engd.AddEntry(1936, "Battle of Cable Street(1936)");
            engd.AddEntry(new LeaderEntry("Neville Chamberlain", new int?[,] {{1937, 1940}}));
            engd.AddEntry(new LiteraryWorkEntry("The Hobbit", "Tolkien",1937) {StartValue = 1937.6});
            engd.AddEntry(new LeaderEntry("Winston Churchill", new int?[,] {{1940, 1945}}) {StartValue = 1939});
            engd.AddEntry(new ScienceAdvEntry("Frisch-Peierls memo", "(#)",1941));
            engd.AddEntry(new LiteraryWorkEntry("Road.Serfdom","Hayek",1944));
            engd.AddEntry(new LiteraryWorkEntry("Animal Farm", "G.Orwell") {StartValue = 1945.6});
            plate.AddArrow(new Arrow(engd,de) {StartValue = 1943.7, Text = "Invasion of Italy"});

            fr.AddEntry(new LeaderEntry("Raymond Poincare", new int?[,] {{1913, 1920}}) {StartValue = 1914.6});
            plate.AddArrow(new Arrow(de, fr) {StartValue = 1915, Text = "2nd Battle of Ypres(1915)[1]"});
            fr.AddEntry(1919, "Treaty of Versailles(1919)");
            fr.AddEntry(new LeaderEntry("Paul Deschanel", new int?[,] {{null, 1920}}) {StartValue = 1920});
            fr.AddEntry(new LeaderEntry("Alexandre Millerand", new int?[,] {{1920, 1924}}) {StartValue = 1922});
            fr.AddEntry(new LeaderEntry("Gaston Doumergue", new int?[,] {{1924, 1931}}));
            fr.AddEntry(new ScienceAdvEntry("big bang theory", "G.Lemaitre",1927));
            fr.AddEntry(new LeaderEntry("Paul Doumer", new int?[,] {{1931, 1932}}));
            fr.AddEntry(new LeaderEntry("Albert Lebrun", new int?[,] {{1932, 1940}}));
            fr.AddEntry(1934, "anti-parliamentarist riots(1934)");

            de.AddEntry(new ScienceAdvEntry("general relativity", "Einstein") {StartValue = 1915.6});
            plate.AddArrow(new Arrow(fr, de) {StartValue = 1916, Text = "Battle of Verdun(1916)"});
            plate.AddArrow(new Arrow(fr, de) {StartValue = 1916.6, Text = "Battle of Somme(1916)"});
            plate.AddArrow(new Arrow(engd, de) {StartValue = 1917, Text = "Battle of Passchendaele(1917)"});
            plate.AddArrow(new Arrow(engd, de) {StartValue = 1917.6, Text = "Battle of Arras(1917)"});
            plate.AddArrow(new Arrow(engd, de) {StartValue = 1918, Text = "The Spring Offensive(1918)"});
            plate.AddArrow(new Arrow(usa, de) {StartValue = 1918.6, Text = "Hundred Days Offensive(1918)"});
            de.AddEntry(1919, 1933, "Weimar Republic", PrintLocation.Right);
            de.AddEntry(new LeaderEntry("Friedrich Ebert", new int?[,] {{1919, 1925}}) {StartValue = 1919.6});
            de.AddEntry(1923, "Beer Hall Putsch(1923)");
            de.AddEntry(new LeaderEntry("Paul von Hindenburg", new int?[,] {{1925, 1933}}));
            de.AddEntry(new LiteraryWorkEntry("Mein Kampf", "A.Hitler") {StartValue = 1925.6});
            de.AddEntry(new LiteraryWorkEntry("All Quiet..W.Front", "E.M.Remarque",1928));
            de.AddEntry(1933.6, 1945, "Nazi Germany", PrintLocation.Right);
            de.AddEntry(1933, "Enabling Act of 1933");
            de.AddEntry(new LeaderEntry("Aldof Hitler", new int?[,] {{1933, 1945}}) {StartValue = 1933.3});
            de.AddEntry(1935, "Nuremberg Laws(1935)");
            de.AddEntry(1938, "Sudetenland annexed(1938)");
            de.AddEntry(1941,1945,"The Holocaust[9]");
            plate.AddArrow(new Arrow(de, fr) {StartValue = 1940, Text = "conquest of France"});
            plate.AddArrow(new Arrow(de, engd) {StartValue = 1940.6, Text = "Battle of Britian"});
            plate.AddArrow(new Arrow(de, ussr) {StartValue = 1942.4, Text = "Op. Barbarossa" });

            plate.AddArrow(new Arrow(de, rus) {StartValue = 1914.6, Text = "Battle of Tannenberg(1914)"});
            rus.AddEntry(1918, "Tsar & family executed(1918)");
            rus.AddEntry(1918.6, "Red Terror");
            rus.AddEntry(1920, "Whites retreat east(1920)");
            rus.AddEntry(1922, "total devastation[2]");

            ussr.AddEntry(1917.6, "Bolshevik Revolution(1917)");
            ussr.AddEntry(1918, "Brest-Litovsk treaty(1918)");
            plate.AddArrow(new Arrow(ussr, rus) {StartValue = 1919, Text = "Keiv captured(1919)"});
            ussr.AddEntry(1919.6, "Polish-Soviet War(1919\\20)");
            ussr.AddEntry(new LeaderEntry("Lenin", new int?[,] {{null, 1924}}) {StartValue = 1922});
            ussr.AddEntry(new LeaderEntry("Stalin", new int?[,] {{1924, 1953}}));
            ussr.AddEntry(1928, 1932, "First Five Year Plan", PrintLocation.Right);
            ussr.AddEntry(1932, 1933, "Soviet famine", PrintLocation.Left);
            ussr.AddEntry(1933, 1937, "Second Five Year Plan", PrintLocation.Right);
            ussr.AddEntry(1936, 1938, "Great Purge[3]", PrintLocation.Left);

            plate.AddArrow(new Arrow(ott, rus) {StartValue = 1914.6, Text = "suprise attack on Black Sea"});
            plate.AddArrow(new Arrow(ott, rus) {StartValue = 1915, Text = "Battle of Gallipoli(1915)"});
            ott.AddEntry(1918, "Armistice of Mudros(1918)");
            ott.AddEntry(1920, "Occupation of Constantinople");
            ott.AddEntry(1920.6, "Treaty of Sevres[5]");

            jpn.AddEntry(1925, "General Election Law");
            plate.AddArrow(new Arrow(jpn, zho) {StartValue = 1931, Text = "invasion of Manchuria"});
            jpn.AddEntry(1932, "PM assassinated[6]");
            jpn.AddEntry(1937,"MarcoPolo Bridge Incident");
            jpn.AddEntry(1914.6, 1926, "Taisho era", PrintLocation.Left);
            jpn.AddEntry(1926.6, 1945, "Showa era", PrintLocation.Left);
            plate.AddArrow(new Arrow(jpn, zho) {StartValue = 1937.6, Text = "Battle of Shanghai"});
            plate.AddArrow(new Arrow(jpn, usa) {StartValue = 1941.9, Text = "Pearl Harbor"});

            zho.AddEntry(1916,1928,"Warlord Era", PrintLocation.Right);
            zho.AddEntry(new LeaderEntry("Sun Yat-sen", new int?[,] { { 1916, 1925} }));
            zho.AddEntry(1921,"CPP formed(1921)");
            zho.AddEntry(new LeaderEntry("Chiang Kai-shek", new int?[,] { {1925,1948} }));
            zho.AddEntry(1927,"'White Terror'[7]");
            zho.AddEntry(1934,1935,"The Long March[8]");
            zho.AddEntry(1937, "Nanking Massacre(1937)");
            zho.AddEntry(1930.6,1945,"Japanese Occupation.", PrintLocation.Right);
            zho.AddEntry(1941,"Unit 731 atrocities[10]");
            plate.AddArrow(new Arrow(zho, jpn) {StartValue = 1940, Text = "Hundred Reg. Offensive"});


            plate.Notes.Add(" [1] First lethal use of chemical weapons.  [2] war and famine, millions dead, industry ruined, skilled and edu workers fled");
            plate.Notes.Add(" [3] 600K-1.2M deaths estimated.            [4] East & South European: limited, African: severely limited, Asian and Arab: banned, Latin American: no restriction");
            plate.Notes.Add(" [5] Ottoman Empire partitioned amoung national groups within it.");
            plate.Notes.Add(" [6] (Army cadet assassins) enables military to act with little restraint.");
            plate.Notes.Add(" [7] Chiang Kai-skek turns on CPP and purges as many as 12K in Shanghai.     [8] CPP in retreat, Mao Zedong takes command at this time.");
            plate.Notes.Add(" [9] 6M Jews murdered (2/3 total Jewish pop. of Euro), another 5M non-Jews likewise murdered");
            plate.Notes.Add(" [10] human experimentation est. 250K murders");
            plate.Notes.Add(" (#) First technical expo on creating the atomic bomb.");
            return plate;
        }
    }
}
