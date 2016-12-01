namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE1600to1200()
        {
            var rule = new Rule {StartValue = 1600, EndValue = 1200, RuleLineSpacing = 5};

            var egypt = new Block {Ruler = rule, Title = "Egypt"};
            egypt.AddEntry(new Entry {StartValue = 1306, EndValue = 1200, Text = "XIX Dynasty", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry {StartValue = 1530, EndValue = 1315, Text = "XVIII Dynasty", Location = PrintLocation.Left });

            egypt.AddEntry(1570,"New Kingdom (Empire)" );
            egypt.AddEntry(1552,"Amosis 1552-1527",PrintLocation.Right);
            egypt.AddEntry(1560,"Hyksos expelled",PrintLocation.Left);
            egypt.AddEntry(1520,"Amenophis I 1527-1507",PrintLocation.Right);
            egypt.AddEntry(1507,"Thutmosis I 1507-1494",PrintLocation.Right);
            egypt.AddEntry(1494,"Thutmosis II 1494-1490",PrintLocation.Right);
            egypt.AddEntry(1490,"Thutmosis III 1490-1436",PrintLocation.Right);
            egypt.AddEntry(1438,"Amenophis II 1438-1412",PrintLocation.Right);
            egypt.AddEntry(1412,"Thutmosis IV 1412-1403",PrintLocation.Right);
            egypt.AddEntry(1390,"Amenophis III 1403-1364",PrintLocation.Right);
            egypt.AddEntry(1364,"Akhenaten 1364-1347",PrintLocation.Right);
            egypt.AddEntry(1333,"Haremhab 1333-1306",PrintLocation.Right);
            egypt.AddEntry(1295,"Sethos I 1305-1290",PrintLocation.Right);
            egypt.AddEntry(1290,"Ramesses II 1290-1224",PrintLocation.Right);
            egypt.AddEntry(1224,"Marniptah 1224-1211");
            egypt.AddEntry(1211,"(weakness and anarchy)");

            egypt.AddEntry(1403,1347,"The Amarna Period",PrintLocation.Center);

            var palestine = new Block {Ruler = rule, Title = "Palestine"};
            palestine.AddEntry(1550,"Late Bronze Age");
            palestine.AddEntry(1500,"Hebrews in Egypt");
            palestine.AddEntry(1340,"Hebrews in Egypt");
            palestine.AddEntry(1280,"Exodus");
            palestine.AddEntry(1250,1200,"Conquest");

            var hittities = new Block { Ruler = rule, Title = "Hittites"};
            hittities.AddEntry(1530,"Mursilis I");
            hittities.AddEntry(1375,"Shuppiluliuma 1375-1335");
            hittities.AddEntry(1306,"Muwattalis 1306-1282");
            hittities.AddEntry(1280,"Hattusilis III 1275-1250",PrintLocation.Right);
            hittities.AddEntry(1550,1410,"Old Kingdom" );
            hittities.AddEntry(1400,1220,"Hittite Empire" );

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
