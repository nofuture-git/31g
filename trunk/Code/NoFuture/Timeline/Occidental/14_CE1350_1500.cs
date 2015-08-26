using System;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1350to1500()
        {
            var rule = new Rule { StartValue = 1350, EndValue = 1500, RuleLineSpacing = 7 };

            var engd = new Block { Ruler = rule, Title = "Kingdom of England", Width = 28 };
            engd.AddEntry(1381,"Peasent Revolt(1381)");
            engd.AddEntry(1327, "Edward III(1327-1377)", PrintLocation.Left);
            engd.AddEntry(1377, "Richard II(1377-1399)", PrintLocation.Left);
            engd.AddEntry(1382, "'The Canterbury Tales'",PrintLocation.Right);
            engd.AddEntry(1384,"Wycliffe translates Bible",PrintLocation.Right);
            engd.AddEntry(1405,"Henry IV(1399-1413)", PrintLocation.Left);
            engd.AddEntry(1413, "Henry V(1413-1422)",PrintLocation.Left);
            engd.AddEntry(1422,"Henry VI(1422-1461)",PrintLocation.Left);
            engd.AddEntry(1470,"Edward IV(1461-1483)",PrintLocation.Left);
            engd.AddEntry(1479,"Edward V(1483)",PrintLocation.Left);
            engd.AddEntry(1481,"Richard III(1483-1485)",PrintLocation.Left);
            engd.AddEntry(1485,"Battle of Bosworth(1485)**");
            engd.AddEntry(1491,"Henry VII(1485-1509)",PrintLocation.Left);

            engd.AddEntry(1399, 1459, "House of Lancaster", PrintLocation.Left);
            engd.AddEntry(1355, 1415, "Hundred Years' War (1337-1415)");
            engd.AddEntry(1455, 1485, "War of the Roses(1455-1485)");
            engd.AddEntry(1461, 1485, "House of York", PrintLocation.Left);
            engd.AddEntry(1488, 1500, "The Tudors", PrintLocation.Left);

            var fr = new Block { Ruler = rule, Title = "Kingdom of France", Width = 28 };
            fr.AddEntry(1358,"Peasent Revolt(1358)");
            fr.AddEntry(1353,"John II(1350-1364)",PrintLocation.Left);
            fr.AddEntry(1364,"Charles V(1364-1380)",PrintLocation.Left);
            fr.AddEntry(1380,"Charles VI(1380-1422)",PrintLocation.Left);
            fr.AddEntry(1422,"Charles VII(1422-1461)",PrintLocation.Left);
            fr.AddEntry(1461,"Louis XI(1461-1483)",PrintLocation.Left);
            fr.AddEntry(1483,"Charles VIII(1483-1498)", PrintLocation.Left);
            fr.AddEntry(1485,"'Le Morte d`Arthur'(1485)");

            var byz = new Block { Ruler = rule, Title = "Byzantine Empire", EndValue = 1453};
            byz.AddEntry(1353,"John V(1341-1391) [b]",PrintLocation.Left);
            byz.AddEntry(1391,"Manuel II(1391-1425)",PrintLocation.Left);
            byz.AddEntry(1425,"John VIII(1425-1448)",PrintLocation.Left);
            byz.AddEntry(1449,"Constantine XI(1449-1453)",PrintLocation.Left);

            var hre = new Block { Ruler = rule, Title = "Holy Roman Empire" };
            hre.AddEntry(1355,"Charles IV(1347-1378)",PrintLocation.Left);
            hre.AddEntry(1378,"Wenceslas(1378-1400)",PrintLocation.Left);
            hre.AddEntry(1386, "Univ. of Heidelberg (1386)");
            hre.AddEntry(1400,"Rupert(1400-1410)",PrintLocation.Left);
            hre.AddEntry(1410,"Sigismund(1410-1437)",PrintLocation.Left);
            hre.AddEntry(1438,"Albert II(1438/39)",PrintLocation.Left);
            hre.AddEntry(1440,"printing press (1439)");
            hre.AddEntry(1442,"Frederick III(1440-1493)",PrintLocation.Left);
            hre.AddEntry(1493,"Maximilian I(1493-1519)",PrintLocation.Left);

            hre.AddEntry(1419,1434,"Hussite Wars(1419-1434)");

            var ppl = new Block() {Ruler = rule, Title = "Papal State"};
            ppl.AddEntry(1356,"Innocent VI(1352-1362)",PrintLocation.Left);
            ppl.AddEntry(1362,"Urban V(1362-1370)",PrintLocation.Left);
            ppl.AddEntry(1370,"Gregory XI(1370-1378)",PrintLocation.Left);
            ppl.AddEntry(1414, "Cncil. of Constance(1414-1418)");
            ppl.AddEntry(1417, "Martin V(1417-1431)", PrintLocation.Left);
            ppl.AddEntry(1431,"Eugenius IV(1431-1447)",PrintLocation.Left);
            ppl.AddEntry(1447,"Nicholas V(1447-1455)",PrintLocation.Left);
            ppl.AddEntry(1455, "Calixtus III(1455-1458)",PrintLocation.Left);
            ppl.AddEntry(1458, "Pius II(1458-1464)",PrintLocation.Left);
            ppl.AddEntry(1464,"Paul II(1464-1471)",PrintLocation.Left);
            ppl.AddEntry(1471,"Sixtus IV(1471-1484)",PrintLocation.Left);
            ppl.AddEntry(1484,"Innocent VIII(1484-1492)",PrintLocation.Left);
            ppl.AddEntry(1490,"Alexander VI(1492-1503)",PrintLocation.Left);
            ppl.AddEntry(1492, "Reconquista/Columbus");
            ppl.AddEntry(1494, "Treaty of Tordesillas(1494)");

            ppl.AddEntry(1378,1417,"Western Schism(3 Popes)");

            var ott = new Block {Ruler = rule, Title = "Ottoman Turks"};
            ott.AddEntry(1362, "Murad I(1362-1389)", PrintLocation.Left);
            ott.AddEntry(1389, "Bayezid I(1389-1403)", PrintLocation.Left);
            ott.AddEntry(1402,1413,"Civil War(1402-1413)*");
            ott.AddEntry(1421, "Murad II(1421-1451)", PrintLocation.Left);
            ott.AddEntry(1451,"Mehmed II(1451-1481)",PrintLocation.Left);
            ott.AddEntry(1481,"Bayezid II(1481-1512)",PrintLocation.Left);

            var rus = new Block {Ruler = rule, Title = "Russia", StartValue = 1470};
            rus.AddEntry(1473, "Ivan III(1462-1505)", PrintLocation.Left);
            rus.AddEntry(1476, "Ivan refuses tribute to Horde");
            rus.AddEntry(1478, "Rep. of Novgorod annexed(1478)");
            rus.AddEntry(1485, "Annex Tver(1485)");

            var plate = new Plate { Ruler = rule, Name = "Late Middle Ages" };

            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(hre);
            plate.AddBlock(ppl);
            plate.AddBlock(byz);
            plate.AddBlock(rus);
            plate.AddBlock(ott);

            plate.AddArrow(new Arrow(fr, ott) {StartValue = 1396, Text = "Crusade of Nicopolis(1396)" });
            plate.AddArrow(new Arrow(engd, fr){StartValue = 1415, Text = "Battle of Agincourt(1415)"});
            plate.AddArrow(new Arrow(ott, byz){StartValue = 1453, Text = "Fall of Constantinople"});
            plate.AddArrow(new Arrow(fr, engd) {StartValue = 1429, Text = "Joan of Arc[siege of Orleans](1429)"});

            plate.Notes.Add("* this was caused by the invasion of Timurid Empire and the Battle of Ankara(1402) in which Bayezid I was captured by the Mongol forces.");
            plate.Notes.Add("** final victory of War of the Roses Henry Tudor defeats Richard III");
            plate.Notes.Add("[b] restored to senior emperor three times.");

            return plate;
        }
    }
}
