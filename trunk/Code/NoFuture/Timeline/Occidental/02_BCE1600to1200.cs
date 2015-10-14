namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE1600to1200()
        {
            var rule = new Rule() {StartValue = 1600, EndValue = 1200, RuleLineSpacing = 5};

            var egypt = new Block() {Ruler = rule, Title = "Egypt"};
            egypt.AddEntry(new Entry() { StartValue = 1306, EndValue = 1200, Text = "XIX Dynasty", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry() { StartValue = 1530, EndValue = 1315, Text = "XVIII Dynasty", Location = PrintLocation.Left });

            egypt.AddEntry(new Entry() { StartValue = 1570, Text = "New Kingdom (Empire)" });
            egypt.AddEntry(new Entry() {StartValue = 1552, Text = "Amosis 1552-1527", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry(){StartValue = 1560, Text = "Hyksos expelled", Location = PrintLocation.Left});
            egypt.AddEntry(new Entry(){StartValue = 1520, Text = "Amenophis I 1527-1507", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry() { StartValue = 1507, Text = "Thutmosis I 1507-1494", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry(){StartValue = 1494,Text = "Thutmosis II 1494-1490", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry(){StartValue = 1490, Text = "Thutmosis III 1490-1436", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry() { StartValue = 1438, Text = "Amenophis II 1438-1412", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry() { StartValue = 1412, Text = "Thutmosis IV 1412-1403", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry(){StartValue = 1390, Text = "Amenophis III 1403-1364",Location = PrintLocation.Right});
            egypt.AddEntry(new Entry(){StartValue = 1364, Text = "Akhenaten 1364-1347", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry() { StartValue = 1333, Text = "Haremhab 1333-1306", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry(){StartValue = 1295,Text = "Sethos I 1305-1290", Location = PrintLocation.Right});
            egypt.AddEntry(new Entry() { StartValue = 1290, Text = "Ramesses II 1290-1224", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry(){StartValue = 1224, Text = "Marniptah 1224-1211"});
            egypt.AddEntry(new Entry(){StartValue = 1211, Text = "(weakness and anarchy)"});

            egypt.AddEntry(new Entry() { StartValue = 1403, EndValue = 1347, Text = "The Amarna Period", Location = PrintLocation.Center });

            var palestine = new Block() {Ruler = rule, Title = "Palestine"};
            palestine.AddEntry(new Entry(){StartValue = 1550, Text = "Late Bronze Age"});
            palestine.AddEntry(new Entry(){StartValue = 1500, Text = "Hebrews in Egypt"});
            palestine.AddEntry(new Entry(){StartValue = 1340, Text = "Hebrews in Egypt"});
            palestine.AddEntry(new Entry(){StartValue = 1280, Text = "Exodus"});
            palestine.AddEntry(new Entry(){StartValue = 1250, EndValue = 1200, Text = "Conquest"});

            var hittities = new Block() { Ruler = rule, Title = "Hittites"};
            hittities.AddEntry(new Entry(){StartValue = 1530, Text = "Mursilis I"});
            hittities.AddEntry(new Entry() {StartValue = 1375, Text = "Shuppiluliuma 1375-1335"});
            hittities.AddEntry(new Entry() {StartValue = 1306, Text = "Muwattalis 1306-1282"});
            hittities.AddEntry(new Entry() {StartValue = 1280, Text = "Hattusilis III 1275-1250", Location = PrintLocation.Right});
            hittities.AddEntry(new Entry() { StartValue = 1550, EndValue = 1410, Text = "Old Kingdom" });
            hittities.AddEntry(new Entry() { StartValue = 1400, EndValue = 1220, Text = "Hittite Empire" });

            var mitanni = new Block() {Ruler = rule, Title = "Mitanni"};
            mitanni.AddEntry(new Entry(){StartValue = 1480, Text = "Shuttarna I"});
            mitanni.AddEntry(new Entry() {StartValue = 1455, Text = "Saushsatar"});
            mitanni.AddEntry(new Entry() {StartValue = 1420, Text = "Artatama"});
            mitanni.AddEntry(new Entry() {StartValue = 1400, Text = "Shuttarna II"});
            mitanni.AddEntry(new Entry() {StartValue = 1380,Text = "Tushratta"});
            mitanni.AddEntry(new Entry() { StartValue = 1540, EndValue = 1370, Text = "*" });

            var assyria = new Block() {Ruler = rule, Title = "Assyria"};
            assyria.AddEntry(new Entry(){StartValue = 1356, Text = "Asshur-uballit I 1356-1321"});
            assyria.AddEntry(new Entry(){StartValue = 1297, Text = "Adad-nirari I 1297-1266"});
            assyria.AddEntry(new Entry() {StartValue = 1265, Text = "Shalmaneser I 1265-1235"});
            assyria.AddEntry(new Entry() {StartValue = 1234, Text = "Tukulti-ninurata I 1234-1197"});

            var greece = new Block() {Ruler = rule, Title = "Greece"};
            greece.AddEntry(new Entry(){StartValue = 1550, Text = "Minoan & Greek first encounter"});
            greece.AddEntry(new Entry(){StartValue = 1500, Text = "Mt. Thera eruption"});
            greece.AddEntry(new Entry(){StartValue = 1580, EndValue = 1200, Text = "Mycenaean Period", Location = PrintLocation.Left});
            greece.AddEntry(new Entry(){StartValue = 1400, Text = "Mycenaean invade Minoan Crete"});
            greece.AddEntry(new Entry(){StartValue = 1240, Text = "Trojan War"});
            greece.AddEntry(new Entry(){StartValue = 1210, Text = "migration of Sea Peoples"});

            var plate = new Plate() {Ruler = rule, Name = "Near East 1600 to 1200 BCE"};

            plate.AddArrow(new Arrow(egypt, palestine){StartValue = 1457, Text = "Battle of Megiddo"});
            plate.AddArrow(new Arrow(mitanni,assyria){StartValue = 1440, Text = "Assur sacked"});
            plate.AddArrow(new Arrow(hittities,mitanni){StartValue = 1360});
            plate.AddArrow(new Arrow(hittities, egypt) { StartValue = 1274, Text = "Battle of Kadesh " });

            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(hittities);
            plate.AddBlock(mitanni);
            plate.AddBlock(assyria);
            plate.AddBlock(greece);
            return plate;
        }
    }
}
