namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE922to750()
        {
            var rule = new Rule() { StartValue = 922, EndValue = 780, RuleLineSpacing = 5 };

            var egypt = new Block() { Ruler = rule, Title = "Egypt" };
            egypt.AddEntry(914,"Osorkon I 914-874");
            egypt.AddEntry(920, 780, "XXII Dynasty 935-725");
            var judah = new Block() { Ruler = rule, Title = "Judah" };
            judah.AddEntry(920,"Rehoboam 922-915");
            judah.AddEntry(915,"Abijah 915-913");
            judah.AddEntry(910,"Asa 913-873");
            judah.AddEntry(873,"Jehoshaphat 873-849");
            judah.AddEntry(849,"Jehoram 849-843");
            judah.AddEntry(843,"Ahaziah 843/2");
            judah.AddEntry(839,"Athaliah 842-837");
            judah.AddEntry(837,"Joash 837-800");
            judah.AddEntry(800,"Amaziah 800-783");
            judah.AddEntry(783,"Uzziah (Azariah) 783-742");

            var israel = new Block() { Ruler = rule, Title = "Israel"};
            israel.AddEntry(920,"Jeroboam I 922-901");
            israel.AddEntry(901,"Nadab 901-900");
            israel.AddEntry(897,"Baasha 900-877");
            israel.AddEntry(877,"Elah 877-876");
            israel.AddEntry(new Entry(){StartValue = 876,Text = "Zimri 876",Location = PrintLocation.Left});
            israel.AddEntry(new Entry(){StartValue = 876,Text= "Omri 876-869",Location = PrintLocation.Right});
            israel.AddEntry(new Entry(){StartValue = 869, Text = "Ahab 869-850",Location = PrintLocation.Left});
            israel.AddEntry(new Entry(){StartValue = 849, Text = "Jehoram 849-843/2", Location = PrintLocation.Left});
            israel.AddEntry(new Entry(){StartValue = 843,Text = "Jehu 843/2-815", Location = PrintLocation.Left});
            israel.AddEntry(new Entry(){StartValue = 815, Text = "Jehoahaz 815-802", Location = PrintLocation.Left});
            israel.AddEntry(new Entry() { StartValue = 802, Text = "Jehoash 802-786", Location =PrintLocation.Left});
            israel.AddEntry(new Entry(){StartValue = 786,Text = "Jeroboam II 786-746", Location = PrintLocation.Left});
            israel.AddEntry(869,850,"(Elijah)",PrintLocation.Right);
            israel.AddEntry(848,802,"(Elisha)",PrintLocation.Right);

            var damascus = new Block() { Ruler = rule, Title = "Damascus"};
            damascus.AddEntry(885, "Ben-hadad I 885-870");
            damascus.AddEntry(870,"Ben-hadad II 870-842");
            damascus.AddEntry(842,"Hazael 842-806");
            damascus.AddEntry(806,"Ben-hadad III");

            var assyria = new Block() { Ruler = rule, Title = "Assyria", Width = 34 };
            assyria.AddEntry(912,"Adad-niari II 912-892");
            assyria.AddEntry(884,"Asshur-nasir-pal II 884-860");
            assyria.AddEntry(859,"Shalmaneser III 859-825");
            assyria.AddEntry(824,"Shamshi-adad V 824-812");
            assyria.AddEntry(811,"Adad-nirari III 811-784");
            var plate = new Plate() {Ruler = rule, Name = "Schism to Mid-Eighth Century BCE"};
            plate.AddArrow(new Arrow(egypt,judah){StartValue = 910});
            plate.AddArrow(new Arrow(assyria,israel){StartValue = 853,Text = "Battle of Qarqar" + new string(Constants.GraphChars.DirectShaft,3)});
            plate.AddArrow(new Arrow(assyria,damascus){StartValue = 796, Text = "Siege of Damascus"});
            plate.AddBlock(egypt);
            plate.AddBlock(judah);
            plate.AddBlock(israel);
            plate.AddBlock(damascus);
            plate.AddBlock(assyria);

            return plate;

        }
    }
}
