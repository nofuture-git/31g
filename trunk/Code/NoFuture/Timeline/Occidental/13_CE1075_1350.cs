using System;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1075to1350()
        {
            var rule = new Rule { StartValue = 1075, EndValue = 1350, RuleLineSpacing = 7 };

            var engd = new Block { Ruler = rule, Title = "Kingdom of England", Width = 35 };

            engd.AddEntry(1087, "William II(1085-1100)", PrintLocation.Left);
            engd.AddEntry(1100, "Henry I(1100-1135)", PrintLocation.Left);
            engd.AddEntry(1135, 1150, "The Anarchy(1135-1154)");
            engd.AddEntry(1159, "Henry II(1154-1189)", PrintLocation.Left);
            engd.AddEntry(1189, "Richard I(1189-1199)", PrintLocation.Left);
            engd.AddEntry(1199, "John(1199-1216)", PrintLocation.Left);
            engd.AddEntry(1219, "Magna Carta(1215)",PrintLocation.Right);
            engd.AddEntry(1222, "Henry III(1216-1272)", PrintLocation.Left);
            engd.AddEntry(1272, "Edward I(1272-1307)", PrintLocation.Left);
            engd.AddEntry(1300, "[Sentences(Duns Scotus)]",PrintLocation.Right);
            engd.AddEntry(1307, "Edward II(1307-1327)", PrintLocation.Left);
            engd.AddEntry(1327, "Edward III(1327-1377)", PrintLocation.Left);
            engd.AddEntry(1334, 1350, "2nd War Scottish Indep.(1332-1357)");
            engd.AddEntry(1296, 1328, "1st War Scottish Indep.(1296-1328)");
            engd.AddEntry(1337, 1350, "Hundred Years' War(1337-1415)");

            engd.AddEntry(1077, 1135, "Norman dyn.", PrintLocation.Left);
            engd.AddEntry(1154, 1214, "Avgevin dyn.", PrintLocation.Left);
            engd.AddEntry(1219, 1350, "Plantagenet dyn.", PrintLocation.Left);

            var hre = new Block { Ruler = rule, Title = "Holy Roman Empire"};
            hre.AddEntry(1084,"Henry IV(1084-1105)", PrintLocation.Left);
            hre.AddEntry(1111, "Henry V(1111-1125)", PrintLocation.Left);
            hre.AddEntry(1133, "Lothair III(1133-1137)", PrintLocation.Left);
            hre.AddEntry(1155, "Frederick I(1155-1190)", PrintLocation.Left);
            hre.AddEntry(1191, "Henry VI(1191-1197)", PrintLocation.Left);
            hre.AddEntry(1209, "Otto IV(1209-1215)",PrintLocation.Left);
            hre.AddEntry(1220, "Frederick II(1220-1250)", PrintLocation.Left);
            hre.AddEntry(1247, "Interregnum(1254-1273)");
            hre.AddEntry(1312, "Henry VII(1312,3)", PrintLocation.Left);
            hre.AddEntry(1328, "Louis IV(1328-1347)", PrintLocation.Left);
            hre.AddEntry(1331, "William of Ockham", PrintLocation.Right);

            hre.AddEntry(1078, 1125, "Salian dyn.", PrintLocation.Left);
            hre.AddEntry(1128, 1137, "Supplinburg dyn.", PrintLocation.Left);
            hre.AddEntry(1152, 1197, "Staufen dyn.", PrintLocation.Left);
            hre.AddEntry(1215, 1250, "Staufen dyn.(again)", PrintLocation.Left);

            var byz = new Block { Ruler = rule, Title = "Byzantine Empire"};
            byz.AddEntry(1081, "Alexios I(1081-1118)", PrintLocation.Left);
            byz.AddEntry(1118, "John II(1118-1143)", PrintLocation.Left);
            byz.AddEntry(1150, "Manuel I(1143-1180)", PrintLocation.Left);
            byz.AddEntry(1180, "Alexios II(1180-1183)", PrintLocation.Left);
            byz.AddEntry(1182, "Andronikos I(1183-1185)", PrintLocation.Left);
            byz.AddEntry(1091, "Isaac II(1185-1195)", PrintLocation.Left);
            byz.AddEntry(1195, "Alexios III(1195-1203)", PrintLocation.Left);
            byz.AddEntry(1200, "Alexios IV & Isaac II(1203,4)", PrintLocation.Left);
            byz.AddEntry(1203, "Alexios V(1204)", PrintLocation.Right);
            byz.AddEntry(1208, "Theodore I(1205-1222)", PrintLocation.Left);
            byz.AddEntry(1222, "John III(1222-1254)", PrintLocation.Left);
            byz.AddEntry(1254, "Theodore II(1254-1258)", PrintLocation.Left);
            byz.AddEntry(1258, "John IV(1258-1261)", PrintLocation.Left);
            byz.AddEntry(1268, "Michael VII(1259-1282)", PrintLocation.Left);
            byz.AddEntry(1282, "Andronikos II(1282-1328)", PrintLocation.Left);
            byz.AddEntry(1328, "Andronikos III(1328-1341)", PrintLocation.Left);
            byz.AddEntry(1341, 1347, "Civil War");


            byz.AddEntry(1079, 1183, "Komnenid dyn.", PrintLocation.Left);
            byz.AddEntry(1188, 1203, "Angelid dyn.", PrintLocation.Left);
            byz.AddEntry(1205, 1259, "Laskarid dyn.", PrintLocation.Left);
            byz.AddEntry(1265, 1350, "Palaiologan dyn.", PrintLocation.Left);


            var slj = new Block { Ruler = rule, Title = "Rum Seljuks", EndValue = 1307};
            slj.AddEntry(1077, "Suleyman(1077-1082)", PrintLocation.Left);
            slj.AddEntry(1092, "Kilij Arslan(1092-1107)", PrintLocation.Left);
            slj.AddEntry(1110, "Malik Shah(1110-1116)", PrintLocation.Left);
            slj.AddEntry(1116, "Masud I(1116-1156)", PrintLocation.Left);
            slj.AddEntry(1156, "Kilij Arslan II(1156-1192)", PrintLocation.Left);
            slj.AddEntry(1194, 1202, "wars of Ascension");
            slj.AddEntry(1205, "Kaykhusraw I(1205-1211)", PrintLocation.Left);
            slj.AddEntry(1211, "Kaykaus I(1211-1220)", PrintLocation.Left);
            slj.AddEntry(1220, "Kayqubad I(1220-1237)", PrintLocation.Left);
            slj.AddEntry(1237, "Kaykhusraw II(1237-1246)", PrintLocation.Left);
            slj.AddEntry(1252,1304,"Mongol admin/intrl dicord");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire", StartValue = 1299};
            ott.AddEntry(1303, "Osman I(1299-1326)");
            ott.AddEntry(1328, "Orhan(1327-1359)");

            var mong = new Block {Ruler = rule, Title = "Mongols", StartValue = 1200, Width = 38};
            mong.AddEntry(1206,"Genghis(1206-1227)",PrintLocation.Left);
            mong.AddEntry(1215, "Battle of Zhongdu(1215)[N. China]");
            mong.AddEntry(1225,"Battle of Kalka R.(1223)[Ukraine][+]");
            mong.AddEntry(1229, "Ogedei(1229-1241)", PrintLocation.Left);
            mong.AddEntry(1233, "Invade Korea(1236)");
            mong.AddEntry(1238, "Battle of Mohi(1241)[Hungary]");
            mong.AddEntry(1241, "Battle of Legnica(1241)[Poland]");
            mong.AddEntry(1248, "Guyuk(1246-1248)", PrintLocation.Left);
            mong.AddEntry(1251, "Mongke(1251-1259)", PrintLocation.Left);
            mong.AddEntry(1265,"sack Hanio(1257)[Vietnam]");
            mong.AddEntry(1260, "Kublai(1260-1294)", PrintLocation.Left);
            mong.AddEntry(1274,"Battle of Bun'ei(1274)[Japan]");
            mong.AddEntry(1281,"Battle of Koan(1281)[Japan]");
            mong.AddEntry(1333, "Toghan Temur(1333-1370)", PrintLocation.Left);

            var ftd = new Block { Ruler = rule, Title = "Fatimid Caliphate", EndValue = 1171};
            ftd.AddEntry(1078, "Abul-Qasim(1094-1101)", PrintLocation.Left);
            ftd.AddEntry(1101, "Abu Ali(1101-1130)", PrintLocation.Left);
            ftd.AddEntry(1130, "Abul-Maymun(1130-1149)", PrintLocation.Left);
            ftd.AddEntry(1149, "Abu Mansur(1149-1154)", PrintLocation.Left);
            ftd.AddEntry(1154, "Abul-Qasim(1154-1160)", PrintLocation.Left);
            ftd.AddEntry(1160, "Abu Muhammad(1160-1171)", PrintLocation.Left);

            var ayy = new Block {Ruler = rule, Title = "Ayyubid Sultanates", StartValue = 1171, EndValue = 1260};
            ayy.AddEntry(1174,"Saladin(1174-1193)", PrintLocation.Left);
            ayy.AddEntry(1194, 1240, "wars of Ascension");

            var mam = new Block {Ruler = rule, Title = "Mamluk Sultanate", StartValue = 1250};
            mam.AddEntry(1254,1350,"Bahri dyn.",PrintLocation.Left);

            var jrs = new Block {Ruler = rule, Title = "Crusader States", StartValue = 1090, EndValue = 1295};
            jrs.AddEntry(1100,1149,"County of Edessa",PrintLocation.Left);
            jrs.AddEntry(1103,1268,"Principality of Antioch",PrintLocation.Right);
            jrs.AddEntry(1106,1288,"Kingdom of Jerusalem");
            jrs.AddEntry(1291, "Fall of Acre(1291)");

            var scdv = new Block {Ruler = rule, Title = "Scandinavia"};

            var fr = new Block { Ruler = rule, Title = "Kingdom of France", Width = 28};
            fr.AddEntry(1096, "Phillip I(1052-1108)", PrintLocation.Left);
            fr.AddEntry(1108, "Louis VI(1108-1137)", PrintLocation.Left);
            fr.AddEntry(1137, "Louis VII(1137-1180)", PrintLocation.Left);
            fr.AddEntry(1167, "[H] Cathars (duality)");
            fr.AddEntry(1180, "Phillip II(1180-1223)", PrintLocation.Left);
            fr.AddEntry(1209, "Cathar Crusade(1209-1229)");
            fr.AddEntry(1223, "Louis VII(1223-1226)", PrintLocation.Left);
            fr.AddEntry(1226, "Louis IX(1226-1270)", PrintLocation.Left);
            fr.AddEntry(1270, "Philip III(1270-1285)", PrintLocation.Left);
            fr.AddEntry(1285, "Philip IV(1285-1314)", PrintLocation.Left);
            fr.AddEntry(1314, "Louis X(1314-1316)", PrintLocation.Left);
            fr.AddEntry(1317, "The Divine Comedy(1320)");
            fr.AddEntry(1320, "Philip V(1316-1322)", PrintLocation.Left);
            fr.AddEntry(1325, "Charles IV(1322-1328)", PrintLocation.Left);
            fr.AddEntry(1328, "Philip VI(1328-1350)", PrintLocation.Left);
            fr.AddEntry(1240,"Thomas Aquinas(1225-1274)",PrintLocation.Right);
            fr.AddEntry(1079, 1350, "Capetian dyn.", PrintLocation.Left);
            fr.AddEntry(1163, 1340, "Notre Dame ctor(1163-1345)", PrintLocation.Right);
            fr.AddEntry(1309, 1350, "Avignon Papacy(1307-1377)");

            var plate = new Plate { Ruler = rule, Name = "High Middle Ages" };
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(hre);
            plate.AddBlock(byz);
            plate.AddBlock(jrs);
            plate.AddBlock(slj);
            plate.AddBlock(ott);
            plate.AddBlock(ftd);
            plate.AddBlock(ayy);
            plate.AddBlock(mong);
            plate.AddBlock(mam);

            plate.AddArrow(new Arrow(fr, slj) {StartValue = 1096, Text = "First Crusade(1096-1099)", ArrowTail = "...", FromLeftToRightArrowHead = ".....>"});
            plate.AddArrow(new Arrow(slj, byz){StartValue = 1176, Text = "Battle of Myriokephalon(1176)"});
            plate.AddArrow(new Arrow(fr, slj)
            {
                StartValue = 1145,
                Text = "Second Crusade(1145-1149)",
                ArrowTail = "...",
                FromLeftToRightArrowHead = ".....>"
            });
            plate.AddArrow(new Arrow(engd, slj) { StartValue = 1189, Text = "Third Crusade(1189-1192)", ArrowTail = "..", FromLeftToRightArrowHead = ".....>" });
            plate.AddArrow(new Arrow(fr, engd) {StartValue = 1214, Text = "Battle of Bouvines(1214)**"});
            plate.AddArrow(new Arrow(mam, engd)
            {
                StartValue = 1314,
                Text = "Medieval Warm Period ends***",
                ArrowTail = "............]",
                FromRightToLeftArrowHead = "[......"
            });
            plate.AddArrow(new Arrow(mam, engd)
            {
                StartValue = 1346,
                Text = "Black Death (1346)****",
                ArrowTail = "............]",
                FromRightToLeftArrowHead = "[......"
            });
            plate.AddArrow(new Arrow(ayy, jrs) {StartValue = 1187, Text = "Battle of Hattin(1187)*"});
            plate.AddArrow(new Arrow(fr, byz) { StartValue = 1202, Text = "Fourth Crusade(1202-1204)", FromLeftToRightArrowHead = ".....>" });
            plate.AddArrow(new Arrow(hre, ayy) {StartValue = 1213, Text = "Fifth Crusade(1213-1221)"});
            plate.AddArrow(new Arrow(hre, ayy) { StartValue = 1228, Text = "Sixth Crusade(1228)", FromLeftToRightArrowHead = ".......>" });
            plate.AddArrow(new Arrow(mong, slj){StartValue = 1243, Text = "Battle of Kose Dag(1243)"});
            plate.AddArrow(new Arrow(jrs, ayy) {StartValue = 1248, Text = "Battle of Harbiyah(1244)"});
            plate.AddArrow(new Arrow(mong, ayy) {StartValue = 1258, Text = "Siege of Baghdad(1258)"});
            plate.AddArrow(new Arrow(mam, mong) {StartValue = 1258, Text = "Battle of Ain Jalut(1260)[a]"});
            plate.AddArrow(new Arrow(mam, mong) { StartValue = 1299, Text = "B. of Wadi al-Khazandar(1299) " });
            plate.AddArrow(new Arrow(mam, mong) { StartValue = 1303, Text = "B. of Marj al-Saffar(1303)" });
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1324, Text = "capture of Bursa(1326)"});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1333, Text = "capture Nicaea(1333)"});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1337, Text = "capture Nicomedia(1337)"});

            plate.Notes.Add("* Jerusalem is retaken by Muslims");
            plate.Notes.Add(
                "** John of England was weakened thereby submitting to sign the Magna Carta while Phillip centralized monarchy ruled until French Rev.");
            plate.Notes.Add("*** This causes wide spread famine and population decline.");
            plate.Notes.Add(
                "**** By the end of this period the population of the west is cut by 1/2th of what it was prior.");
            plate.Notes.Add("[+] Republic of Vience had struck a deal with the Mongols to destroy any other European trading posts.");
            plate.Notes.Add("[a] first time Mongols ever permanently beaten back in direct combat - Mongols never reach the Nile.");
            plate.Notes.Add("Sultan : sovereign;  Emir : local cheiften; Caliph : successor to Muhammad");
            return plate;
        }
    }
}
