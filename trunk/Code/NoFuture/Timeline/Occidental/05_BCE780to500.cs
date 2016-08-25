namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE780to500()
        {
            var rule = new Rule() { StartValue = 780, EndValue = 500, RuleLineSpacing = 7 };
            var egypt = new Block() {StartValue = 780, EndValue = 528, Ruler = rule, Title = "Egypt" };
            egypt.AddEntry(705,"Shabako 710/9-696/5");
            egypt.AddEntry(696,"Shebteko 696/5-685/4");
            egypt.AddEntry(690,"(Tirhakah coregent 690/89)");
            egypt.AddEntry(685,"Tirhakah (690) 685/4-664");
            egypt.AddEntry(655,"Psammetichus I 664-610");
            egypt.AddEntry(610,"Neco II 610-594");
            egypt.AddEntry(594,"Psammetichus II 594-589");
            egypt.AddEntry(589,"Apries (Hophra) 589-570");
            egypt.AddEntry(759, 716, "Libyian Dyn.(s) 759-715 ", PrintLocation.Right);
            egypt.AddEntry(710,663,"Nubian Dyn. 715-663 (Kush)",PrintLocation.Right);
            egypt.AddEntry(660,525, "XXVI Dyn. 664-525", PrintLocation.Right);
            egypt.AddEntry(570, "Armasis 570-526");
            egypt.AddEntry(530, "Psammetichus III 526/5");

            var judah = new Block() { Ruler = rule, Title = "Judah", StartValue = 780, EndValue = 500, Width = 34};
            judah.AddEntry(750,"(Jotham coregent 750)");
            judah.AddEntry(new Entry(){StartValue = 742, Text = "Jotham 742-735", Location = PrintLocation.Left});
            judah.AddEntry(new Entry() { StartValue = 735, Text = "Ahaz 735-715", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 715, Text = "Hezekiah 715-687/6", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 687, Text = "Manasseh 687/6-642", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 642, Text = "Amon 642-640", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 638, Text = "Josiah 640-609", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 609, Text = "Jehoahaz 609", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 604, Text = "Jehoiakim 609-598", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 598, Text = "Jehoiachin 598/7", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 595, Text = "Zedekiah 597-587", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 590, Text = "1st dep. 597", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 587, Text = "2nd dep.", Location = PrintLocation.Left });
            judah.AddEntry(new Entry() { StartValue = 582, Text = "3rd dep. 582", Location = PrintLocation.Left });
            judah.AddEntry(new Entry(){StartValue = 625, Text = "(Zephaniah)", Location = PrintLocation.Right});
            judah.AddEntry(new Entry() { StartValue = 619, Text = "(Nahum)", Location = PrintLocation.Right });
            judah.AddEntry(new Entry() { StartValue = 602, Text = "(Habakkuk)", Location = PrintLocation.Right });
            judah.AddEntry(735,701,"(Micah)",PrintLocation.Right);
            judah.AddEntry(730,688,"(Isaiah)");
            judah.AddEntry(630,587,"(Jeremiah)");
            judah.AddEntry(595, 575, "(Ezekiel)", PrintLocation.Right);
            judah.AddEntry(568,"in exile");
            judah.AddEntry(545,525,"(II Isaiah)", PrintLocation.Right);
            judah.AddEntry(538,"Cyrus' edict(538)");
            judah.AddEntry(528,"Zerubbabel (gov. Judah)");
            judah.AddEntry(520,"The Temple rebuilt 520-515");
            judah.AddEntry(new Entry(){StartValue = 515,Text = "(Haggai, Zechariah)", Location = PrintLocation.Right});

            var israel = new Block() { Ruler = rule, Title = "Israel", StartValue = 780, EndValue = 722 };
            israel.AddEntry(new Entry() {StartValue = 746, Text = "Zechariah 746/5", Location = PrintLocation.Left});
            israel.AddEntry(new Entry() { StartValue = 745, Text = "Shallum 745", Location = PrintLocation.Left });
            israel.AddEntry(new Entry() { StartValue = 745, Text = "Menahem 745-737", Location = PrintLocation.Right });
            israel.AddEntry(new Entry() { StartValue = 737, Text = "Pekahiah 737/6", Location = PrintLocation.Left });
            israel.AddEntry(new Entry() { StartValue = 736, Text = "Pekah 736-732", Location = PrintLocation.Right });
            israel.AddEntry(new Entry() { StartValue = 732, Text = "Hoshea 732-724", Location = PrintLocation.Left });
            israel.AddEntry(new Entry(){StartValue = 756, Text = "(Amos)", Location = PrintLocation.Right});
            israel.AddEntry(750,736,"(Hosea)", PrintLocation.Right);

            var assyria = new Block() { Ruler = rule, Title = "Assyria", StartValue = 780, EndValue = 604, Width = 29};
            assyria.AddEntry(777,750,"Assyrian weakness");
            assyria.AddEntry(746,"Tiglath-pileser III 745-727");
            assyria.AddEntry(726,"Shalmaneser V 726-722");
            assyria.AddEntry(721,"Sargon II 721-705");
            assyria.AddEntry(704,"Sennacherib 704-681");
            assyria.AddEntry(680,"Esarhaddon 680-669");
            assyria.AddEntry(668,"Asshurbanapal 668-627");
            assyria.AddEntry(629,"Sin-shar-ishkun 629-612");
            assyria.AddEntry(new Entry() { StartValue = 609, Text = "Asshur-uballit II 612-609" });

            var babylon = new Block() {Ruler = rule, Title = "Babylon", StartValue = 666,EndValue = 542};
            babylon.AddEntry(626,"Nabopolassar 626-605");
            babylon.AddEntry(609, "Nebuchadnezzar 605-562");
            babylon.AddEntry(562,"Amel-marduk 562-560");
            babylon.AddEntry(560,"Neriglissar 560-556");
            babylon.AddEntry(556,"Nabonidus 556-539");

            var persia = new Block() {Ruler = rule, Title = "Persia/Media", StartValue = 675, EndValue = 500};
            persia.AddEntry(670,638,"Medes");
            persia.AddEntry(625,"Cyaxares 625-585");
            persia.AddEntry(585,"Astyages 585-550");
            persia.AddEntry(550,"Cyrus (the Great) 550-530");
            persia.AddEntry(530,"Cambyses 530-522");
            persia.AddEntry(520, "Darius I 522-486");

            var greece = new Block() {Ruler = rule, Title = "Archaic Greece", Width = 24};
            greece.AddEntry(776,"First Olympic Games");
            greece.AddEntry(new Entry(){StartValue = 750, Text = "(Homer)",Location = PrintLocation.Right});
            greece.AddEntry(710, "Spread of Greek colonies");
            greece.AddEntry(new Entry(){StartValue = 550, Text = "(Xenophanes)", Location = PrintLocation.Right});
            greece.AddEntry(new Entry(){StartValue = 540, Text = "(Pythagoras)", Location = PrintLocation.Right});
            greece.AddEntry(743, 724, "First Messenian War");
            greece.AddEntry(685,668,"Second Messenian War");

            var rome = new Block() { Ruler = rule, Title = "Kingdom of Rome" };
            rome.AddEntry(753, new string(Config.GraphChars.Rail,3) + "Rome founded" + new string(Config.GraphChars.Rail,3));
            rome.AddEntry(748,"Romulus 753-716");
            rome.AddEntry(717, "Numa Pompilius 715-672");
            rome.AddEntry(674, "Tullus Hostilius 672-640");
            rome.AddEntry(643, "Ancus Marcius 640-616");
            rome.AddEntry(616,"Lucius T. Priscus 616-578");
            rome.AddEntry(578,"Servius Tullius 578-534");
            rome.AddEntry(534, "Lucius T Superbus 534-510");
            rome.AddEntry(509, new string(Config.GraphChars.Rail, 3) + "Republic Founded" + new string(Config.GraphChars.Rail, 3));

            var plate = new Plate() { Ruler = rule, Name = "Mid-Eighth to Fifth Centuries BCE" };
            plate.AddArrow(new Arrow(assyria,egypt){StartValue = 663,Text = "Thebes sacked(663)"});
            plate.AddArrow(new Arrow(babylon,egypt){StartValue = 568, Text = "Nebuchadnezzer invades"});
            plate.AddArrow(new Arrow(persia,babylon){StartValue = 539, Text = "Fall of Babylon(539)"});
            plate.AddArrow(new Arrow(persia,egypt){StartValue = 525, Text = "Fall of Egypt(525)"});
            plate.AddArrow(new Arrow(assyria,israel){StartValue = 720, Text = "Fall of Samaria(722)"});
            plate.AddArrow(new Arrow(babylon, judah) { StartValue = 587, Text = "Fall of Jerusalem(587)" });
            plate.AddArrow(new Arrow(babylon,assyria){StartValue = 612, Text = "Fall of Nineveh(612)"});
            plate.AddBlock(egypt);
            plate.AddBlock(judah);
            plate.AddBlock(israel);
            plate.AddBlock(assyria);
            plate.AddBlock(babylon);
            plate.AddBlock(persia);
            plate.AddBlock(greece);
            plate.AddBlock(rome);

            return plate;

        }
    }
}
