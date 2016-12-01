namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1500to1700()
        {

            var rule = new Rule {StartValue = 1500, EndValue = 1705, RuleLineSpacing = 7};

            var amer = new Block {Ruler = rule, Title = "Americas", Width = 50};
            amer.AddEntry(1506, "[Balboa] Pacific from Panama(1513)", PrintLocation.Left);
            amer.AddEntry(1510, "[Ponce d'Leon]-Florida(1513)", PrintLocation.Left);
            amer.AddEntry(1512, "1st African-native slaves(1517)");
            amer.AddEntry(1517, "[Franciso Garay]Mississippi R.(1519)", PrintLocation.Left);

            amer.AddEntry(1522, "[Magellan]cir.navigation(1522)", PrintLocation.Left);
            amer.AddEntry(1525, "[Verrazano]East Coast(1525)", PrintLocation.Left);

            amer.AddEntry(1534, "[Jacques Cartier]St. Lawrence R.(1534)", PrintLocation.Left);
            amer.AddEntry(1536, "[Cortes]-California(1536)", PrintLocation.Left);
            amer.AddEntry(1539, "[De Soto]-Alabama(1539)", PrintLocation.Left);
            amer.AddEntry(1542, "[Coronado]-Arizonia, Kansas(1540/41/42)", PrintLocation.Left);
            amer.AddEntry(1576, "[Frobisher]-NW Passage(1576-78)", PrintLocation.Left);
            amer.AddEntry(1583, "[H.Gilbert]-claims Newfoundland for England(1583)", PrintLocation.Left);
            amer.AddEntry(1586, "[W.Raleigh]-claims Virginia for England(1584)", PrintLocation.Left);
            amer.AddEntry(1590, "Roanoke Island-Lost Colony(1590)");
            amer.AddEntry(1603, "French colony in Nova Scotia(1603)");
            amer.AddEntry(1607, "Jamestown, Virginia(1607)(d.)");
            amer.AddEntry(1610, "[H.Hudson]-Hudson Riv.&Bay(1609/10)", PrintLocation.Left);

            amer.AddEntry(1619, "1st N.Amer African slaves @ Jamestown(1619)");
            amer.AddEntry(1623, "Pilgrims @ Plymouth(1620)");
            amer.AddEntry(1628, "Dutch purchase Manhattan Island(1626)");
            amer.AddEntry(1631, "Boston, MA founded(1630)");
            amer.AddEntry(1636, "Harvard College(1636)");
            amer.AddEntry(1639, "Delaware settled by Finns & Swedes(1638)");
            amer.AddEntry(1650, "settlment in N.Carolina(1650)");
            amer.AddEntry(1661, "Charlestown SC(1663)");
            amer.AddEntry(1664, "English take New Amsterdam-rename New York(1664)");
            amer.AddEntry(1669, "[La.Salle]-Ohio Riv.&Lake Mich.(1669/70)", PrintLocation.Left);
            amer.AddEntry(1682, "Philadelphia, PA founded(1682)");
            amer.AddEntry(1685, "[La.Salle]-Mid West N.Amer(1681-84)", PrintLocation.Left);

            var engd = new Block {Ruler = rule, Title = "England", Width = 36};
            engd.AddEntry(1509, "[Henry VIII 1509-1547]", PrintLocation.Left);
            engd.AddEntry(1547, "[Edward VI 1547-1553]", PrintLocation.Left);
            engd.AddEntry(1553, "[Mary 1553-1558]", PrintLocation.Left);
            engd.AddEntry(1558, "[Elizabeth I 1558-1603]", PrintLocation.Left);
            engd.AddEntry(1603, "[James I 1603-1625]", PrintLocation.Left);
            engd.AddEntry(1625, "[Charles I 1625-1649]", PrintLocation.Left);
            engd.AddEntry(1645, "[O. Cromwell 1644-1658]", PrintLocation.Left);
            engd.AddEntry(1656, "[R. Cromwell 1658-1660]", PrintLocation.Left);
            engd.AddEntry(1663, "[Charles II 1660-1685]", PrintLocation.Left);
            engd.AddEntry(1685, "[James II 1685-1688]", PrintLocation.Left);
            engd.AddEntry(1689, "[William III 1689-1702]", PrintLocation.Left);

            engd.AddEntry(1516, "'Utopia' Thomas More(1516)", PrintLocation.Right);
            engd.AddEntry(1534, "Church Of England(1534)", PrintLocation.Right);
            engd.AddEntry(1595, "Stock Frame invented(1589)(c.)", PrintLocation.Right);
            engd.AddEntry(1580, "W.Shakespeare (1564-1616)", PrintLocation.Right);
            engd.AddEntry(1605, "Gunpowder Plot(1605)");
            engd.AddEntry(1611, "'KJV Bible'(1611)", PrintLocation.Right);
            engd.AddEntry(1614, "'.Logarithms' J.Napier(1614)", PrintLocation.Right);
            engd.AddEntry(1619, "'Novum Organum' F.Bacon(1620)", PrintLocation.Right);
            engd.AddEntry(1642, "English Civil War(1642-1651)");
            engd.AddEntry(1651, "'Leviathan' T.Hobbes(1651)", PrintLocation.Right);
            engd.AddEntry(1628, "'De Motu Cordis' W.Harvey(1628)(e.)", PrintLocation.Right);
            engd.AddEntry(1667, "'Paradise Lost' Milton(1667)", PrintLocation.Right);
            engd.AddEntry(1687, "'Principia' Newton(1687)", PrintLocation.Right);
            engd.AddEntry(1691, "'Two Treatises' J.Locke(1689)", PrintLocation.Right);

            engd.AddEntry(1660, 1700, "Stuart(restored)", PrintLocation.Left);
            engd.AddEntry(1600, 1649, "Stuart", PrintLocation.Left);
            engd.AddEntry(1503, 1553, "Tudor", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(1509, "[Louis XII 1498-1515]", PrintLocation.Left);
            fr.AddEntry(1515, "[Francis I 1515-1547]", PrintLocation.Left);
            fr.AddEntry(1547, "[Henry II 1547-1559]", PrintLocation.Left);
            fr.AddEntry(1559, "[Francis II 1559/60]", PrintLocation.Left);
            fr.AddEntry(1563, "[Charles IX 1560-1574]", PrintLocation.Left);
            fr.AddEntry(1576, "[Henry III 1574-1589]", PrintLocation.Left);
            fr.AddEntry(1594, "[Henry IV 1589-1610]", PrintLocation.Left);
            fr.AddEntry(1610, "[Louis XIII 1610-1643]", PrintLocation.Left);
            fr.AddEntry(1629, "[Richelieu PM 1629-1642]", PrintLocation.Left);
            fr.AddEntry(1643, "[Louis XIV 1643-1715]", PrintLocation.Left);

            fr.AddEntry(1598, "Edict of Nantes(1598)", PrintLocation.Right);
            fr.AddEntry(1637, "'Discourse on the Method'", PrintLocation.Right);
            fr.AddEntry(1639, "    R.Descartes(1637)", PrintLocation.Right);
            fr.AddEntry(1656, "C.Huygens[pendulum clock](1656)", PrintLocation.Right);
            fr.AddEntry(1654, "O.Guericke[vacuum pump](1654)", PrintLocation.Right);
            fr.AddEntry(1673, "Leeuwenhoek[microscope](1675)", PrintLocation.Right);
            fr.AddEntry(1677, "'Ethics' B.Spinoza(1677)", PrintLocation.Right);

            fr.AddEntry(1685, "revocation of Edict of Nantes(1685)", PrintLocation.Left);
            fr.AddEntry(1589, 1792, "Capetian[Bourbon]", PrintLocation.Left);
            fr.AddEntry(1503, 1586, "Capetian[Valois]", PrintLocation.Left);

            var esp = new Block {Ruler = rule, Title = "Spain & Portugal"};
            esp.AddEntry(1516, "[Charles V 1516-1556]", PrintLocation.Left);
            esp.AddEntry(1556, "[Philip II 1556-1598]", PrintLocation.Left);
            esp.AddEntry(1598, "[Philip III 1598-1621]", PrintLocation.Left);
            esp.AddEntry(1621, "[Philip IV 1621-1665]", PrintLocation.Left);
            esp.AddEntry(1665, "[Charles II 1665-1700]", PrintLocation.Left);

            esp.AddEntry(1510, "Portuguese in India(1510)", PrintLocation.Right);
            esp.AddEntry(1542, "Portuguese in Japan(1542)", PrintLocation.Right);
            esp.AddEntry(1606, "Portuguese discover Australia(1606)", PrintLocation.Right);

            esp.AddEntry(1503, 1700, "Hapsburg Dyn.", PrintLocation.Left);

            var hre = new Block
            {
                Ruler = rule,
                Title = "Holy Roman Empire",
                Width = 38,
                StartValue = 1500,
                EndValue = 1648
            };
            hre.AddEntry(1511, "[Maximilian I 1486-1519]", PrintLocation.Left);
            hre.AddEntry(1540, "[Ferdinand I 1531-1564]", PrintLocation.Left);
            hre.AddEntry(1564, "[Maximilian II 1564-1576]", PrintLocation.Left);
            hre.AddEntry(1575, "[Rudolf II 1575-1612]", PrintLocation.Left);
            hre.AddEntry(1612, "[Matthias 1612-1619]", PrintLocation.Left);
            hre.AddEntry(1623, "[Ferdinand II 1619-1637]", PrintLocation.Left);
            hre.AddEntry(1637, "[Ferdinand III 1637-1657]", PrintLocation.Left);

            hre.AddEntry(1509, "Michelangelo - Sistine Chapel(1508)", PrintLocation.Right);
            hre.AddEntry(1517, "Martin Luther 'Theses'(1517)", PrintLocation.Right);
            hre.AddEntry(1525, "German Peasants' War(1525)");
            hre.AddEntry(1532, "'The Prince' Machiavelli(1532)", PrintLocation.Right);
            hre.AddEntry(1543, "'Celestial Spheres' Copernicus(1543)", PrintLocation.Right);
            hre.AddEntry(1545, "Council of Trent(1545,1563)", PrintLocation.Right);
            hre.AddEntry(1555, "Peace of Augsburg(1555)", PrintLocation.Right);
            hre.AddEntry(1582, "Gregorian Calendar(1582)", PrintLocation.Right);
            hre.AddEntry(1609, "Galileo[telescope](1609)", PrintLocation.Right);
            hre.AddEntry(1643, "Torricelli[barometer](1643)", PrintLocation.Right);
            hre.AddEntry(1648, "Peace of Westphalia(1648)");

            hre.AddEntry(1618, 1648, "Thirty Years War(1618-1648)(g.)", PrintLocation.Right);

            var swd = new Block {Ruler = rule, Title = "Swedish Empire", StartValue = 1611};
            swd.AddEntry(1614, "[Gustavus Adolphus 1611-1632]", PrintLocation.Left);
            swd.AddEntry(1632, "[Kristina 1632-1654]", PrintLocation.Left);
            swd.AddEntry(1654, "[Karl X Gustav 1654-1660]", PrintLocation.Left);
            swd.AddEntry(1660, "[Karl XI 1660-1697]", PrintLocation.Left);

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire", Width = 34};
            ott.AddEntry(1503, "[Bayezid II 1481-1512]", PrintLocation.Left);
            ott.AddEntry(1512, "[Selim I 1512-1520]", PrintLocation.Left);
            ott.AddEntry(1520, "[Suleiman I 1520-1566]", PrintLocation.Left);
            ott.AddEntry(1566, "[Selim II 1566-1574]", PrintLocation.Left);
            ott.AddEntry(1570, "[Murad III 1574-1595]", PrintLocation.Left);
            ott.AddEntry(1595, "[Mehmed III 1595-1603]", PrintLocation.Left);
            ott.AddEntry(1603, "[Ahmed I 1603-1617]", PrintLocation.Left);
            ott.AddEntry(1617, "[Mustafa I 1617/18/22/23]", PrintLocation.Left);
            ott.AddEntry(1620, "[Osman II 1618-1622]", PrintLocation.Left);
            ott.AddEntry(1623, "[Murad IV 1623-1640]", PrintLocation.Left);
            ott.AddEntry(1640, "[Ibrahim 1640-1648]", PrintLocation.Left);
            ott.AddEntry(1648, "[Mehmed IV 1648-1687]", PrintLocation.Left);
            ott.AddEntry(1687, "[Suleiman II 1687-1691]", PrintLocation.Left);
            ott.AddEntry(1691, "[Ahmed II 1691-1695]", PrintLocation.Left);
            ott.AddEntry(1695, "[Mustafa II 1695-1703]", PrintLocation.Left);

            ott.AddEntry(1517, "Egypt & Arabia conquered(1517/18)");

            var plate = new Plate {Ruler = rule, Name = "Early Modern Age(1)"};
            plate.AddBlock(amer);
            plate.AddBlock(engd);
            plate.AddBlock(esp);
            plate.AddBlock(fr);
            plate.AddBlock(swd);
            plate.AddBlock(hre);
            plate.AddBlock(ott);
            plate.Notes.Add("(a.)Eurasian disease est. to have killed 90% of W.Hemisphere pop.");
            plate.Notes.Add("(b.)Ottoman naval power limited to the east Mediterranean");
            plate.Notes.Add("(c.)First invention to mech. the textile industry.");
            plate.Notes.Add("(d.)Considered the beginning of the British Empire");
            plate.Notes.Add("(e.)circulation of blood in the body");
            plate.Notes.Add("(f.)marks the end of Ottoman expansion into Europe");
            plate.Notes.Add("(g.)300+ German principalities become independent of any central auth.");

            plate.AddArrow(new Arrow(ott, hre) {StartValue = 1534, Text = "Siege of Vienna(1529)"});
            plate.AddArrow(new Arrow(esp, ott) {StartValue = 1571, Text = "Battle of Lepanto(1571)(b.)"});
            plate.AddArrow(new Arrow(esp, amer) {StartValue = 1521, Text = "Aztec Empire conquered(1519-21)(a.)"});
            plate.AddArrow(new Arrow(esp, amer) {StartValue = 1532, Text = "Inca Empire conquered(1532)"});
            plate.AddArrow(new Arrow(esp, engd) {StartValue = 1588, Text = "Spanish Armada(1588)"});
            plate.AddArrow(new Arrow(ott, hre) {StartValue = 1599, Text = "Battle of Keresztes(1596)"});
            plate.AddArrow(new Arrow(esp, hre) {StartValue = 1620, Text = "Battle of White Mountain(1620)"});
            plate.AddArrow(new Arrow(swd, hre) {StartValue = 1631, Text = "Battle of Breitenfeld(1631)"});
            plate.AddArrow(new Arrow(swd, hre) {StartValue = 1634, Text = "Battle of Nordlingen(1634)"});
            plate.AddArrow(new Arrow(hre, ott) {StartValue = 1664, Text = "Battle of St.Gotthard(1664)"});
            plate.AddArrow(new Arrow(ott, hre) {StartValue = 1683, Text = "Siege of Vienna(1683)(f.)"});
            plate.AddArrow(new Arrow(hre, ott) {StartValue = 1700, Text = "Battle of Zenta(1697)"});
            return plate;
        }
    }
}
