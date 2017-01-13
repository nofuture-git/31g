namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE1250to900()
        {
            var rule = new Rule { StartValue = 1250, EndValue = 900, RuleLineSpacing = 5 };

            var egypt = new Block { Ruler = rule, Title = "Egypt",Width = 30};
            egypt.AddEntry(new LeaderEntry("Marniptah", new int?[,] { { 1224, 1211 } }) {StartValue = 1224 });
            egypt.AddEntry(1215,  "(Sea Peoples defeated)");
            egypt.AddEntry(new LeaderEntry("Ramesses III", new int?[,] { { 1183, 1152 } }) { StartValue = 1175 });
            egypt.AddEntry(1150,  "(Sea Peoples defeated)" );
            egypt.AddEntry(new LeaderEntry("Ramesses IV-XI", new int?[,] { { 1152, 1069 } }) { StartValue = 1125 });
            egypt.AddEntry(1100,  "Egyptian empire ends");
            egypt.AddEntry(1000,  "XXI (Tanite) Dynasty 1069-935");
            egypt.AddEntry(935,  "XXII Dynasty 935-725");
            egypt.AddEntry(new LeaderEntry("Shishak", new int?[,] { { 935, 914 } }) { StartValue = 925 });
            egypt.AddEntry(1185, 1069, "XX Dyn. 1185-1069", PrintLocation.Left );
            egypt.AddEntry(1206, 1150, "Late Bronze Age collapse", PrintLocation.Right);

            var palestine = new Block { Ruler = rule, Title = "Palestine"};
            palestine.AddEntry(1225, "Israelite Conquest of Palestine 1250-1200");
            palestine.AddEntry(1210, "Beginning of Iron Age 1200-1000");
            palestine.AddEntry(1190,"Philistines settle in Palestine", PrintLocation.Right );
            palestine.AddEntry(1149,"Deborah");
            palestine.AddEntry(1145,"Gideon");
            palestine.AddEntry(1050,"Fall of Shiloh - Ark captured");
            palestine.AddEntry(1030, "Samuel");
            palestine.AddEntry(new LeaderEntry("Saul", new int?[,] { { 1020, 1000 } }) {StartValue = 1010 });
            palestine.AddEntry(new LeaderEntry("David", new int?[,] { { 1000, 961 } }) { StartValue = 1000 });
            palestine.AddEntry(new LeaderEntry("Solomon", new int?[,] { { 962, 922 } }) { StartValue = 961 });

            palestine.AddEntry(1200,1020,"The Period of the Judges 1200-1020");

            var judah = new Block {Ruler = rule, Title = "Judah", StartValue = 922};
            var israel = new Block { Ruler = rule, Title = "Israel", StartValue = 922 };
            var damascus = new Block { Ruler = rule, Title = "Damascus", StartValue = 945 };
            damascus.AddEntry(940,900,"Rezon");
            palestine.AddInnerBlock(judah);
            palestine.AddInnerBlock(israel);
            palestine.AddInnerBlock(damascus);

            var assyria = new Block { Ruler = rule, Title = "Assyria", Width = 28};
            assyria.AddEntry(new LeaderEntry("Tukulti-ninurta I", new int?[,] { { 1234, 1197 } }) {StartValue = 1234, EndValue = 1197 });
            assyria.AddEntry(1180, "Assyrian weakness");
            assyria.AddEntry(1100,"(brief Assyrian revival)");
            assyria.AddEntry(new LeaderEntry("Tiglath-pileser I", new int?[,] { { 1116, 1078 } }) { StartValue = 1116, EndValue = 1078 });
            assyria.AddEntry(1050, "Assyrian weakness");
            assyria.AddEntry(1010,"Aramean pressure",PrintLocation.Left);
            assyria.AddEntry(new LeaderEntry("Asshur-dan II", new int?[,] { { 935, 913 } }) { StartValue = 935 });
            assyria.AddEntry(925,"(Assryia's recovery begins)");

            var plate = new Plate { Ruler = rule, Name = "Near East 1250 to 900 BCE" };
            plate.AddBlock(egypt);
            plate.AddBlock(palestine);
            plate.AddBlock(assyria);
            return plate;
        }
    }
}
