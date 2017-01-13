namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE550to825()
        {
            var rule = new Rule {StartValue = 550, EndValue = 825, RuleLineSpacing = 7};

            var byz = new Block {Ruler = rule, Title = "Byzantine Empire", Width = 36};
            byz.AddEntry(new LeaderEntry("Maurice", new int?[,] { { 582, 602 } }) {StartValue = 582 });
            byz.AddEntry(new LeaderEntry("Phocas", new int?[,] { { 602, 610 } }) { StartValue = 602 });
            byz.AddEntry(new LeaderEntry("Heraclius", new int?[,] { { 610, 641 } }) { StartValue = 610 });
            byz.AddEntry(680, "Sixth Ecumen. Council(680)");
            byz.AddEntry(692, 717, "Twenty Years Anarchy(695-717)", PrintLocation.Left);
            byz.AddEntry(new LeaderEntry("Leo III", new int?[,] { { 717, 741 } }) { StartValue = 725 });
            byz.AddEntry(new LeaderEntry("Constantine V", new int?[,] { { 741, 775 } }) { StartValue = 746 });
            byz.AddEntry(new LeaderEntry("Artabasdos ursurpation", new int?[,] { { 741, 743 } }) { StartValue = 741 });
            byz.AddEntry(new LeaderEntry("Leo IV", new int?[,] { { 775, 780 } }) { StartValue = 775 });
            byz.AddEntry(new LeaderEntry("Constantine VI", new int?[,] { { 780, 797 } }) { StartValue = 780 });
            byz.AddEntry(new LeaderEntry("Irene [empress]", new int?[,] { { 797, 802 } }) { StartValue = 797 });
            byz.AddEntry(787, "2nd Council of Nicaea(787)[end Iconoclasm]");
            byz.AddEntry(719, 802, "Isaurian dyn(717-802)", PrintLocation.Right);
            

            var euro = new Block {Ruler = rule, Title = "West Europe", Width = 54};
            euro.AddEntry(577, "Battle of Deorham(577) - Anglo-Saxon defeat Britons");
            euro.AddEntry(589, "'filioque' intro'ed into Latin west");
            euro.AddEntry(new LeaderEntry("Pope Gregory", new int?[,] { { 590, 604 } }) {StartValue = 600 });
            euro.AddEntry(561, 614, "Gaul decends, total illiteracy", PrintLocation.Left);
            euro.AddEntry(new LiteraryWorkEntry("Beowulf", "") {StartValue = 675});
            euro.AddEntry(new PhilosopherEntry("Venerable Bede", 673, 735) {StartValue = 700 });
            euro.AddEntry(new LeaderEntry("Pope Zachary", new int?[,] { { 741, 752 } }) { StartValue = 741 });
            euro.AddEntry(new LeaderEntry("Pepin the Short", new int?[,] { { 751, 768 } }) { StartValue = 751 });
            euro.AddEntry(new LeaderEntry("Pope Stephen II", new int?[,] { { 752, 757 } }) { StartValue = 752 });
            euro.AddEntry(756, "Donation of Pepin(754)");
            euro.AddEntry(new LeaderEntry("Charlemagne-Holy Roman Emperor", new int?[,] { { null, 800 } }) { StartValue = 800 });
            euro.AddEntry(new LeaderEntry("Halfdan (the Black) first king of Norway", new int?[,] { { null, 800 } }) { StartValue = 805 });
            euro.AddEntry(793, "1st Viking raid [England](793)");

            var mus = new Block {Ruler = rule, StartValue = 565, Title = "Muslims", Width = 56};
            mus.AddEntry(new LeaderEntry("Mohammed", new int?[,] { { 569, 632 } }) {StartValue = 569, Location = PrintLocation.Left});
            mus.AddEntry(610, "vision @ Mt. Hira(610)");
            mus.AddEntry(627, "Mohammad takes Mecca(630)");
            mus.AddEntry(new LiteraryWorkEntry("Quran","Mohammed") {StartValue = 630});
            mus.AddEntry(new LeaderEntry("Abu Bekr", new int?[,] { { 633, 634 } }) { StartValue = 633, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Omar", new int?[,] { { 634, 644 } }) { StartValue = 638, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Othman", new int?[,] { { 644, 656 } }) { StartValue = 644, Location = PrintLocation.Left });
            mus.AddEntry(656, "war of succesion Ali/Muawiya(656-669)");
            mus.AddEntry(new LeaderEntry("Muawiya", new int?[,] { { 669, 680 } }) { StartValue = 669, Location = PrintLocation.Left });
            mus.AddEntry(679, "war of succesion(680-683)");
            mus.AddEntry(new LeaderEntry("Abd-al-Malik", new int?[,] { { 683, 705 } }) { StartValue = 685, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Walid", new int?[,] { { 705, 715 } }) { StartValue = 705, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Suleiman", new int?[,] { { 715, 717 } }) { StartValue = 715 });
            mus.AddEntry(new LeaderEntry("Omar II", new int?[,] { { 717, 720 } }) { StartValue = 719, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Yezid II", new int?[,] { { 720, 724 } }) { StartValue = 722, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Hisham", new int?[,] { { 724, 743 } }) { StartValue = 725, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Walid II", new int?[,] { { 743, 744 } }) { StartValue = 743, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Merwan II", new int?[,] { { 744, 750 } }) { StartValue = 747, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Abu al-Abbas", new int?[,] { { 750, 754 } }) { StartValue = 751, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Abu Jafar", new int?[,] { { 754, 775 } }) { StartValue = 756, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("al-Mahdi", new int?[,] { { 775, 784 } }) { StartValue = 775, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Abbasid", new int?[,] { { 784, 786 } }) { StartValue = 784, Location = PrintLocation.Left });
            mus.AddEntry(new LeaderEntry("Harun al-Rashid", new int?[,] { { 786, 809 } }) { StartValue = 787, Location = PrintLocation.Left });

            mus.AddEntry(738, "Battle of Rajasthan(738) [India]");

            mus.AddEntry(620, 661, "Rashidun Caliphate", PrintLocation.Right);
            mus.AddEntry(683, 750, "Umayyad Caliphate", PrintLocation.Right);
            mus.AddEntry(750, 825, "Abbasid Caliphate", PrintLocation.Right);


            var sas = new Block {Ruler = rule, Title = "Sasanian Empire", EndValue = 642, Width = 30};
            sas.AddEntry(new LeaderEntry("Khosru II", new int?[,] { { 590, 628 } }) {StartValue = 596 });

            var plate = new Plate {Ruler = rule, Name = "The Dark Ages"};
            plate.AddBlock(euro);
            plate.AddBlock(byz);
            plate.AddBlock(mus);
            plate.AddBlock(sas);

            plate.AddArrow(new Arrow(sas, byz) {StartValue = 627, Text = "seige Constantinople(626)"});
            plate.AddArrow(new Arrow(sas, byz) {StartValue = 614, Text = "capture Jerusalem(614)"});
            plate.AddArrow(new Arrow(byz, sas) {StartValue = 624, Text = "counter attack(624)"});
            plate.AddArrow(new Arrow(mus, sas) {StartValue = 642, Text = "battle of Nahavand(642)"});
            plate.AddArrow(new Arrow(mus, byz) {StartValue = 634, Text = "siege of Damascus(634)"});
            plate.AddArrow(new Arrow(mus, byz) {StartValue = 674, Text = "seige Constantinople(674)"});
            plate.AddArrow(new Arrow(mus, byz) {StartValue = 717, Text = "seige Constantinople(717)"});
            plate.AddArrow(new Arrow(mus, euro) {StartValue = 711, Text = "Moors invade Spain(711)"});
            plate.AddArrow(new Arrow(mus, byz) {StartValue = 698, Text = "Battle of Carthage(698)"});
            plate.AddArrow(new Arrow(mus, euro) {StartValue = 732, Text = "Battle of Tours(732)"});

            plate.Notes.Add(
                "Western Christendom was a beleaguered citadel which only survived because its greatest enemy, Islam, had");
            plate.Notes.Add(
                "reached the end of its lines of communication, and its lesser enemies (the Slavs, the Hungarians, and the Vikings)");
            plate.Notes.Add("were organized only for raids and for plunder.");
            return plate;
        }
    }
}
