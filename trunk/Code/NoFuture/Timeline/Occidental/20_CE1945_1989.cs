using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1945to1992()
        {
            var rule = new Rule { StartValue = 1945, EndValue = 1992, RuleLineSpacing = 7 };
            var plate = new Plate { Ruler = rule, Name = "The Cold War" };


            var usa = new Block { Ruler = rule, Title = "United States"};
            var engd = new Block { Ruler = rule, Title = "United Kingdom" };
            var fr = new Block { Ruler = rule, Title = "France"};
            var de = new Block { Ruler = rule, Title = "Germany" };
            var ussr = new Block { Ruler = rule, Title = "Soviet Union"};
            var zho = new Block { Ruler = rule, Title = "China" };
            var jpn = new Block { Ruler = rule, Title = "Japan" };

            plate.AddBlock(zho);
            plate.AddBlock(jpn);
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(ussr);

            zho.AddEntry(1946,1949,"Civil War", PrintLocation.Right);
            zho.AddEntry(1949, "KMT flees to Taiwan");
            zho.AddEntry(new LeaderEntry("Mao Zedong",new int?[,] { {1946, 1976} }));
            zho.AddEntry(new LeaderEntry("Hua Guofeng", new int?[,] { {1976,1978} }));
            zho.AddEntry(new LeaderEntry("Deng Xiaoping", new int?[,] { {1978, 1989} }));
            zho.AddEntry(1958, 1962, "'Great Leap Foward'[1]");
            zho.AddEntry(1964, "test atomic bomb(1964)");
            zho.AddEntry(1966, 1976, "'Cultural Revolution'[2]");
            zho.AddEntry(1979,"economic reform");

            jpn.AddEntry(1946, "Tokyo Trials(warcrimes)");
            jpn.AddEntry(new LiteraryWorkEntry("Constitution.Japan","",1947));
            jpn.AddEntry(1947.8,"Fundamental Law of Edu(1947)");
            jpn.AddEntry(1945.6, 1952, "Allied occupation");
            jpn.AddEntry(new LeaderEntry("Shigeru Yoshida", new int?[,] { { 1948, 1954 } })); 
            jpn.AddEntry(new LeaderEntry("Ichiro Hatoyama", new int?[,] { { 1954, 1956 } }));
            jpn.AddEntry(new LeaderEntry("Tanzan Ishibashi", new int?[,] { { 1956, 1957} }));
            jpn.AddEntry(new LeaderEntry("Nobusuke Kishi", new int?[,] { { 1957, 1960 } }));
            jpn.AddEntry(new LeaderEntry("Hayato Ikeda",new int?[,] { {1960, 1964} }));
            jpn.AddEntry(new ScienceAdvEntry("high-speed rail","",1964) {StartValue = 1965.6});
            jpn.AddEntry(new LeaderEntry("Eisaku Sato", new int?[,] { { 1964, 1972 } }));
            jpn.AddEntry(new LeaderEntry("Kakuei Tanaka", new int?[,] { {1972,1974} }));
            jpn.AddEntry(new LeaderEntry("Takeo Miki", new int?[,] { { 1974, 1976 } }));
            jpn.AddEntry(new LeaderEntry("Takeo Fukuda", new int?[,] { { 1976, 1978 } }));
            jpn.AddEntry(new LeaderEntry("Masayoshi Ohira", new int?[,] { { 1978, 1980 } }));
            jpn.AddEntry(new LeaderEntry("Zenko Suzuki", new int?[,] { { 1980, 1982 } }));
            jpn.AddEntry(new LeaderEntry("Yasuhiro Nakasone", new int?[,] { { 1982, 1987 } }));
            jpn.AddEntry(new LeaderEntry("Noboru Takeshita", new int?[,] { { 1987, 1989 } }));
            jpn.AddEntry(1965,1980,"rapid econ. expansion", PrintLocation.Right);
            jpn.AddEntry(1980.6,"becomes 2nd largest econ.");

            usa.AddEntry(new LeaderEntry("Truman",new int?[,] { {1945, 1953} }) {StartValue = 1946});
            usa.AddEntry(1947,"Truman Doctrine[4]");
            usa.AddEntry(1948,"NATO formed");
            usa.AddEntry(new LeaderEntry("Eisenhower", new int?[,] { {1953, 1961} }));
            usa.AddEntry(1950,1953,"Korean War", PrintLocation.Right);
            usa.AddEntry(1953.5, "support Iran coup");
            usa.AddEntry(new LeaderEntry("J.F.Kennedy",new int?[,] { {1961,1963} }));
            usa.AddEntry(1959,"Communist Cuba");
            usa.AddEntry(new LeaderEntry("L.B.Johnson", new int?[,] { { 1963, 1969 } }));
            usa.AddEntry(new LeaderEntry("Nixon", new int?[,] { { 1969, 1974 } }));
            usa.AddEntry(new LeaderEntry("G.Ford", new int?[,] { { 1974, 1977 } }));
            usa.AddEntry(new LeaderEntry("J.Cater", new int?[,] { { 1977, 1981 } }));
            usa.AddEntry(new LeaderEntry("Reagan", new int?[,] { { 1981, 1989 } }));

            ussr.AddEntry(1949, "test atomic bomb(1949)");
            ussr.AddEntry(1955,"Warsaw Pact formed");
            ussr.AddEntry(1956, "Hungarian Revolt crushed(1956)");
            ussr.AddEntry(new ScienceAdvEntry("satallite(Sputnik)","",1957));
            ussr.AddEntry(1979,"Afghanistan invasion(1979)");

            de.AddEntry(1948, 1949, "Berlin blockade");
            de.AddEntry(1961, "ctor Berlin Wall(1961)");
            de.AddEntry(1989,"Berlin Wall comes down");

            plate.AddArrow(new Arrow(usa, jpn)
            {
                StartValue = 1952,
                Text = "San Franciso Treaty (1952)",
                FromRightToLeftArrowHead = "+```````",
                ArrowShaft = '`',
                ArrowTail = "```````+"
            });
            plate.AddArrow(new Arrow(zho, ussr) {StartValue = 1969, Text = "border conflict"});
            plate.Notes.Add(" [1] An economic disaster resulting in millions of deaths.  [2] brought serious disaster and turmoil to the Communist Party and the Chinese people.");
            plate.Notes.Add(" [3] China & USSR nearly come to nuclear war.");
            plate.Notes.Add(" [4] Policy to contain spread of Soviet influence.");

            return plate;
        }
    }
}
