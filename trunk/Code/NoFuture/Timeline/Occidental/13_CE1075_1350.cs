namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1075to1350()
        {
            var rule = new Rule {StartValue = 1075, EndValue = 1350, RuleLineSpacing = 7};

            var engd = new Block {Ruler = rule, Title = "Kingdom of England", Width = 35};

            engd.AddEntry(new LeaderEntry("William II", new int?[,] { { 1085, 1100 } }) {StartValue = 1087, Location =  PrintLocation.Left});
            engd.AddEntry(new LeaderEntry("Henry I", new int?[,] { { 1100, 1135 } }) { StartValue = 1100, Location = PrintLocation.Left });
            engd.AddEntry(1135, 1150, "The Anarchy(1135-1154)");
            engd.AddEntry(new LeaderEntry("Henry II", new int?[,] { { 1154, 1189 } }) { StartValue = 1159, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Richard I", new int?[,] { { 1189, 1199 } }) { StartValue = 1189, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("John", new int?[,] { { 1199, 1216 } }) { StartValue = 1199, Location = PrintLocation.Left });
            engd.AddEntry(1219, "Magna Carta(1215)", PrintLocation.Right);
            engd.AddEntry(new LeaderEntry("Henry III", new int?[,] { { 1216, 1272 } }) { StartValue = 1222, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Edward I", new int?[,] { { 1272, 1307 } }) { StartValue = 1272, Location = PrintLocation.Left });
            engd.AddEntry(new LiteraryWorkEntry("Sentences", "Duns Scotus") {StartValue = 1300, Location = PrintLocation.Right});
            engd.AddEntry(new LeaderEntry("Edward II", new int?[,] { { 1307, 1327 } }) { StartValue = 1307, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Edward III", new int?[,] { { 1327, 1377 } }) { StartValue = 1327, Location = PrintLocation.Left });
            engd.AddEntry(1334, 1350, "2nd War Scottish Indep.(1332-1357)");
            engd.AddEntry(1296, 1328, "1st War Scottish Indep.(1296-1328)");
            engd.AddEntry(1337, 1350, "Hundred Years' War(1337-1415)");

            engd.AddEntry(1077, 1135, "Norman dyn.", PrintLocation.Left);
            engd.AddEntry(1154, 1214, "Avgevin dyn.", PrintLocation.Left);
            engd.AddEntry(1219, 1350, "Plantagenet dyn.", PrintLocation.Left);

            var hre = new Block {Ruler = rule, Title = "Holy Roman Empire"};
            hre.AddEntry(new LeaderEntry("Henry IV", new int?[,] { { 1084, 1105 } }) {StartValue = 1084, Location = PrintLocation.Left});
            hre.AddEntry(new LeaderEntry("Henry V", new int?[,] { { 1111, 1125 } }) { StartValue = 1111, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Lothair III", new int?[,] { { 1133, 1137 } }) { StartValue = 1133, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Frederick I", new int?[,] { { 1155, 1190 } }) { StartValue = 1155, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Henry VI", new int?[,] { { 1191, 1197 } }) { StartValue = 1191, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Otto IV", new int?[,] { { 1209, 1215 } }) { StartValue = 1209, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Frederick II", new int?[,] { { 1220, 1250 } }) { StartValue = 1220, Location = PrintLocation.Left });
            hre.AddEntry(1247, "Interregnum(1254-1273)");
            hre.AddEntry(new LeaderEntry("Henry VII", new int?[,] { { 1312, 1313 } }) { StartValue = 1312, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Louis IV", new int?[,] { { 1328, 1347 } }) { StartValue = 1328, Location = PrintLocation.Left });
            hre.AddEntry(new PhilosopherEntry("William of Ockham", 1285,1347) {StartValue = 1336, Location = PrintLocation.Right });
            

            hre.AddEntry(1078, 1125, "Salian dyn.", PrintLocation.Left);
            hre.AddEntry(1128, 1137, "Supplinburg dyn.", PrintLocation.Left);
            hre.AddEntry(1152, 1197, "Staufen dyn.", PrintLocation.Left);
            hre.AddEntry(1215, 1250, "Staufen dyn.(again)", PrintLocation.Left);

            var byz = new Block {Ruler = rule, Title = "Byzantine Empire"};
            byz.AddEntry(new LeaderEntry("Alexios I", new int?[,] { { 1081, 1118 } }) {StartValue = 1081, Location = PrintLocation.Left});
            byz.AddEntry(new LeaderEntry("John II", new int?[,] { { 1118, 1143 } }) { StartValue = 1118, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Manuel I", new int?[,] { { 1143, 1180 } }) { StartValue = 1150, Location = PrintLocation.Left });
            byz.AddEntry(1180, "Latins Massacred", PrintLocation.Right);
            byz.AddEntry(new LeaderEntry("Alexios II", new int?[,] { { 1180, 1183 } }) { StartValue = 1176, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Andronikos I", new int?[,] { { 1183, 1185 } }) { StartValue = 1182, Location = PrintLocation.Center });
            byz.AddEntry(new LeaderEntry("Isaac II", new int?[,] { { 1185, 1195 } }) { StartValue = 1091, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Alexios III", new int?[,] { { 1195, 1203 } }) { StartValue = 1195, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Alexios IV & Isaac II", new int?[,] { { 1203, 1204 } }) { StartValue = 1200, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Theodore I", new int?[,] { { 1205, 1222 } }) { StartValue = 1218, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("John III", new int?[,] { { 1222, 1254 } }) { StartValue = 1240, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Theodore II", new int?[,] { { 1254, 1258 } }) { StartValue = 1254, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("John IV", new int?[,] { { 1258, 1261 } }) { StartValue = 1258, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Michael VII", new int?[,] { { 1259, 1282 } }) { StartValue = 1268, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Andronikos II", new int?[,] { { 1282, 1328 } }) { StartValue = 1282, Location = PrintLocation.Left });
            byz.AddEntry(new LeaderEntry("Andronikos III", new int?[,] { { 1328, 1341 } }) { StartValue = 1328, Location = PrintLocation.Left });
            byz.AddEntry(1341, 1347, "Civil War");

            byz.AddEntry(1079, 1183, "Komnenid dyn.", PrintLocation.Left);
            byz.AddEntry(1188, 1203, "Angelid dyn.", PrintLocation.Left);
            byz.AddEntry(1205, 1259, "Nicaean Empire", PrintLocation.Right);
            byz.AddEntry(1265, 1350, "Palaiologan dyn.", PrintLocation.Left);


            var slj = new Block {Ruler = rule, Title = "Rum Seljuks", EndValue = 1307};
            slj.AddEntry(new LeaderEntry("Suleyman", new int?[,] { { 1077, 1082 } }) {StartValue = 1077, Location = PrintLocation.Left});
            slj.AddEntry(new LeaderEntry("Kilij Arslan", new int?[,] { { 1092, 1107 } }) { StartValue = 1092, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Malik Shah", new int?[,] { { 1110, 1116 } }) { StartValue = 1110, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Masud I", new int?[,] { { 1116, 1156 } }) { StartValue = 1116, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Kilij Arslan II", new int?[,] { { 1156, 1192 } }) { StartValue = 1156, Location = PrintLocation.Left });
            slj.AddEntry(1194, 1202, "wars of Ascension");
            slj.AddEntry(new LeaderEntry("Kaykhusraw I", new int?[,] { { 1205, 1211 } }) { StartValue = 1205, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Kaykaus I", new int?[,] { { 1211, 1220 } }) { StartValue = 1211, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Kayqubad I", new int?[,] { { 1220, 1237 } }) { StartValue = 1220, Location = PrintLocation.Left });
            slj.AddEntry(new LeaderEntry("Kaykhusraw II", new int?[,] { { 1237, 1246 } }) { StartValue = 1237, Location = PrintLocation.Left });
            slj.AddEntry(1252, 1304, "Mongol admin/intrl dicord");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire", StartValue = 1299};
            ott.AddEntry(new LeaderEntry("Osman I", new int?[,] { { 1299, 1326 } }) {StartValue = 1303 });
            ott.AddEntry(new LeaderEntry("Orhan", new int?[,] { { 1327, 1359 } }) { StartValue = 1328 });

            var mong = new Block {Ruler = rule, Title = "Mongols", StartValue = 1200, Width = 38};
            mong.AddEntry(new LeaderEntry("Genghis", new int?[,] { { 1206, 1227 } }) {StartValue = 1206, Location = PrintLocation.Left});
            mong.AddEntry(1215, "Battle of Zhongdu(1215)[N. China]");
            mong.AddEntry(1225, "Battle of Kalka R.(1223)[Ukraine][+]");
            mong.AddEntry(new LeaderEntry("Ogedei", new int?[,] { { 1229, 1241 } }) { StartValue = 1229, Location = PrintLocation.Left });
            mong.AddEntry(1233, "Invade Korea(1236)");
            mong.AddEntry(1238, "Battle of Mohi(1241)[Hungary]");
            mong.AddEntry(1241, "Battle of Legnica(1241)[Poland]");
            mong.AddEntry(new LeaderEntry("Guyuk", new int?[,] { { 1246, 1248 } }) { StartValue = 1248, Location = PrintLocation.Left });
            mong.AddEntry(new LeaderEntry("Mongke", new int?[,] { { 1251, 1259 } }) { StartValue = 1251, Location = PrintLocation.Left });
            mong.AddEntry(1265, "sack Hanio(1257)[Vietnam]");
            mong.AddEntry(new LeaderEntry("Kublai", new int?[,] { { 1260, 1294 } }) { StartValue = 1260, Location = PrintLocation.Left });
            mong.AddEntry(1274, "Battle of Bun'ei(1274)[Japan]");
            mong.AddEntry(1281, "Battle of Koan(1281)[Japan]");
            mong.AddEntry(new LeaderEntry("Toghan Temur", new int?[,] { { 1333, 1370 } }) { StartValue = 1333, Location = PrintLocation.Left });

            var ftd = new Block {Ruler = rule, Title = "Fatimid Caliphate", EndValue = 1171};
            ftd.AddEntry(new LeaderEntry("Abul-Qasim", new int?[,] { { 1094, 1101 } }) {StartValue = 1078, Location = PrintLocation.Left});
            ftd.AddEntry(new LeaderEntry("Abu Ali", new int?[,] { { 1101, 1130 } }) { StartValue = 1101, Location = PrintLocation.Left });
            ftd.AddEntry(new LeaderEntry("Abul-Maymun", new int?[,] { { 1130, 1149 } }) { StartValue = 1130, Location = PrintLocation.Left });
            ftd.AddEntry(new LeaderEntry("Abu Mansur", new int?[,] { { 1149, 1154 } }) { StartValue = 1149, Location = PrintLocation.Left });
            ftd.AddEntry(new LeaderEntry("Abul-Qasim", new int?[,] { { 1154, 1160 } }) { StartValue = 1154, Location = PrintLocation.Left });
            ftd.AddEntry(new LeaderEntry("Abu Muhammad", new int?[,] { { 1160, 1171 } }) { StartValue = 1160, Location = PrintLocation.Left });

            var ayy = new Block {Ruler = rule, Title = "Ayyubid Sultanates", StartValue = 1171, EndValue = 1260};
            ayy.AddEntry(new LeaderEntry("Saladin", new int?[,] { { 1174, 1193 } }) {StartValue = 1174, Location = PrintLocation.Left});
            ayy.AddEntry(1194, 1240, "wars of Ascension");

            var mam = new Block {Ruler = rule, Title = "Mamluk Sultanate", StartValue = 1250};
            mam.AddEntry(1254, 1350, "Bahri dyn.", PrintLocation.Left);

            var jrs = new Block {Ruler = rule, Title = "Crusader States", StartValue = 1090, EndValue = 1295};
            jrs.AddEntry(1100, 1149, "County of Edessa", PrintLocation.Left);
            jrs.AddEntry(1103, 1268, "Principality of Antioch", PrintLocation.Right);
            jrs.AddEntry(1106, 1288, "Kingdom of Jerusalem");
            jrs.AddEntry(1205,1261, "Latin Empire",PrintLocation.Left);
            jrs.AddEntry(1291, "Fall of Acre(1291)");

            var fr = new Block {Ruler = rule, Title = "Kingdom of France", Width = 28};
            fr.AddEntry(new LeaderEntry("Phillip I", new int?[,] { { 1052, 1108 } }) {StartValue = 1096, Location = PrintLocation.Left});
            fr.AddEntry(new LeaderEntry("Louis VI", new int?[,] { { 1108, 1137 } }) { StartValue = 1108, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Louis VII", new int?[,] { { 1137, 1180 } }) { StartValue = 1137, Location = PrintLocation.Left });
            fr.AddEntry(new HeresyEntry("Cathars", "duality") {StartValue = 1167});
            fr.AddEntry(new LeaderEntry("Phillip II", new int?[,] { { 1180, 1223 } }) { StartValue = 1180, Location = PrintLocation.Left });
            fr.AddEntry(1209, "Cathar Crusade(1209-1229)");
            fr.AddEntry(new LeaderEntry("Louis VII", new int?[,] { { 1223, 1226 } }) { StartValue = 1223, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Louis IX", new int?[,] { { 1226, 1270 } }) { StartValue = 1226, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Philip III", new int?[,] { { 1270, 1285 } }) { StartValue = 1270, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Philip IV", new int?[,] { { 1285, 1314 } }) { StartValue = 1285, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Louis X", new int?[,] { { 1314, 1316 } }) { StartValue = 1314, Location = PrintLocation.Left });
            fr.AddEntry(new LiteraryWorkEntry("The Divine Comedy", "Dante") {StartValue = 1320});
            fr.AddEntry(new LeaderEntry("Philip V", new int?[,] { { 1316, 1322 } }) { StartValue = 1317, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Charles IV", new int?[,] { { 1322, 1328 } }) { StartValue = 1325, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Philip VI", new int?[,] { { 1328, 1350 } }) { StartValue = 1328, Location = PrintLocation.Left });
            fr.AddEntry(new PhilosopherEntry("Thomas Aquinas", 1225,1274) {StartValue = 1240, Location = PrintLocation.Right});
            fr.AddEntry(1079, 1350, "Capetian dyn.", PrintLocation.Left);
            fr.AddEntry(1163, 1340, "Notre Dame ctor(1163-1345)", PrintLocation.Right);
            fr.AddEntry(1309, 1350, "Avignon Papacy(1307-1377)");

            var plate = new Plate {Ruler = rule, Name = "High Middle Ages"};
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

            plate.AddArrow(new Arrow(fr, slj)
            {
                StartValue = 1096,
                Text = "First Crusade(1096-1099)",
                ArrowTail = "...",
                FromLeftToRightArrowHead = ".....>"
            });
            plate.AddArrow(new Arrow(slj, byz) {StartValue = 1171, Text = "Battle of Myriokephalon(1176)"});
            plate.AddArrow(new Arrow(fr, slj)
            {
                StartValue = 1145,
                Text = "Second Crusade(1145-1149)",
                ArrowTail = "...",
                FromLeftToRightArrowHead = ".....>"
            });
            plate.AddArrow(new Arrow(engd, slj)
            {
                StartValue = 1189,
                Text = "Third Crusade(1189-1192)",
                ArrowTail = "..",
                FromLeftToRightArrowHead = ".....>"
            });
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
            plate.AddArrow(new Arrow(fr, byz)
            {
                StartValue = 1202,
                Text = "Fourth Crusade(1202-1204)",
                FromLeftToRightArrowHead = ".>"
            });
            plate.AddArrow(new Arrow(hre, ayy) {StartValue = 1213, Text = "Fifth Crusade(1213-1221)"});
            plate.AddArrow(new Arrow(hre, ayy)
            {
                StartValue = 1228,
                Text = "Sixth Crusade(1228)",
                FromLeftToRightArrowHead = ".......>"
            });
            plate.AddArrow(new Arrow(mong, slj) {StartValue = 1243, Text = "Battle of Kose Dag(1243)"});
            plate.AddArrow(new Arrow(jrs, ayy) {StartValue = 1248, Text = "Battle of Harbiyah(1244)"});
            plate.AddArrow(new Arrow(mong, ayy) {StartValue = 1258, Text = "Siege of Baghdad(1258)"});
            plate.AddArrow(new Arrow(mam, mong) {StartValue = 1258, Text = "Battle of Ain Jalut(1260)[a]"});
            plate.AddArrow(new Arrow(mam, mong) {StartValue = 1299, Text = "B. of Wadi al-Khazandar(1299) "});
            plate.AddArrow(new Arrow(mam, mong) {StartValue = 1303, Text = "B. of Marj al-Saffar(1303)"});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1324, Text = "capture of Bursa(1326)"});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1333, Text = "capture Nicaea(1333)"});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1337, Text = "capture Nicomedia(1337)"});

            plate.Notes.Add("* Jerusalem is retaken by Muslims");
            plate.Notes.Add(
                "** John of England was weakened thereby submitting to sign the Magna Carta while Phillip centralized monarchy ruled until French Rev.");
            plate.Notes.Add("*** This causes wide spread famine and population decline.");
            plate.Notes.Add(
                "**** By the end of this period the population of the west is cut by 1/2th of what it was prior.");
            plate.Notes.Add(
                "[+] Republic of Vience had struck a deal with the Mongols to destroy any other European trading posts.");
            plate.Notes.Add(
                "[a] first time Mongols ever permanently beaten back in direct combat - Mongols never reach the Nile.");
            plate.Notes.Add("Sultan : sovereign;  Emir : local cheiften; Caliph : successor to Muhammad");
            return plate;
        }
    }
}
