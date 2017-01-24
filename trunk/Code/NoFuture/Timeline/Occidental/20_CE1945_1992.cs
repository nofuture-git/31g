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
            var ussr = new Block { Ruler = rule, Title = "Soviet Union", EndValue = 1991};
            var zho = new Block { Ruler = rule, Title = "China" };
            var jpn = new Block { Ruler = rule, Title = "Japan" };
            var israel = new Block { Ruler = rule, Title = "Israel", StartValue = 1948};

            plate.AddBlock(jpn);
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(israel);
            plate.AddBlock(ussr);
            plate.AddBlock(zho);

            zho.AddEntry(1946,1949,"Civil War", PrintLocation.Right);
            zho.AddEntry(1949, "KMT flees to Taiwan");
            zho.AddEntry(new LeaderEntry("Mao Zedong",new int?[,] { {1946, 1976} }));
            zho.AddEntry(new LeaderEntry("Hua Guofeng", new int?[,] { {1976,1978} }));
            zho.AddEntry(new LeaderEntry("Deng Xiaoping", new int?[,] { {1978, 1989} }));
            zho.AddEntry(new LeaderEntry("Chen Yun", new int?[,] { {1987, 1992} }));
            zho.AddEntry(1958, 1962, "'Great Leap Foward'[1]");
            zho.AddEntry(1964, "test atomic bomb(1964)");
            zho.AddEntry(1966, 1976, "'Cultural Revolution'[2]");
            zho.AddEntry(1972,"Nixon's visit(1972)");
            zho.AddEntry(1979,"economic reform");

            jpn.AddEntry(1946, "Tokyo Trials(warcrimes)");
            jpn.AddEntry(new LiteraryWorkEntry("Constitution.Japan","",1947));
            jpn.AddEntry(1947.8,"Fundamental Law of Edu(1947)");
            jpn.AddEntry(new LeaderEntry("Shigeru Yoshida", new int?[,] { { 1948, 1954 } }) {StartValue = 1949}); 
            jpn.AddEntry(new LeaderEntry("Ichiro Hatoyama", new int?[,] { { 1954, 1956 } }));
            jpn.AddEntry(new LeaderEntry("Tanzan Ishibashi", new int?[,] { { 1956, 1957} }));
            jpn.AddEntry(new LeaderEntry("Nobusuke Kishi", new int?[,] { { 1957, 1960 } }));
            jpn.AddEntry(new LeaderEntry("Hayato Ikeda",new int?[,] { {1960, 1964} }));
            jpn.AddEntry(new ScienceAdvEntry("high-speed rail","",1964) {StartValue = 1965.6});
            jpn.AddEntry(new LeaderEntry("Eisaku Sato", new int?[,] { { 1964, 1972 } }));
            jpn.AddEntry(new ScienceAdvEntry("pocket calc","",1970));
            jpn.AddEntry(new LeaderEntry("Kakuei Tanaka", new int?[,] { {1972,1974} }));
            jpn.AddEntry(new LeaderEntry("Takeo Miki", new int?[,] { { 1974, 1976 } }));
            jpn.AddEntry(new LeaderEntry("Takeo Fukuda", new int?[,] { { 1976, 1978 } }));
            jpn.AddEntry(new LeaderEntry("Masayoshi Ohira", new int?[,] { { 1978, 1980 } }));
            jpn.AddEntry(new LeaderEntry("Zenko Suzuki", new int?[,] { { 1980, 1982 } }));
            jpn.AddEntry(new LeaderEntry("Yasuhiro Nakasone", new int?[,] { { 1982, 1987 } }));
            jpn.AddEntry(new LeaderEntry("Noboru Takeshita", new int?[,] { { 1987, 1989 } }));
            jpn.AddEntry(new LeaderEntry("Toshiki Kaifu", new int?[,] { {1989,1991} }));
            jpn.AddEntry(1945.6, 1952, "Allied occupation");
            jpn.AddEntry(1965,1980,"rapid econ. expansion", PrintLocation.Right);
            jpn.AddEntry(1980.6,"becomes 2nd largest econ.");

            usa.AddEntry(1945.5, "United Nations formed");
            usa.AddEntry(new LeaderEntry("Truman",new int?[,] { {1945, 1953} }) {StartValue = 1946});
            usa.AddEntry(1947,"Truman Doctrine[4]");
            usa.AddEntry(new ScienceAdvEntry("transistor","",1947) {StartValue = 1947.6});
            usa.AddEntry(1948,"Marshall Plan");
            usa.AddEntry(1948.6,"NATO formed");
            usa.AddEntry(1952.6, "hydrogen bomb tested");
            usa.AddEntry(new LeaderEntry("Eisenhower", new int?[,] { {1953, 1961} }));
            usa.AddEntry(1950,1953,"Korean War", PrintLocation.Right);
            usa.AddEntry(1953.5, "support Iran coup");
            usa.AddEntry(1954,"Domino Theory");
            usa.AddEntry(1956.6, "military assist Vietnam");
            usa.AddEntry(new LeaderEntry("J.F.Kennedy",new int?[,] { {1961,1963} }));
            usa.AddEntry(new ScienceAdvEntry("integrated circuit","",1958));
            usa.AddEntry(1959,"Communist Cuba");
            usa.AddEntry(1962,"Cuban Missile Crisis[5]");
            usa.AddEntry(new LeaderEntry("L.B.Johnson", new int?[,] { { 1963, 1969 } }));
            usa.AddEntry(1964,1972,"Vietnam War",PrintLocation.Right);
            usa.AddEntry(new ScienceAdvEntry("moon landing","",1969) {StartValue = 1969});
            usa.AddEntry(1969.9,1973,"draft lottery", PrintLocation.Left);
            usa.AddEntry(new LeaderEntry("Nixon", new int?[,] { { 1969, 1974 } }) {StartValue = 1969.6});
            usa.AddEntry(1972,"Watergate incident");
            usa.AddEntry(1973,"oil crisis");
            usa.AddEntry(1973.6, "support Chilean coup");
            usa.AddEntry(new LeaderEntry("G.Ford", new int?[,] { { 1974, 1977 } }));
            usa.AddEntry(new LeaderEntry("J.Cater", new int?[,] { { 1977, 1981 } }));
            usa.AddEntry(1979, "Iran hostage crisis");
            usa.AddEntry(1979.6,"oil crisis");
            usa.AddEntry(new LeaderEntry("Reagan", new int?[,] { { 1981, 1989 } }));
            usa.AddEntry(1983, "invade Grenada");
            usa.AddEntry(new LeaderEntry("Bush", new int?[,] { {1989,1992} }));
            usa.AddEntry(1990,"first Gulf War");

            engd.AddEntry(1947,"India independent");
            engd.AddEntry(1945.5,1954,"rationing",PrintLocation.Right);
            engd.AddEntry(1952, "test atomic bomb(1952)");
            engd.AddEntry(1948,"National Health Serv.");
            engd.AddEntry(1949,"withdraw from Palestine");
            engd.AddEntry(new ScienceAdvEntry("DNA","",1953));
            engd.AddEntry(1956,"Suez Crisis[8]");
            engd.AddEntry(1955.4,1965, "econ. recovery", PrintLocation.Right);
            engd.AddEntry(1978.6,"'Winter of Discontent'[9]");
            engd.AddEntry(new LeaderEntry("Clement Attlee", new int?[,] { {1945,1951} }));
            engd.AddEntry(new LeaderEntry("Churchill", new int?[,] { {1940,1945}, { 1951, 1955 } }));
            engd.AddEntry(new LeaderEntry("Anthony Eden", new int?[,] { { 1955, 1957 } }));
            engd.AddEntry(new LeaderEntry("Harold Macmillan", new int?[,] { { 1957, 1963 } }));
            engd.AddEntry(new LeaderEntry("A.Douglas-Home", new int?[,] { { 1963, 1964 } }));
            engd.AddEntry(new LeaderEntry("Harold Wilson", new int?[,] { { 1964, 1970 }, { 1974, 1976 } }));
            engd.AddEntry(new LeaderEntry("Edward Heath", new int?[,] { { 1970, 1974 } }));
            engd.AddEntry(new LeaderEntry("James Callaghan", new int?[,] { { 1976, 1979 } }));
            engd.AddEntry(new LeaderEntry("M.Thatcher", new int?[,] { { 1977, 1990 } }));
            engd.AddEntry(1982,"Falkland War");
            engd.AddEntry(1984,"miners' strike");
            engd.AddEntry(new LeaderEntry("John Major", new int?[,] { { 1990, 1997 } }));
            
            israel.AddEntry(1948.6,"Arab-Israeli War"); 
            israel.AddEntry(1949,"armistice w\\ arabs");
            israel.AddEntry(new LeaderEntry("D.Ben-Gurion",new int?[,] { {1949, 1954}, {1955,1963} }) {StartValue = 1949.6});
            israel.AddEntry(1950,"Law of Return");
            israel.AddEntry(1952,"rel. W Germany");
            israel.AddEntry(new LeaderEntry("M.Sharett", new int?[,] { { 1954, 1955 } }));
            israel.AddEntry(1961, "Eichmann trail");
            israel.AddEntry(new LeaderEntry("Levi Eshkol", new int?[,] { { 1963, 1969 } }));
            israel.AddEntry(1966,"US military support");
            israel.AddEntry(1967,"six-day war[10]");
            israel.AddEntry(new LeaderEntry("Golda Meir", new int?[,] { { 1969, 1974 } }));
            israel.AddEntry(1973,"Yom Kippur War[11]");
            israel.AddEntry(new LeaderEntry("Y.Rabin", new int?[,] { { 1974, 1977 }, {1992,1995} }));
            israel.AddEntry(1975, "efforts settle West Bank ");
            israel.AddEntry(new LeaderEntry("M.Begin", new int?[,] { { 1977, 1983 } }));
            israel.AddEntry(1978,"Egypt-Israel peace");
            israel.AddEntry(1982, "Lebanon War[12]");
            israel.AddEntry(new LeaderEntry("Y.Shamir", new int?[,] { { 1983, 1984 }, {1986,1992} }));
            israel.AddEntry(1987,1997,"First Intifada", PrintLocation.Right);
            israel.AddEntry(1988, "launch spy satallite");
            israel.AddEntry(new LeaderEntry("S.Peres", new int?[,] { {1984,1986}, { 1995, 1996 } }));
            israel.AddEntry(1990, "Soviet Jews free migration");


            de.AddEntry(1955.6, "total econ. recovery[W]");
            de.AddEntry(new LeaderEntry("Konrad Adenauer", new int?[,] { {1949, 1963} }) {StartValue = 1950});
            de.AddEntry(1951,"industrial limits lifted");
            de.AddEntry(1955, "sovereign W.Germany ");
            de.AddEntry(new LeaderEntry("Ludwig Erhard", new int?[,] { { 1963, 1966 } }));
            de.AddEntry(new LeaderEntry("K.G.Kiesinger", new int?[,] { { 1966, 1969 } }));
            de.AddEntry(new LeaderEntry("Willy Brandt", new int?[,] { { 1969, 1974 } }));
            de.AddEntry(new LeaderEntry("Helmut Schmidt", new int?[,] { { 1974, 1982 } }));
            de.AddEntry(new LeaderEntry("Helmut Kohl", new int?[,] { { 1982, 1998 } }));
            de.AddEntry(1961, "ctor Berlin Wall(1961)");
            de.AddEntry(new LiteraryWorkEntry("Holcaust[6]","TV-series",1979));
            de.AddEntry(1989, "Berlin Wall comes down");
            de.AddEntry(1990, "German re-unification");
            de.AddEntry(1948, 1949, "Berlin blockade", PrintLocation.Right);
            de.AddEntry(1945.6, 1949, "Allied occupation");

            fr.AddEntry(1956.6, "French leave Vietnam");
            fr.AddEntry(new LeaderEntry("DeGaulle", new int?[,] { { 1946, 1947 }, { 1959, 1969 } }) {StartValue = 1945.5});
            fr.AddEntry(1951,"ECSC formed[7]");
            fr.AddEntry(new LeaderEntry("V.Auriol",new int?[,] { {1947, 1954} }));
            fr.AddEntry(new LeaderEntry("Rene Coty", new int?[,] { { 1954, 1959 } }));
            fr.AddEntry(1957, "EEC superseds ECSC");
            fr.AddEntry(new LeaderEntry("G.Pompidou", new int?[,] { {1969, 1974} }));
            fr.AddEntry(new LeaderEntry("V.G.d'Estaing", new int?[,] { { 1974, 1981 } }));
            fr.AddEntry(new LeaderEntry("F.Mitterrand", new int?[,] { { 1981, 1995 } }));
            fr.AddEntry(1958,"Algerian crisis");
            fr.AddEntry(1946, 1956, "Indochina War(Vietnam)",PrintLocation.Left);
            fr.AddEntry(1949, 1958, "Fourth Republic", PrintLocation.Right);
            fr.AddEntry(1958.6,1992,"Fifth Republic", PrintLocation.Right);

            ussr.AddEntry(1949, "test atomic bomb(1949)");
            ussr.AddEntry(1955.4,"Warsaw Pact formed");
            ussr.AddEntry(1956, "Hungarian Revolt crushed(1956)");
            ussr.AddEntry(new ScienceAdvEntry("satallite(Sputnik)","",1957));
            ussr.AddEntry(1968,"Czech revolt crushed(1968)");
            ussr.AddEntry(1979,1987,"Soviet-Afghan war", PrintLocation.Right);
            ussr.AddEntry(1990,"Soviet Union abolished");
            ussr.AddEntry(new LeaderEntry("Stalin",new int?[,] { {1924, 1953} }) {StartValue = 1946});
            ussr.AddEntry(new LeaderEntry("G.Malenkov", new int?[,] { { 1953,1955 } }));
            ussr.AddEntry(new LeaderEntry("Khrushchev", new int?[,] { { 1955, 1964} }));
            ussr.AddEntry(new LeaderEntry("L.Brezhnev", new int?[,] { { 1964, 1982 } }));
            ussr.AddEntry(new LeaderEntry("Y.Andropov", new int?[,] { { 1982, 1984 } }));
            ussr.AddEntry(new LeaderEntry("K.Chernenko", new int?[,] { { 1984, 1985 } }));
            ussr.AddEntry(new LeaderEntry("M.Gorbachev", new int?[,] { { 1985, 1991 } }));

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
            plate.Notes.Add(" [4] Policy to contain spread of Soviet influence.  [5] Closest to point WWIII.");
            plate.Notes.Add(" [6] When term was first coined, brought Nazi attrocities to W.German public");
            plate.Notes.Add(" [7] ECSC: European Coal and Steel Community, EEC: European Economic Community");
            plate.Notes.Add(" [8] loss of superpower status, leads to further decolonization");
            plate.Notes.Add(" [9] Political crisis with wide-spread strikes.");
            plate.Notes.Add(" [10] territory gains and access to Jerusalem's old city.  [11] US support led Saudi gov't to initate oil embargo (1973 oil crisis).");
            plate.Notes.Add(" [11] S.Lebanon controlled by islamic radicals - invasion success all the way to Beirut.");

            return plate;
        }
    }
}
