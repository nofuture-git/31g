namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE780to500()
        {
            var rule = new Rule {StartValue = 780, EndValue = 500, RuleLineSpacing = 7};
            var egypt = new Block {StartValue = 780, EndValue = 528, Ruler = rule, Title = "Egypt"};
            egypt.AddEntry(new LeaderEntry("Shabako", new int?[,] {{710, 696}}) {StartValue = 705});
            egypt.AddEntry(new LeaderEntry("Shebteko", new int?[,] {{696, 685}}));
            egypt.AddEntry(new LeaderEntry("Tirhakah coregent", new int?[,] {{690, 689}}));
            egypt.AddEntry(new LeaderEntry("Tirhakah", new int?[,] {{685, 664}}));
            egypt.AddEntry(new LeaderEntry("Psammetichus I", new int?[,] {{664, 610}}) {StartValue = 655});
            egypt.AddEntry(new LeaderEntry("Neco II", new int?[,] {{610, 594}}));
            egypt.AddEntry(new ScienceAdvEntry("lighthouse","") {StartValue = 600});
            egypt.AddEntry(new LeaderEntry("Psammetichus II", new int?[,] {{594, 589}}));
            egypt.AddEntry(new LeaderEntry("Apries (Hophra)", new int?[,] {{589, 570}}));
            egypt.AddEntry(759, 716, "Libyian Dyn.(s) 759-715 ", PrintLocation.Right);
            egypt.AddEntry(710, 663, "Nubian Dyn. 715-663 (Kush)", PrintLocation.Right);
            egypt.AddEntry(660, 525, "XXVI Dyn. 664-525", PrintLocation.Right);
            egypt.AddEntry(new LeaderEntry("Armasis", new int?[,] {{570, 526}}));
            egypt.AddEntry(new LeaderEntry("Psammetichus III", new int?[,] {{526, 525}}) {StartValue = 530});

            var judah = new Block {Ruler = rule, Title = "Judah", StartValue = 780, EndValue = 500, Width = 34};
            judah.AddEntry(new LeaderEntry("Jotham coregent", new int?[,] {{null, 750}}) {StartValue = 750});
            judah.AddEntry(new LeaderEntry("Jotham", new int?[,] {{742, 735}}));
            judah.AddEntry(new LeaderEntry("Ahaz", new int?[,] {{735, 715}}));
            judah.AddEntry(new LeaderEntry("Hezekiah", new int?[,] {{715, 687}}));
            judah.AddEntry(new LeaderEntry("Manasseh", new int?[,] {{687, 642}}));
            judah.AddEntry(new LeaderEntry("Amon", new int?[,] {{642, 640}}));
            judah.AddEntry(new LeaderEntry("Josiah", new int?[,] {{640, 609}})
            {
                StartValue = 638,
            });
            judah.AddEntry(new LeaderEntry("Jehoahaz", new int?[,] {{null, 609}}) {StartValue = 609});
            judah.AddEntry(new LeaderEntry("Jehoiakim", new int?[,] {{609, 598}})
            {
                StartValue = 604,
            });
            judah.AddEntry(new LeaderEntry("Jehoiachin", new int?[,] {{598, 597}}));
            judah.AddEntry(new LeaderEntry("Zedekiah", new int?[,] {{597, 587}})
            {
                StartValue = 595,
            });
            judah.AddEntry(590, "1st dep. 597", PrintLocation.Left);
            judah.AddEntry(587, "2nd dep.", PrintLocation.Left);
            judah.AddEntry(582, "3rd dep. 582", PrintLocation.Left);
            judah.AddEntry(new ProphetEntry("Zephaniah") {StartValue = 625, Location = PrintLocation.Right});
            judah.AddEntry(new ProphetEntry("Nahum") { StartValue = 619, Location = PrintLocation.Right});
            judah.AddEntry(new ProphetEntry("Habakkuk") {StartValue = 602, Location = PrintLocation.Right});
            judah.AddEntry(new ProphetEntry("Micah")
            {
                StartValue = 735,
                EndValue = 701,
                Location = PrintLocation.Right
            });
            judah.AddEntry(new ProphetEntry("Isaiah") { StartValue = 730, EndValue = 688});
            judah.AddEntry(new ProphetEntry("Jeremiah") {StartValue = 630, EndValue = 587});
            judah.AddEntry(new ProphetEntry("Ezekiel")
            {
                StartValue = 595,
                EndValue = 575,
                Location = PrintLocation.Right
            });
            judah.AddEntry(568, "in exile");
            judah.AddEntry(new ProphetEntry("II Isaiah")
            {
                StartValue = 545,
                EndValue = 525,
                Location = PrintLocation.Right
            });
            judah.AddEntry(538, "Cyrus' edict(538)");
            judah.AddEntry(528, "Zerubbabel (gov. Judah)");
            judah.AddEntry(520, "The Temple rebuilt 520-515");
            judah.AddEntry(new ProphetEntry("Haggai, Zechariah")
            {
                StartValue = 515,
                Location = PrintLocation.Right
            });

            var israel = new Block {Ruler = rule, Title = "Israel", StartValue = 780, EndValue = 722};
            israel.AddEntry(new LeaderEntry("Zechariah", new int?[,] {{746, 745}}));
            israel.AddEntry(new LeaderEntry("Shallum", new int?[,] {{null, 745}})
            {
                StartValue = 745,
            });
            israel.AddEntry(new LeaderEntry("Menahem", new int?[,] {{745, 737}})
            {
                Location = PrintLocation.Right
            });
            israel.AddEntry(new LeaderEntry("Pekahiah", new int?[,] {{737, 736}}));
            israel.AddEntry(new LeaderEntry("Pekah", new int?[,] {{736, 732}})
            {
                Location = PrintLocation.Right
            });
            israel.AddEntry(new LeaderEntry("Hoshea", new int?[,] {{732, 724}}));
            israel.AddEntry(new ProphetEntry("Amos") { StartValue = 756, Location = PrintLocation.Right});
            israel.AddEntry(new ProphetEntry("Hosea")
            {
                StartValue = 750,
                EndValue = 736,
                Location = PrintLocation.Right
            });

            var assyria = new Block {Ruler = rule, Title = "Assyria", StartValue = 780, EndValue = 604, Width = 29};
            assyria.AddEntry(777, 750, "Assyrian weakness");
            assyria.AddEntry(new LeaderEntry("Tiglath-pileser III", new int?[,] {{745, 727}}) {StartValue = 746});
            assyria.AddEntry(new LeaderEntry("Shalmaneser V", new int?[,] {{726, 722}}));
            assyria.AddEntry(new LeaderEntry("Sargon II", new int?[,] {{721, 705}}) {StartValue = 715});
            assyria.AddEntry(new LeaderEntry("Sennacherib", new int?[,] {{704, 681}}));
            assyria.AddEntry(new LeaderEntry("Esarhaddon", new int?[,] {{680, 669}}) );
            assyria.AddEntry(new LeaderEntry("Asshurbanapal", new int?[,] {{668, 627}}));
            assyria.AddEntry(new LeaderEntry("Sin-shar-ishkun", new int?[,] {{629, 612}}));
            assyria.AddEntry(new LeaderEntry("Asshur-uballit II", new int?[,] {{612, 609}}) {StartValue = 609});

            var babylon = new Block {Ruler = rule, Title = "Babylon", StartValue = 666, EndValue = 542};
            babylon.AddEntry(new LeaderEntry("Nabopolassar", new int?[,] {{626, 605}}));
            babylon.AddEntry(new LeaderEntry("Nebuchadnezzar", new int?[,] {{605, 562}}) {StartValue = 609});
            babylon.AddEntry(new LeaderEntry("Amel-marduk", new int?[,] {{562, 560}}));
            babylon.AddEntry(new LeaderEntry("Neriglissar", new int?[,] {{560, 556}}));
            babylon.AddEntry(new LeaderEntry("Nabonidus", new int?[,] {{556, 539}}));

            var persia = new Block {Ruler = rule, Title = "Persia/Media", StartValue = 675, EndValue = 500};
            persia.AddEntry(670, 638, "Medes");
            persia.AddEntry(new LeaderEntry("Cyaxares", new int?[,] {{625, 585}}));
            persia.AddEntry(new LeaderEntry("Astyages", new int?[,] {{585, 550}}));
            persia.AddEntry(new LeaderEntry("Cyrus (the Great)", new int?[,] {{550, 530}}));
            persia.AddEntry(new LeaderEntry("Cambyses", new int?[,] {{530, 522}}) );
            persia.AddEntry(new LeaderEntry("Darius I", new int?[,] {{522, 486}}) {StartValue = 520});

            var greece = new Block {Ruler = rule, Title = "Archaic Greece", Width = 24};
            greece.AddEntry(776, "First Olympic Games");
            greece.AddEntry(new PhilosopherEntry("Homer",null,null) {Location = PrintLocation.Right, StartValue = 750});
            greece.AddEntry(710, "Spread of Greek colonies");
            greece.AddEntry(new PhilosopherEntry("Xenophanes", null,null) {Location = PrintLocation.Right, StartValue = 550});
            greece.AddEntry(new PhilosopherEntry("Pythagoras",null,null) {Location = PrintLocation.Right, StartValue = 540});
            greece.AddEntry(743, 724, "First Messenian War");
            greece.AddEntry(685, 668, "Second Messenian War");
            greece.AddEntry(new ScienceAdvEntry("crane","") {StartValue = 515});

            var rome = new Block {Ruler = rule, Title = "Kingdom of Rome", StartValue = 750};
            rome.AddEntry(753,
                "Rome founded(753)");
            rome.AddEntry(new LeaderEntry("Romulus", new int?[,] {{753, 716}}) {StartValue = 748});
            rome.AddEntry(new LeaderEntry("Numa Pompilius", new int?[,] {{715, 672}}));
            rome.AddEntry(new LeaderEntry("Tullus Hostilius", new int?[,] {{672, 640}}));
            rome.AddEntry(new LeaderEntry("Ancus Marcius", new int?[,] {{640, 616}}));
            rome.AddEntry(new LeaderEntry("Lucius T. Priscus", new int?[,] {{616, 578}}));
            rome.AddEntry(new LeaderEntry("Servius Tullius", new int?[,] {{578, 534}}));
            rome.AddEntry(new LeaderEntry("Lucius T Superbus", new int?[,] {{534, 510}}));
            rome.AddEntry(509,
                new string(Config.GraphChars.Rail, 3) + "Republic Founded" + new string(Config.GraphChars.Rail, 3));

            var plate = new Plate {Ruler = rule, Name = "The Prophesy Fulfilled"};
            plate.AddArrow(new Arrow(assyria, egypt) {StartValue = 663, Text = "Thebes sacked(663)"});
            plate.AddArrow(new Arrow(babylon, egypt) {StartValue = 568, Text = "Nebuchadnezzer invades"});
            plate.AddArrow(new Arrow(persia, babylon) {StartValue = 539, Text = "Fall of Babylon(539)"});
            plate.AddArrow(new Arrow(persia, egypt) {StartValue = 525, Text = "Fall of Egypt(525)"});
            plate.AddArrow(new Arrow(assyria, israel) {StartValue = 720, Text = "Fall of Samaria(722)"});
            plate.AddArrow(new Arrow(babylon, judah) {StartValue = 587, Text = "Fall of Jerusalem(587)"});
            plate.AddArrow(new Arrow(babylon, assyria) {StartValue = 612, Text = "Fall of Nineveh(612)"});
            plate.AddBlock(egypt);
            plate.AddBlock(judah);
            plate.AddBlock(israel);
            plate.AddBlock(assyria);
            plate.AddBlock(babylon);
            plate.AddBlock(persia);
            plate.AddBlock(greece);
            plate.AddBlock(rome);
            plate.FileName = "05_BCE780to500";

            return plate;

        }
    }
}
