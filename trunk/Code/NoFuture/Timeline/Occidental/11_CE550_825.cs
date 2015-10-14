namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE550to825()
        {
            var rule = new Rule {StartValue = 550, EndValue = 825, RuleLineSpacing = 7};

            var byz = new Block {Ruler = rule, Title = "Byzantine Empire", Width = 36};
            byz.AddEntry(582,"Maurice(582-602)");
            byz.AddEntry(602,"Phocas(602-610)");
            byz.AddEntry(610,"Heraclius(610-641)");
            byz.AddEntry(680, "Sixth Ecumen. Council(680)");
            byz.AddEntry(692,717,"Twenty Years Anarchy(695-717)",PrintLocation.Left);
            byz.AddEntry(725,"Leo III(717-741)");
            byz.AddEntry(746,"Constantine V(741-775)");
            byz.AddEntry(741,"Artabasdos ursurpation(741-743)");
            byz.AddEntry(775,"Leo IV(775-780)");
            byz.AddEntry(780,"Constantine VI(780-797)");
            byz.AddEntry(797,"Irene [empress](797-802)");
            byz.AddEntry(787,"2nd Council of Nicaea(787)[end Iconoclasm]");
            byz.AddEntry(719, 802, "Isaurian dyn(717-802)", PrintLocation.Right);

            var euro = new Block() {Ruler = rule, Title = "West Europe", Width = 54};
            euro.AddEntry(577, "Battle of Deorham(577) - Anglo-Saxon defeat Britons");
            euro.AddEntry(589, "'filioque' intro'ed into Latin west");
            euro.AddEntry(600,"Pope Gregory(590-604)");
            euro.AddEntry(561, 614, "Gaul decends, total illiteracy",PrintLocation.Left);
            euro.AddEntry(675, "Beowulf composed(?)");
            euro.AddEntry(700, "Venerable Bede(673-735)");
            euro.AddEntry(741, "Pope Zachary(741-752)");
            euro.AddEntry(751, "Pepin the Short(751-768)", PrintLocation.Left);
            euro.AddEntry(752, "Pope Stephen II(752-757)", PrintLocation.Right);
            euro.AddEntry(756, "Donation of Pepin(754)");
            euro.AddEntry(800, "Charlemagne-Holy Roman Emperor(800)");
            euro.AddEntry(805, "Halfdan (the Black) first king of Norway(800)");
            euro.AddEntry(793,"1st Viking raid [England](793)");

            var mus = new Block {Ruler = rule, StartValue = 565, Title = "Muslims", Width = 56};
            mus.AddEntry(569, 632, "Mohammed (569-632)", PrintLocation.Left);
            mus.AddEntry(610, "vision @ Mt. Hira(610)");
            mus.AddEntry(630, "Mohammad takes Mecca(630)");
            mus.AddEntry(633, "Abu Bekr(633-634) & Omar(634-644)");
            mus.AddEntry(644, "Othman(644-656)", PrintLocation.Left);
            mus.AddEntry(656, "war of succesion Ali/Muawiya(656-669)");
            mus.AddEntry(669, "Muawiya(669-680)", PrintLocation.Left);
            mus.AddEntry(679, "war of succesion(680-683)");
            mus.AddEntry(685, "Abd-al-Malik(683-705)", PrintLocation.Left);
            mus.AddEntry(705, "Walid(705-715)", PrintLocation.Left);
            mus.AddEntry(715, "Suleiman(715-717)");
            mus.AddEntry(719, "Omar II(717-720)", PrintLocation.Left);
            mus.AddEntry(722, "Yezid II(720-24)", PrintLocation.Left);
            mus.AddEntry(725, "Hisham(724-743)", PrintLocation.Left);
            mus.AddEntry(743, "Walid II(743/44)", PrintLocation.Left);
            mus.AddEntry(747, "Merwan II(744-750)", PrintLocation.Left);
            mus.AddEntry(751, "Abu al-Abbas(750-754)", PrintLocation.Left);
            mus.AddEntry(756, "Abu Jafar(754-775)", PrintLocation.Left);
            mus.AddEntry(775, "al-Mahdi(775-784)", PrintLocation.Left);
            mus.AddEntry(784, "Abbasid(784-786)", PrintLocation.Left);
            mus.AddEntry(787, "Harun al-Rashid(786-809)", PrintLocation.Left);

            mus.AddEntry(738, "Battle of Rajasthan(738) [India]");
            
            mus.AddEntry(620,661,"Rashidun Caliphate",PrintLocation.Right);
            mus.AddEntry(683, 750, "Umayyad Caliphate", PrintLocation.Right);
            mus.AddEntry(750, 825, "Abbasid Caliphate", PrintLocation.Right);
            

            var sas = new Block {Ruler = rule, Title = "Sasanian Empire", EndValue = 642, Width = 30};
            sas.AddEntry(596,"Khosru II(590-628)");

            var plate = new Plate { Ruler = rule, Name = "The Dark Ages" };
            plate.AddBlock(euro);
            plate.AddBlock(byz);
            plate.AddBlock(mus);
            plate.AddBlock(sas);

            plate.AddArrow(new Arrow(sas,byz){StartValue = 627, Text = "seige Constantinople(626)"});
            plate.AddArrow(new Arrow(sas,byz){StartValue = 614, Text = "capture Jerusalem(614)"});
            plate.AddArrow(new Arrow(byz, sas){StartValue = 624, Text = "counter attack(624)"});
            plate.AddArrow(new Arrow(mus,sas){StartValue = 642, Text = "battle of Nahavand(642)"});
            plate.AddArrow(new Arrow(mus,byz) {StartValue = 634, Text = "siege of Damascus(634)"});
            plate.AddArrow(new Arrow(mus, byz) { StartValue = 674, Text = "seige Constantinople(674)" });
            plate.AddArrow(new Arrow(mus, byz) { StartValue = 717, Text = "seige Constantinople(717)" });
            plate.AddArrow(new Arrow(mus, euro){StartValue = 711, Text = "Moors invade Spain(711)"});
            plate.AddArrow(new Arrow(mus, byz){StartValue = 698, Text = "Battle of Carthage(698)"});
            plate.AddArrow(new Arrow(mus, euro){StartValue = 732, Text = "Battle of Tours(732)"});

            plate.Notes.Add("Western Christendom was a beleaguered citadel which only survived because its greatest enemy, Islam, had");
            plate.Notes.Add("reached the end of its lines of communication, and its lesser enemies (the Slavs, the Hungarians, and the Vikings)");
            plate.Notes.Add("were organized only for raids and for plunder.");
            return plate;
        }
    }
}
