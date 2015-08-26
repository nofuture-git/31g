using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE825to1075()
        {
            var rule = new Rule { StartValue = 825, EndValue = 1075, RuleLineSpacing = 7 };

            var scdv = new Block { Ruler = rule, Title = "Scandinavia", Width = 40 };
            scdv.AddEntry(830,"Vikings settle Iceland(825)");
            scdv.AddEntry(872, "Harald Fairhair[Norway(872-930)]",PrintLocation.Left);
            scdv.AddEntry(960, "Harold Bluetooth[Denmark(985-986)", PrintLocation.Left);
            scdv.AddEntry(963, ",Norway(970-986)]", PrintLocation.Left);
            scdv.AddEntry(995, "Olaf Tryggvason[Norway(995-1000)]", PrintLocation.Left);
            scdv.AddEntry(986, "Sweyn Forkbeard[Denmark(986-1014)", PrintLocation.Left);
            scdv.AddEntry(989, ",Norway(986-995 & 999-1014)", PrintLocation.Left);
            scdv.AddEntry(992, ",England(1013/14)]", PrintLocation.Left);
            scdv.AddEntry(1011, "Olaf II Haraldsson[Norway(1015-1028)]", PrintLocation.Left);
            scdv.AddEntry(1018, "Cnut the Great[Denmark(1018-1035)", PrintLocation.Left);
            scdv.AddEntry(1021, ",Norway(1028-1035)", PrintLocation.Left);
            scdv.AddEntry(1025, ",England(1016-1035)]", PrintLocation.Left);
            scdv.AddEntry(1046, "Harald Sigurdsson[Norway(1046-1066)]***", PrintLocation.Left);
            scdv.AddEntry(1000,"Leif Erikson in N.America(1000)");

            var engd = new Block { Ruler = rule, Title = "English Isles", Width = 34};
            engd.AddEntry(853, "Vikings found Dublin(853)");
            engd.AddEntry(871, "Alfred the Great(871-899)");
            engd.AddEntry(1009,"AEthelred(1013-1014)");
            engd.AddEntry(1042,"Edward the Confessor(1042-1066)");

            var hre = new Block {Ruler = rule, Title = "Holy Roman Empire", Width = 48};
            hre.AddEntry(831, "Archdiocese of Hamburg founded(831)");
            hre.AddEntry(835,"Louis I(814-840)");
            hre.AddEntry(843,"Treaty of Verdun(843)");
            hre.AddEntry(846,"Lothair I(Italy);Louis(Germany);Charles(France)");

            hre.AddEntry(911, "Rollo[Normandy(911-927)]", PrintLocation.Left);
            hre.AddEntry(927, "William I[Normandy(927-942)]", PrintLocation.Left);
            hre.AddEntry(936, "Otto I[Germany(962-973)]", PrintLocation.Right);
            hre.AddEntry(942, "Richard I[Normandy(942-966)]", PrintLocation.Left);
            hre.AddEntry(963, "Otto has Pope John XII deposed(963)");
            hre.AddEntry(967,"Otto has Pope John XIII restored(966)");
            hre.AddEntry(972,"Otto's son married to Byz. princess(972)");
            hre.AddEntry(975, "Otto II[Germany(973-983)]", PrintLocation.Right);
            hre.AddEntry(996, "Richard II[Normandy(996-1026)]", PrintLocation.Left);
            hre.AddEntry(999, "Otto III[Germany(996-1002)]", PrintLocation.Right);
            hre.AddEntry(1027, "Robert I[Normandy(1027-1035)]", PrintLocation.Left);
            hre.AddEntry(1035, "William III [Normandy(1035-1087)", PrintLocation.Left);
            hre.AddEntry(1038, ",England(1066-1087)]", PrintLocation.Left);
            
            var byz = new Block { Ruler = rule, Title = "Byzantine Empire",Width = 32};
            byz.AddEntry(842,"Michael III(842-867)");
            byz.AddEntry(867, "Basil I(867-886)");
            byz.AddEntry(886,"Leo VI(886-912)");
            byz.AddEntry(913,"Constantine VII(913-959)");
            byz.AddEntry(960, "retake Island of Crete(960)");
            byz.AddEntry(963,"Nikephoros II Phokas(963-969)");
            byz.AddEntry(976,"Tzimisces(969-976)");
            byz.AddEntry(980,"Basil II(976-1025)");
            byz.AddEntry(1025,"Constantine VIII(1025-1028)");
            byz.AddEntry(1030, "Zoe(1028-1050)");
            byz.AddEntry(1042, "Constantine IX(1042-1055)");
            byz.AddEntry(1045, "rebuild Holy Sepulchre(1042)");
            byz.AddEntry(1059, "Constantine X(1059-1067)");
            byz.AddEntry(1068,"Romanos IV(1067-1071)");

            var abb = new Block { Ruler = rule, Title = "Abbasid Caliphate", EndValue = 1055, Width = 30};
            abb.AddEntry(861,870,"Anarchy at Samarra(861-870)");
            abb.AddEntry(833, "Al-Mu'tasim(833-842)");
            abb.AddEntry(842, "Al-Wathiq(842-847)");
            abb.AddEntry(847, "Al-Mutawakkil(847-861)");
            abb.AddEntry(874,"Al-Mu'tamid(870-892)");
            abb.AddEntry(892,"Al-Mu'tadid(892-902)");
            abb.AddEntry(902,"Al-Muktafi(902-908)");
            abb.AddEntry(908,"Al-Muqtadir(908-932)");
            abb.AddEntry(932,"Al-Qahir(932-934)");
            abb.AddEntry(935,"Al-Radi(934,940)");
            abb.AddEntry(940,"Al-Muttaqi(904-944)");
            abb.AddEntry(944,"Al-Mustakfi(944-946)");
            abb.AddEntry(947,"Al-Muti(946-974)");
            abb.AddEntry(974,"Al-Ta'i(974-991)");
            abb.AddEntry(991,"Al-Qadir(991-1031)");
            abb.AddEntry(1031,"Al-Qa'im(1031-1075)");

            var ftd = new Block {Ruler = rule, Title = "Fatimid Caliphate", StartValue = 909, Width = 32};
            ftd.AddEntry(972, "Cario est.(969)");
            ftd.AddEntry(996,"Al-Hakim bi-Amr(996-1021)");
            ftd.AddEntry(1009, "destroys Holy Sepulchre(1009)");
            ftd.AddEntry(1021,"Ali az-Zahir(1021-1036)");
            ftd.AddEntry(1036, "Al-Mustansir(1036-1094)");
            ftd.AddEntry(1065, "famine(1065 & 1072)");

            var slj = new Block {Ruler = rule, Title = "Seljuk Empire", StartValue = 1037};
            slj.AddEntry(1040,"Tughril(1037-1063)");
            slj.AddEntry(1063,"Alp Arslan(1063-1072)");

            var plate = new Plate { Ruler = rule, Name = "Viking Age" };
            plate.AddBlock(scdv);
            plate.AddBlock(engd);
            plate.AddBlock(hre);
            plate.AddBlock(ftd);
            plate.AddBlock(byz);
            plate.AddBlock(slj);
            plate.AddBlock(abb);

            plate.AddArrow(new Arrow(scdv, engd) { StartValue = 830, Text = "raid Ireland(812-853)" });
            plate.AddArrow(new Arrow(scdv, engd) { StartValue = 839, Text = "end Pict kingdom(839)" });
            plate.AddArrow(new Arrow(scdv, engd) { StartValue = 878, Text = "Battle of Edington(878)" });
            plate.AddArrow(new Arrow(scdv,hre){StartValue = 908,Text = "settle Normandy(911)"});
            plate.AddArrow(new Arrow(hre,scdv){StartValue = 950, Text = "Otto I defeat Bluetooth(950)"});
            plate.AddArrow(new Arrow(hre, byz){StartValue = 968,Text = "Otto seize S.Italy(968)"});
            plate.AddArrow(new Arrow(ftd,abb){StartValue = 972, Text ="seize Egypt(969)"});
            plate.AddArrow(new Arrow(scdv,engd){StartValue = 1013, Text = "Danes England(1013)"});
            plate.AddArrow(new Arrow(hre, ftd) { StartValue = 1070, Text = "R.Guiscard Sicily(1070)" });
            plate.AddArrow(new Arrow(hre,engd){StartValue = 1066,Text = "Norman conquest(1066)"});
            plate.AddArrow(new Arrow(hre,ftd){StartValue = 982,Text = "Battle Cape Colonna(982)"});
            plate.AddArrow(new Arrow(slj,abb){StartValue = 1055, Text = "capture Baghdad(1055)"});
            plate.AddArrow(new Arrow(slj, byz){StartValue = 1071,Text = "Battle of Manzikert(1071)**"});

            plate.Notes.Add("* this guy was all over the place, fights alongside his brother Olaf Haraldsson, a.k.a. Saint Olaf, who");
            plate.Notes.Add("  sought to reclaim the Danish throne from Cnut the Great - is defeated, goes into exile to the Varangian (Viking) kingdom around Novgorod (Russia) working");
            plate.Notes.Add("  with Yaroslav the Wise who unites Keiv and Novgorod then moves to Constantinople (1034) becomes captain of Byzantine Varangian Guard (Viking bodyguards to the Emperor) gets rich,");
            plate.Notes.Add("  leaves in 1042 and becomes King of Norway 1045, was caught up in war of succession in England, goes there to become king and is killed at the Battle of Stamford Bridge(1066)");
            plate.Notes.Add("  by Harold Godwinson's forces (which is considered the end of the Viking Age).  Even more, it is Harold Godwinson who is defeated by the Norman Conquest weeks later at the Battle of Hastings(1066).");
            plate.Notes.Add("");
            plate.Notes.Add("** during this war Palestine is taken from Fatimid Cali.- later catalyst for Crusades");
            
            return plate;
        }
    }
}
