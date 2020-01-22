namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE550to825()
        {
            var rule = new Rule {StartValue = 550, EndValue = 825, RuleLineSpacing = 7};

            var byz = new Block {Ruler = rule, Title = "Byzantine Empire", Width = 36};
            byz.AddEntry(new LeaderEntry("Maurice", new int?[,] { { 582, 602 } }));
            byz.AddEntry(new LeaderEntry("Phocas", new int?[,] { { 602, 610 } }));
            byz.AddEntry(new LeaderEntry("Heraclius", new int?[,] { { 610, 641 } }));
            byz.AddEntry(680, "Sixth Ecumen. Council(680)");
            byz.AddEntry(new ScienceAdvEntry("greek fire","") {StartValue = 669});
            byz.AddEntry(692, 717, "Twenty Years Anarchy(695-717)", PrintLocation.Left);
            byz.AddEntry(new LeaderEntry("Leo III", new int?[,] { { 717, 741 } }) { StartValue = 725 });
            byz.AddEntry(new LeaderEntry("Constantine V", new int?[,] { { 741, 775 } }) { StartValue = 746 });
            byz.AddEntry(new LeaderEntry("Artabasdos ursurpation", new int?[,] { { 741, 743 } }));
            byz.AddEntry(new LeaderEntry("Leo IV", new int?[,] { { 775, 780 } }));
            byz.AddEntry(new LeaderEntry("Constantine VI", new int?[,] { { 780, 797 } }));
            byz.AddEntry(new LeaderEntry("Irene [empress]", new int?[,] { { 797, 802 } }));
            byz.AddEntry(787, "2nd Council of Nicaea(787)[end Iconoclasm]");
            byz.AddEntry(719, 802, "Isaurian dyn(717-802)", PrintLocation.Right);
            

            var euro = new Block {Ruler = rule, Title = "West Europe", Width = 54};
            euro.AddEntry(577, "Battle of Deorham(577) - Anglo-Saxon defeat Britons");
            euro.AddEntry(589, "'filioque' intro'ed into Latin west");
            euro.AddEntry(new LeaderEntry("Pope Gregory", new int?[,] { { 590, 604 } }) {StartValue = 600 });
            euro.AddEntry(new LeaderEntry("Pope Zachary", new int?[,] { { 741, 752 } }));
            euro.AddEntry(new LeaderEntry("Pepin the Short", new int?[,] { { 751, 768 } }));
            euro.AddEntry(new LeaderEntry("Pope Stephen II", new int?[,] {{752, 757}}) {Location = PrintLocation.Right});
            euro.AddEntry(new LeaderEntry("Charlemagne-Holy Roman Emperor", 800));
            euro.AddEntry(new LeaderEntry("Halfdan (the Black) first king of Norway", 800) { StartValue = 805 });
            euro.AddEntry(new LiteraryWorkEntry("Beowulf", "") { StartValue = 675 });
            euro.AddEntry(new PhilosopherEntry("Venerable Bede", 673, 735) { StartValue = 700 });
            euro.AddEntry(561, 614, "Gaul decends, total illiteracy", PrintLocation.Left);
            euro.AddEntry(756, "Donation of Pepin(754)");
            euro.AddEntry(793, "1st Viking raid [England](793)");

            var mus = new Block {Ruler = rule, StartValue = 565, Title = "Muslims", Width = 56};
            mus.AddEntry(new LeaderEntry("Mohammed", new int?[,] { { 569, 632 } }));
            mus.AddEntry(610, "vision @ Mt. Hira(610)");
            mus.AddEntry(627, "Mohammad takes Mecca(630)");
            mus.AddEntry(new LeaderEntry("Abu Bekr", new int?[,] { { 633, 634 } }) {Location = PrintLocation.Right});
            mus.AddEntry(new LeaderEntry("Omar", new int?[,] { { 634, 644 } }) { StartValue = 638 });
            mus.AddEntry(new LeaderEntry("Othman", new int?[,] { { 644, 656 } }));
            mus.AddEntry(656, "war of succesion Ali/Muawiya(656-669)");
            mus.AddEntry(new LeaderEntry("Muawiya", new int?[,] { { 669, 680 } }));
            mus.AddEntry(679, "war of succesion(680-683)");
            mus.AddEntry(new LeaderEntry("Abd-al-Malik", new int?[,] { { 683, 705 } }) { StartValue = 685 });
            mus.AddEntry(new LeaderEntry("Walid", new int?[,] { { 705, 715 } }));
            mus.AddEntry(new LeaderEntry("Suleiman", new int?[,] { { 715, 717 } }){Location = PrintLocation.Right});
            mus.AddEntry(new LeaderEntry("Omar II", new int?[,] { { 717, 720 } }) { StartValue = 719 });
            mus.AddEntry(new LeaderEntry("Yezid II", new int?[,] { { 720, 724 } }) { StartValue = 722 });
            mus.AddEntry(new LeaderEntry("Hisham", new int?[,] { { 724, 743 } }) { StartValue = 725 });
            mus.AddEntry(new LeaderEntry("Walid II", new int?[,] { { 743, 744 } }));
            mus.AddEntry(new LeaderEntry("Merwan II", new int?[,] { { 744, 750 } }) { StartValue = 747 });
            mus.AddEntry(new LeaderEntry("Abu al-Abbas", new int?[,] { { 750, 754 } }) { StartValue = 751 });
            mus.AddEntry(new LeaderEntry("Abu Jafar", new int?[,] { { 754, 775 } }) { StartValue = 756 });
            mus.AddEntry(new LeaderEntry("al-Mahdi", new int?[,] { { 775, 784 } }));
            mus.AddEntry(new LeaderEntry("Abbasid", new int?[,] { { 784, 786 } }));
            mus.AddEntry(new LeaderEntry("Harun al-Rashid", new int?[,] { { 786, 809 } }) { StartValue = 787 });
            mus.AddEntry(new ScienceAdvEntry("windmill", "") { StartValue = 650 });
            mus.AddEntry(new ScienceAdvEntry("algebra","") {StartValue = 820});
            mus.AddEntry(new LiteraryWorkEntry("Quran", "Mohammed") { StartValue = 630, Location = PrintLocation.Left });
            mus.AddEntry(new ScienceAdvEntry("zero", "Brahmagupta") { StartValue = 630, Location = PrintLocation.Right });

            mus.AddEntry(738, "Battle of Rajasthan(738) [India]");

            mus.AddEntry(620, 661, "Rashidun Caliphate", PrintLocation.Right);
            mus.AddEntry(683, 750, "Umayyad Caliphate", PrintLocation.Right);
            mus.AddEntry(750, 825, "Abbasid Caliphate", PrintLocation.Right);
            mus.AddEntry(775,825, "\"Islamic golden age\"");


            var sas = new Block {Ruler = rule, Title = "Sasanian Empire", EndValue = 642, Width = 30};
            sas.AddEntry(new LeaderEntry("Khosru II", new int?[,] { { 590, 628 } }) {StartValue = 596 });

            var plate = new Plate {Ruler = rule, Name = "Early Middle Age"};
            plate.AddBlock(euro);
            plate.AddBlock(byz);
            plate.AddBlock(mus);
            plate.AddBlock(sas);
            plate.FileName = "11_CE550_825";

            //plate.AddArrow(new Arrow(sas, byz) {StartValue = 627, Text = "seige Constantinople(626)"});
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
