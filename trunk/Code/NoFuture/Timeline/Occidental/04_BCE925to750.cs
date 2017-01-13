namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE922to750()
        {
            var rule = new Rule {StartValue = 922, EndValue = 780, RuleLineSpacing = 5};

            var egypt = new Block {Ruler = rule, Title = "Egypt"};
            egypt.AddEntry(914, "Osorkon I 914-874");
            egypt.AddEntry(920, 780, "XXII Dynasty 935-725");
            var judah = new Block {Ruler = rule, Title = "Judah"};
            judah.AddEntry(new LeaderEntry("Rehoboam", new int?[,] {{922, 915}}) {StartValue = 920});
            judah.AddEntry(new LeaderEntry("Abijah", new int?[,] {{915, 913}}) {StartValue = 915});
            judah.AddEntry(new LeaderEntry("Asa", new int?[,] {{913, 873}}) {StartValue = 910});
            judah.AddEntry(new LeaderEntry("Jehoshaphat", new int?[,] {{873, 849}}) {StartValue = 873});
            judah.AddEntry(new LeaderEntry("Jehoram", new int?[,] {{849, 843}}) {StartValue = 849});
            judah.AddEntry(new LeaderEntry("Ahaziah", new int?[,] {{843, 842}}) {StartValue = 843});
            judah.AddEntry(new LeaderEntry("Athaliah", new int?[,] {{842, 837}}) {StartValue = 839});
            judah.AddEntry(new LeaderEntry("Joash", new int?[,] {{837, 800}}) {StartValue = 837});
            judah.AddEntry(new LeaderEntry("Amaziah", new int?[,] {{800, 783}}) {StartValue = 800});
            judah.AddEntry(new LeaderEntry("Uzziah (Azariah)", new int?[,] {{783, 742}}) {StartValue = 783});

            var israel = new Block {Ruler = rule, Title = "Israel"};
            israel.AddEntry(new LeaderEntry("Jeroboam I", new int?[,] {{922, 901}}) {StartValue = 920});
            israel.AddEntry(new LeaderEntry("Nadab", new int?[,] {{901, 900}}) {StartValue = 901});
            israel.AddEntry(new LeaderEntry("Baasha", new int?[,] {{900, 877}}) {StartValue = 897});
            israel.AddEntry(new LeaderEntry("Zimri & Elah ", new int?[,] {{null, 876}, {877, 876}})
            {
                StartValue = 876,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Omri", new int?[,] {{876, 869}})
            {
                StartValue = 876,
                Location = PrintLocation.Right
            });
            israel.AddEntry(new LeaderEntry("Ahab", new int?[,] {{869, 850}})
            {
                StartValue = 869,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Jehoram", new int?[,] {{849, 843}})
            {
                StartValue = 849,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Jehu", new int?[,] {{843, 815}})
            {
                StartValue = 843,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Jehoahaz", new int?[,] {{815, 802}})
            {
                StartValue = 815,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Jehoash", new int?[,] {{802, 786}})
            {
                StartValue = 802,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new LeaderEntry("Jeroboam II", new int?[,] {{786, 746}})
            {
                StartValue = 786,
                Location = PrintLocation.Left
            });
            israel.AddEntry(new ProphetEntry("Elijah")
            {
                StartValue = 869,
                EndValue = 850,
                Location = PrintLocation.Right
            });

            israel.AddEntry(new ProphetEntry("Elisha")
            {
                StartValue = 848,
                EndValue = 802,
                Location = PrintLocation.Right
            });

            var damascus = new Block {Ruler = rule, Title = "Damascus"};
            damascus.AddEntry(new LeaderEntry("Ben-hadad I", new int?[,] { { 885, 870 } }) {StartValue = 885 });
            damascus.AddEntry(new LeaderEntry("Ben-hadad II", new int?[,] { { 870, 842 } }) { StartValue = 870 });
            damascus.AddEntry(new LeaderEntry("Hazael", new int?[,] { { 842, 806 } }) { StartValue = 842 });
            damascus.AddEntry(new LeaderEntry("Ben-hadad III", 806) { StartValue = 806 });

            var assyria = new Block {Ruler = rule, Title = "Assyria", Width = 34};
            assyria.AddEntry(new LeaderEntry("Adad-niari II", new int?[,] { { 912, 892 } }) {StartValue = 912 });
            assyria.AddEntry(new LeaderEntry("Asshur-nasir-pal II", new int?[,] { { 884, 860 } }) { StartValue = 884 });
            assyria.AddEntry(new LeaderEntry("Shalmaneser III", new int?[,] { { 859, 825 } }) { StartValue = 859 });
            assyria.AddEntry(new LeaderEntry("Shamshi-adad V", new int?[,] { { 824, 812 } }) { StartValue = 824 });
            assyria.AddEntry(new LeaderEntry("Adad-nirari III", new int?[,] { { 811, 784 } }) { StartValue = 811 });

            var plate = new Plate {Ruler = rule, Name = "Schism to Mid-Eighth Century BCE"};
            plate.AddArrow(new Arrow(egypt, judah) {StartValue = 910});
            plate.AddArrow(new Arrow(assyria, israel)
            {
                StartValue = 853,
                Text = "Battle of Qarqar" + new string(Config.GraphChars.DirectShaft, 3)
            });
            plate.AddArrow(new Arrow(assyria, damascus) {StartValue = 796, Text = "Siege of Damascus"});
            plate.AddBlock(egypt);
            plate.AddBlock(judah);
            plate.AddBlock(israel);
            plate.AddBlock(damascus);
            plate.AddBlock(assyria);

            return plate;

        }
    }
}
