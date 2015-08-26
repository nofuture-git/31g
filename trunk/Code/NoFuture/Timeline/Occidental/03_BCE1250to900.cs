namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE1250to900()
        {
            var rule = new Rule() { StartValue = 1250, EndValue = 900, RuleLineSpacing = 5 };

            var egypt = new Block() { Ruler = rule, Title = "Egypt",Width = 30};
            egypt.AddEntry(new Entry(){StartValue = 1224, Text = "Marniptah 1224-1211"});
            egypt.AddEntry(new Entry(){StartValue = 1215, Text = "(Sea Peoples defeated)"});
            egypt.AddEntry(new Entry(){StartValue = 1175, Text = "Ramesses III 1183-1152"});
            egypt.AddEntry(new Entry() { StartValue = 1150, Text = "(Sea Peoples defeated)" });
            egypt.AddEntry(new Entry() {StartValue = 1125, Text = "Ramesses IV-XI 1152-1069"});
            egypt.AddEntry(new Entry() {StartValue = 1100, Text = "Egyptian empire ends"});
            egypt.AddEntry(new Entry(){StartValue = 1000, Text = "XXI (Tanite) Dynasty 1069-935"});
            egypt.AddEntry(new Entry(){StartValue = 935, Text = "XXII Dynasty 935-725"});
            egypt.AddEntry(new Entry(){StartValue = 925, Text = "Shishak 935-914"});
            egypt.AddEntry(new Entry() { StartValue = 1185, EndValue = 1069, Text = "XX Dyn. 1185-1069", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry() { StartValue = 1206, EndValue = 1150, Text = "Late Bronze Age collapse", Location = PrintLocation.Right});

            var palestine = new Block() { Ruler = rule, Title = "Palestine"};
            palestine.AddEntry(1225, "Israelite Conquest of Palestine 1250-1200");
            palestine.AddEntry(1210, "Beginning of Iron Age 1200-1000");
            palestine.AddEntry(new Entry() { StartValue = 1190, Text = "Philistines settle in Palestine", Location = PrintLocation.Right });
            palestine.AddEntry(1149,"Deborah");
            palestine.AddEntry(1145,"Gideon");
            palestine.AddEntry(1050,"Fall of Shiloh - Ark captured");
            palestine.AddEntry(1030, "Samuel");
            palestine.AddEntry(1010, "Saul 1020-1000");
            palestine.AddEntry(1000,"David 1000-961");
            palestine.AddEntry(961,"Solomon 962-922");
            
            palestine.AddEntry(1200,1020,"The Period of the Judges 1200-1020");

            var judah = new Block() {Ruler = rule, Title = "Judah", StartValue = 922};
            var israel = new Block() { Ruler = rule, Title = "Israel", StartValue = 922 };
            var damascus = new Block() { Ruler = rule, Title = "Damascus", StartValue = 945 };
            damascus.AddEntry(940,900,"Rezon");
            palestine.AddInnerBlock(judah);
            palestine.AddInnerBlock(israel);
            palestine.AddInnerBlock(damascus);

            var assyria = new Block() { Ruler = rule, Title = "Assyria", Width = 28};

            assyria.AddEntry(1234,1197,"Tukulti-ninurta I 1234-1197");
            assyria.AddEntry(1180, "Assyrian weakness");
            assyria.AddEntry(1100,"(brief Assyrian revival)");
            assyria.AddEntry(1116,1078,"Tiglath-pileser I 1116-1078");
            assyria.AddEntry(1050, "Assyrian weakness");
            assyria.AddEntry(new Entry(){StartValue = 1010,Text = "Aramean pressure",Location = PrintLocation.Left});
            assyria.AddEntry(935,"Asshur-dan II 935-913");
            assyria.AddEntry(925,"(Assryia's recovery begins)");

            var plate = new Plate() { Ruler = rule, Name = "Near East 1250 to 900 BCE" };
            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(assyria);
            return plate;
        }
    }
}
