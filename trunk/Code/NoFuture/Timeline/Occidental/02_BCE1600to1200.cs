namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE1600to1200()
        {
            var rule = new Rule {StartValue = 1600, EndValue = 1200, RuleLineSpacing = 5};

            var egypt = new Block {Ruler = rule, Title = "Egypt"};

            egypt.AddEntry(1570, "New Kingdom (Empire)");

            egypt.AddEntry(new LeaderEntry("Amosis", new int?[,] {{1552, 1527}}));
            egypt.AddEntry(1560, "Hyksos expelled", PrintLocation.Left);
            egypt.AddEntry(new LeaderEntry("Amenophis I", new int?[,] {{1527, 1507}}) {StartValue = 1520});
            egypt.AddEntry(new LeaderEntry("Thutmosis I", new int?[,] {{1507, 1494}}));
            egypt.AddEntry(new LeaderEntry("Thutmosis II", new int?[,] {{1494, 1490}}));
            egypt.AddEntry(new LeaderEntry("Thutmosis III", new int?[,] {{1490, 1436}}));
            egypt.AddEntry(new LeaderEntry("Amenophis II", new int?[,] {{1438, 1412}}));
            egypt.AddEntry(new LeaderEntry("Thutmosis IV", new int?[,] {{1412, 1403}}));
            egypt.AddEntry(new LeaderEntry("Amenophis III", new int?[,] {{1403, 1364}}) {StartValue = 1390});
            egypt.AddEntry(new LeaderEntry("Akhenaten", new int?[,] {{1364, 1347}}));
            egypt.AddEntry(new LeaderEntry("Haremhab", new int?[,] {{1333, 1306}}));
            egypt.AddEntry(new LeaderEntry("Sethos I", new int?[,] {{1305, 1290}}) {StartValue = 1295});
            egypt.AddEntry(new LeaderEntry("Ramesses II", new int?[,] {{1290, 1224}}));
            egypt.AddEntry(new LeaderEntry("Marniptah", new int?[,] {{1224, 1211}}));
            egypt.AddEntry(1211, "(weakness and anarchy)");

            egypt.AddEntry(1306, 1200, "XIX Dynasty", PrintLocation.Left);
            egypt.AddEntry(1530, 1315, "XVIII Dynasty", PrintLocation.Left);
            egypt.AddEntry(1403, 1347, "The Amarna Period", PrintLocation.Center);

            var palestine = new Block {Ruler = rule, Title = "Palestine"};
            palestine.AddEntry(1550, "Late Bronze Age");
            palestine.AddEntry(1500, "Hebrews in Egypt");
            palestine.AddEntry(1340, "Hebrews in Egypt");
            palestine.AddEntry(1280, "Exodus");
            palestine.AddEntry(1250, 1200, "Conquest");

            var hittities = new Block {Ruler = rule, Title = "Hittites"};
            hittities.AddEntry(new LeaderEntry("Mursilis I", 1530));
            hittities.AddEntry(new LeaderEntry("Shuppiluliuma", new int?[,] {{1375, 1335}}));
            hittities.AddEntry(new LeaderEntry("Muwattalis", new int?[,] {{1306, 1282}}));
            hittities.AddEntry(new LeaderEntry("Hattusilis III", new int?[,] {{1275, 1250}}) {StartValue = 1280});
            hittities.AddEntry(1550, 1410, "Old Kingdom");
            hittities.AddEntry(new ScienceAdvEntry("steel", "") {StartValue = 1400});
            hittities.AddEntry(1380, 1220, "Hittite Empire");

            var mitanni = new Block {Ruler = rule, Title = "Mitanni"};
            mitanni.AddEntry(new LeaderEntry("Shuttarna I", 1480));
            mitanni.AddEntry(new LeaderEntry("Saushsatar", 1455));
            mitanni.AddEntry(new LeaderEntry("Artatama", 1420));
            mitanni.AddEntry(new LeaderEntry("Shuttarna II", 1400));
            mitanni.AddEntry(new LeaderEntry("Tushratta", 1380));
            mitanni.AddEntry(new Entry {StartValue = 1540, EndValue = 1370, Text = "*"});

            var assyria = new Block {Ruler = rule, Title = "Assyria"};
            assyria.AddEntry(new LeaderEntry("Asshur-uballit I", new int?[,] {{1356, 1321}}));
            assyria.AddEntry(new LeaderEntry("Adad-nirari I", new int?[,] {{1297, 1266}}));
            assyria.AddEntry(new LeaderEntry("Shalmaneser I", new int?[,] {{1265, 1235}}));
            assyria.AddEntry(new LeaderEntry("Tukulti-ninurata I", new int?[,] {{1234, 1197}}));

            var greece = new Block() {Ruler = rule, Title = "Greece"};
            greece.AddEntry(1550, "Minoan & Greek first encounter");
            greece.AddEntry(1500, "Mt. Thera eruption");
            greece.AddEntry(1580, 1200, "Mycenaean Period", PrintLocation.Left);
            greece.AddEntry(1400, "Mycenaean invade Minoan Crete");
            greece.AddEntry(1240, "Trojan War");
            greece.AddEntry(1210, "migration of Sea Peoples");

            var plate = new Plate() {Ruler = rule, Name = "Near East 1600 to 1200 BCE"};

            plate.AddArrow(new Arrow(egypt, palestine) {StartValue = 1457, Text = "Battle of Megiddo"});
            plate.AddArrow(new Arrow(mitanni, assyria) {StartValue = 1440, Text = "Assur sacked"});
            plate.AddArrow(new Arrow(hittities, mitanni) {StartValue = 1360});
            plate.AddArrow(new Arrow(hittities, egypt) {StartValue = 1274, Text = "Battle of Kadesh "});

            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(hittities);
            plate.AddBlock(mitanni);
            plate.AddBlock(assyria);
            plate.AddBlock(greece);
            plate.FileName = "02_BCE1600to1200";

            return plate;
        }
    }
}
