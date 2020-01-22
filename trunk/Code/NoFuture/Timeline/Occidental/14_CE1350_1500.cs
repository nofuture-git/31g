namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1350to1500()
        {
            var rule = new Rule {StartValue = 1350, EndValue = 1500, RuleLineSpacing = 7};

            var engd = new Block {Ruler = rule, Title = "Kingdom of England", Width = 28};
            engd.AddEntry(1381, "Peasent Revolt(1381)");
            engd.AddEntry(new LeaderEntry("Edward III", new int?[,] { { 1327, 1377 } }));
            engd.AddEntry(new LeaderEntry("Richard II", new int?[,] { { 1377, 1399 } }));
            engd.AddEntry(new LiteraryWorkEntry("The Canterbury Tales", "", 1382) {Location = PrintLocation.Right});
            engd.AddEntry(1384, "Wycliffe translates Bible", PrintLocation.Right);
            engd.AddEntry(new LeaderEntry("Henry IV", new int?[,] { { 1399, 1413 } }) { StartValue = 1405});
            engd.AddEntry(new LeaderEntry("Henry V", new int?[,] { { 1413, 1422 } }));
            engd.AddEntry(new LeaderEntry("Henry VI", new int?[,] { { 1422, 1461 } }));
            engd.AddEntry(new LeaderEntry("Edward IV", new int?[,] { { 1461, 1483 } }) { StartValue = 1470 });
            engd.AddEntry(new LeaderEntry("Edward V", new int?[,] { { null, 1483 } }) { StartValue = 1479 });
            engd.AddEntry(new LeaderEntry("Richard III", new int?[,] { { 1483, 1485 } }) { StartValue = 1481 });
            engd.AddEntry(1485, "Battle of Bosworth(1485)**");
            engd.AddEntry(new LeaderEntry("Henry VII", new int?[,] { { 1485, 1509 } }) { StartValue = 1491 });

            engd.AddEntry(1399, 1459, "House of Lancaster", PrintLocation.Left);
            engd.AddEntry(1355, 1415, "Hundred Years' War (1337-1415)");
            engd.AddEntry(1455, 1485, "War of the Roses(1455-1485)");
            engd.AddEntry(1461, 1485, "House of York", PrintLocation.Left);
            engd.AddEntry(1488, 1500, "The Tudors", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "Kingdom of France", Width = 28};
            fr.AddEntry(1358, "Peasent Revolt(1358)");
            fr.AddEntry(new LeaderEntry("John II", new int?[,] { { 1350, 1364 } }) {StartValue = 1353});
            fr.AddEntry(new LeaderEntry("Charles V", new int?[,] { { 1364, 1380 } }));
            fr.AddEntry(new LeaderEntry("Charles VI", new int?[,] { { 1380, 1422 } }));
            fr.AddEntry(new LeaderEntry("Charles VII", new int?[,] { { 1422, 1461 } }));
            fr.AddEntry(new LeaderEntry("Louis XI", new int?[,] { { 1461, 1483 } }));
            fr.AddEntry(new LeaderEntry("Charles VIII", new int?[,] { { 1483, 1498 } }));
            fr.AddEntry(new LiteraryWorkEntry("Le Morte d`Arthur", "") {StartValue = 1485});

            var byz = new Block {Ruler = rule, Title = "Byzantine Empire", EndValue = 1453};
            byz.AddEntry(new LeaderEntry("John V [b]", new int?[,] { { 1341, 1391 } }) {StartValue = 1353});
            byz.AddEntry(new LeaderEntry("Manuel II", new int?[,] { { 1391, 1425 } }));
            byz.AddEntry(new LeaderEntry("John VIII", new int?[,] { { 1425, 1448 } }));
            byz.AddEntry(new LeaderEntry("Constantine XI", new int?[,] { { 1449, 1453 } }));

            var hre = new Block {Ruler = rule, Title = "Holy Roman Empire"};
            hre.AddEntry(new LeaderEntry("Charles IV", new int?[,] { { 1347, 1378 } }) {StartValue = 1355});
            hre.AddEntry(new LeaderEntry("Wenceslas", new int?[,] { { 1378, 1400 } }));
            hre.AddEntry(1386, "Univ. of Heidelberg (1386)");
            hre.AddEntry(new LeaderEntry("Rupert", new int?[,] { { 1400, 1410 } }));
            hre.AddEntry(new LeaderEntry("Sigismund", new int?[,] { { 1410, 1437 } }));
            hre.AddEntry(new LeaderEntry("Albert II", new int?[,] { { 1438, 1439 } }) { StartValue = 1436});
            hre.AddEntry(new ScienceAdvEntry("printing press", "") {StartValue = 1439});
            hre.AddEntry(new LeaderEntry("Frederick III", new int?[,] { { 1440, 1493 } }) { StartValue = 1442 });
            hre.AddEntry(new LeaderEntry("Maximilian I", new int?[,] { { 1493, 1519 } }));
            hre.AddEntry(1419, 1434, "Hussite Wars(1419-1434)");

            var ppl = new Block {Ruler = rule, Title = "Papal State"};
            ppl.AddEntry(new LeaderEntry("Innocent VI", new int?[,] { { 1352, 1362 } }) {StartValue = 1356});
            ppl.AddEntry(new LeaderEntry("Urban V", new int?[,] { { 1362, 1370 } }));
            ppl.AddEntry(new LeaderEntry("Gregory XI", new int?[,] { { 1370, 1378 } }));
            ppl.AddEntry(1414, "Cncil. of Constance(1414-1418)");
            ppl.AddEntry(new LeaderEntry("Martin V", new int?[,] { { 1417, 1431 } }));
            ppl.AddEntry(new LeaderEntry("Eugenius IV", new int?[,] { { 1431, 1447 } }));
            ppl.AddEntry(new LeaderEntry("Nicholas V", new int?[,] { { 1447, 1455 } }));
            ppl.AddEntry(new LeaderEntry("Calixtus III", new int?[,] { { 1455, 1458 } }));
            ppl.AddEntry(new LeaderEntry("Pius II", new int?[,] { { 1458, 1464 } }));
            ppl.AddEntry(new LeaderEntry("Paul II", new int?[,] { { 1464, 1471 } }));
            ppl.AddEntry(new LeaderEntry("Sixtus IV", new int?[,] { { 1471, 1484 } }));
            ppl.AddEntry(new LeaderEntry("Innocent VIII", new int?[,] { { 1484, 1492 } }));
            ppl.AddEntry(new LeaderEntry("Alexander VI", new int?[,] { { 1492, 1503 } }) { StartValue = 1490});
            ppl.AddEntry(1492, "Reconquista/Columbus");
            ppl.AddEntry(1494, "Treaty of Tordesillas(1494)");

            ppl.AddEntry(1378, 1417, "Western Schism(3 Popes)");

            var ott = new Block {Ruler = rule, Title = "Ottoman Turks"};
            ott.AddEntry(new LeaderEntry("Murad I", new int?[,] { { 1362, 1389 } }));
            ott.AddEntry(new LeaderEntry("Bayezid I", new int?[,] { { 1389, 1403 } }));
            ott.AddEntry(1402, 1413, "Civil War(1402-1413)*");
            ott.AddEntry(new LeaderEntry("Murad II", new int?[,] { { 1421, 1451 } }));
            ott.AddEntry(new LeaderEntry("Mehmed II", new int?[,] { { 1451, 1481 } }));
            ott.AddEntry(new LeaderEntry("Bayezid II", new int?[,] { { 1481, 1512 } }));

            var rus = new Block {Ruler = rule, Title = "Russia", StartValue = 1470};
            rus.AddEntry(new LeaderEntry("Ivan III", new int?[,] { { 1462, 1505 } }) {StartValue = 1473});
            rus.AddEntry(1476, "Ivan refuses tribute to Horde");
            rus.AddEntry(1478, "Rep. of Novgorod annexed(1478)");
            rus.AddEntry(1485, "Annex Tver(1485)");

            var plate = new Plate {Ruler = rule, Name = "Late Middle Ages"};

            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(hre);
            plate.AddBlock(ppl);
            plate.AddBlock(byz);
            plate.AddBlock(rus);
            plate.AddBlock(ott);
            plate.FileName = "14_CE1350_1500";

            plate.AddArrow(new Arrow(fr, ott) {StartValue = 1396, Text = "Crusade of Nicopolis(1396)"});
            plate.AddArrow(new Arrow(engd, fr) {StartValue = 1415, Text = "Battle of Agincourt(1415)", ArrowTail = "..."});
            plate.AddArrow(new Arrow(ott, byz) {StartValue = 1453, Text = "Fall of Constantinople"});
            plate.AddArrow(new Arrow(fr, engd) {StartValue = 1429, Text = "Joan of Arc[siege of Orleans](1429)"});

            plate.Notes.Add(
                "* this was caused by the invasion of Timurid Empire and the Battle of Ankara(1402) in which Bayezid I was captured by the Mongol forces.");
            plate.Notes.Add("** final victory of War of the Roses Henry Tudor defeats Richard III");
            plate.Notes.Add("[b] restored to senior emperor three times.");

            return plate;
        }
    }
}
