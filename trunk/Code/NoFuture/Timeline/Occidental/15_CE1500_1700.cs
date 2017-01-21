namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1500to1700()
        {

            var rule = new Rule {StartValue = 1500, EndValue = 1705, RuleLineSpacing = 7};

            var amer = new Block {Ruler = rule, Title = "Americas", Width = 50};
            amer.AddEntry(new ExplorerEntry("Balboa", "Pacific from Panama", 1513) {StartValue = 1506, Location = PrintLocation.Left});
            amer.AddEntry(new ExplorerEntry("Ponce d'Leon", "Florida", 1513) { StartValue = 1510, Location = PrintLocation.Left });
            amer.AddEntry(1512, "1st African-native slaves(1517)");
            amer.AddEntry(new ExplorerEntry("Franciso Garay", "Mississippi R.", 1519) { StartValue = 1517, Location = PrintLocation.Left });

            amer.AddEntry(new ExplorerEntry("Magellan", "cir.navigation") { StartValue = 1522, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("Verrazano", "N.Amer.East Coast") { StartValue = 1525, Location = PrintLocation.Left });

            amer.AddEntry(new ExplorerEntry("Jacques Cartier", "St. Lawrence R.") { StartValue = 1534, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("Cortes", "California") { StartValue = 1536, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("De Soto", "Alabama") { StartValue = 1539, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("Coronado", "Arizonia, Kansas",1541) { StartValue = 1542, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("Frobisher", "NW Passage") { StartValue = 1576, EndValue = 1578, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("H.Gilbert", "claims Newfoundland for England") { StartValue = 1583, Location = PrintLocation.Left });
            amer.AddEntry(new ExplorerEntry("W.Raleigh", "claims Virginia for England", 1584) { StartValue = 1586, Location = PrintLocation.Left });
            amer.AddEntry(1590, "Roanoke Island-Lost Colony(1590)");
            amer.AddEntry(1603, "French colony in Nova Scotia(1603)");
            amer.AddEntry(1607, "Jamestown, Virginia(1607)(d.)");
            amer.AddEntry(new ExplorerEntry("H.Hudson", "Hudson Riv.&Bay") { StartValue = 1609, EndValue = 1610, Location = PrintLocation.Left });

            amer.AddEntry(1619, "1st N.Amer African slaves @ Jamestown(1619)");
            amer.AddEntry(1623, "Pilgrims @ Plymouth(1620)");
            amer.AddEntry(1628, "Dutch purchase Manhattan Island(1626)");
            amer.AddEntry(1631, "Boston, MA founded(1630)");
            amer.AddEntry(1636, "Harvard College(1636)");
            amer.AddEntry(1639, "Delaware settled by Finns & Swedes(1638)");
            amer.AddEntry(1650, "settlment in N.Carolina(1650)");
            amer.AddEntry(1661, "Charlestown SC(1663)");
            amer.AddEntry(1664, "English take New Amsterdam-rename New York(1664)");
            amer.AddEntry(new ExplorerEntry("La.Salle", "Ohio Riv.&Lake Mich.") { StartValue = 1669, EndValue = 1670, Location = PrintLocation.Left });
            amer.AddEntry(1682, "Philadelphia, PA founded(1682)");
            amer.AddEntry(new ExplorerEntry("La.Salle", "Mid West N.Amer", 1683) { StartValue = 1685, Location = PrintLocation.Left });

            var engd = new Block {Ruler = rule, Title = "England", Width = 36};
            engd.AddEntry(new LeaderEntry("Henry VIII", new int?[,] { { 1509, 1547 } }) {StartValue = 1509, Location = PrintLocation.Left});
            engd.AddEntry(new LeaderEntry("Edward VI", new int?[,] { { 1547, 1553 } }) { StartValue = 1547, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Mary", new int?[,] { { 1553, 1558 } }) { StartValue = 1553, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Elizabeth I", new int?[,] { { 1558, 1603 } }) { StartValue = 1558, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("James I", new int?[,] { { 1603, 1625 } }) { StartValue = 1603, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Charles I", new int?[,] { { 1625, 1649 } }) { StartValue = 1625, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("O. Cromwell", new int?[,] { { 1644, 1658 } }) { StartValue = 1645, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("R. Cromwell", new int?[,] { { 1658, 1660 } }) { StartValue = 1656, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("Charles II", new int?[,] { { 1660, 1685 } }) { StartValue = 1663, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("James II", new int?[,] { { 1685, 1688 } }) { StartValue = 1685, Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("William III", new int?[,] { { 1689, 1702 } }) { StartValue = 1689, Location = PrintLocation.Left });

            engd.AddEntry(new LiteraryWorkEntry("Utopia", "Thomas More") {StartValue = 1516, Location = PrintLocation.Right});
            engd.AddEntry(1534, "Church Of England(1534)", PrintLocation.Right);
            engd.AddEntry(new ScienceAdvEntry("Stock Frame invented(c.)", "", 1589) {StartValue = 1595, Location = PrintLocation.Right});
            engd.AddEntry(1580, "W.Shakespeare (1564-1616)", PrintLocation.Right);
            engd.AddEntry(1605, "Gunpowder Plot(1605)");
            engd.AddEntry(new LiteraryWorkEntry("KJV Bible", "") { StartValue = 1611, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry(".Logarithms", "J.Napier") { StartValue = 1614, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("Novum Organum", "F.Bacon", 1620) { StartValue = 1619, Location = PrintLocation.Right });
            engd.AddEntry(1642, "English Civil War(1642-1651)");
            engd.AddEntry(new LiteraryWorkEntry("Leviathan", "T.Hobbes") { StartValue = 1651, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("De Motu Cordis(e.)", "W.Harvey") { StartValue = 1628, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("Paradise Lost", "Milton") { StartValue = 1667, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("Principia", "Newton") { StartValue = 1687, Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("Two Treatises", "J.Locke", 1689) { StartValue = 1691, Location = PrintLocation.Right });

            engd.AddEntry(1660, 1700, "Stuart(restored)", PrintLocation.Left);
            engd.AddEntry(1600, 1649, "Stuart", PrintLocation.Left);
            engd.AddEntry(1503, 1553, "Tudor", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(new LeaderEntry("Louis XII", new int?[,] { { 1498, 1515 } }) {StartValue = 1509, Location = PrintLocation.Left});
            fr.AddEntry(new LeaderEntry("Francis I", new int?[,] { { 1515, 1547 } }) { StartValue = 1515, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Henry II", new int?[,] { { 1547, 1559 } }) { StartValue = 1547, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Francis II", new int?[,] { { 1559, 1560 } }) { StartValue = 1559, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Charles IX", new int?[,] { { 1560, 1574 } }) { StartValue = 1563, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Henry III", new int?[,] { { 1574, 1589 } }) { StartValue = 1576, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Henry IV", new int?[,] { { 1589, 1610 } }) { StartValue = 1594, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Louis XIII", new int?[,] { { 1610, 1643 } }) { StartValue = 1610, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Richelieu PM", new int?[,] { { 1629, 1642 } }) { StartValue = 1629, Location = PrintLocation.Left });
            fr.AddEntry(new LeaderEntry("Louis XIV", new int?[,] { { 1643, 1715 } }) { StartValue = 1643, Location = PrintLocation.Left });

            fr.AddEntry(1598, "Edict of Nantes(1598)", PrintLocation.Right);
            fr.AddEntry(new LiteraryWorkEntry("Discourse..Method", "Descartes") {StartValue = 1637, Location = PrintLocation.Right});
            fr.AddEntry(new ScienceAdvEntry("pendulum clock", "C.Huygens") {StartValue = 1656, Location = PrintLocation.Right});
            fr.AddEntry(new ScienceAdvEntry("vacuum pump", "O.Guericke") { StartValue = 1654, Location = PrintLocation.Right });
            fr.AddEntry(new ScienceAdvEntry("microscope", "Leeuwenhoek", 1675) { StartValue = 1673, Location = PrintLocation.Right });
            fr.AddEntry(new LiteraryWorkEntry("Ethics", "B.Spinoza") { StartValue = 1677, Location = PrintLocation.Right });

            fr.AddEntry(1685, "revocation of Edict of Nantes(1685)", PrintLocation.Left);
            fr.AddEntry(1589, 1792, "Capetian[Bourbon]", PrintLocation.Left);
            fr.AddEntry(1503, 1586, "Capetian[Valois]", PrintLocation.Left);

            var esp = new Block {Ruler = rule, Title = "Spain & Portugal"};
            esp.AddEntry(new LeaderEntry("Charles V", new int?[,] { { 1516, 1556 } }) {StartValue = 1516, Location = PrintLocation.Left});
            esp.AddEntry(new LeaderEntry("Philip II", new int?[,] { { 1556, 1598 } }) { StartValue = 1556, Location = PrintLocation.Left });
            esp.AddEntry(new LeaderEntry("Philip III", new int?[,] { { 1598, 1621 } }) { StartValue = 1598, Location = PrintLocation.Left });
            esp.AddEntry(new LeaderEntry("Philip IV", new int?[,] { { 1621, 1665 } }) { StartValue = 1621, Location = PrintLocation.Left });
            esp.AddEntry(new LeaderEntry("Charles II", new int?[,] { { 1665, 1700 } }) { StartValue = 1665, Location = PrintLocation.Left });

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
            hre.AddEntry(new LeaderEntry("Maximilian I", new int?[,] { { 1486, 1519 } }) {StartValue = 1511, Location = PrintLocation.Left});
            hre.AddEntry(new LeaderEntry("Ferdinand I", new int?[,] { { 1531, 1564 } }) { StartValue = 1540, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Maximilian II", new int?[,] { { 1564, 1576 } }) { StartValue = 1564, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Rudolf II", new int?[,] { { 1575, 1612 } }) { StartValue = 1575, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Matthias", new int?[,] { { 1612, 1619 } }) { StartValue = 1612, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Ferdinand II", new int?[,] { { 1619, 1637 } }) { StartValue = 1623, Location = PrintLocation.Left });
            hre.AddEntry(new LeaderEntry("Ferdinand III", new int?[,] { { 1637, 1657 } }) { StartValue = 1637, Location = PrintLocation.Left });

            hre.AddEntry(1509, "Michelangelo - Sistine Chapel(1508)", PrintLocation.Right);
            hre.AddEntry(new LiteraryWorkEntry("Theses", "Martin Luther") {StartValue = 1517, Location = PrintLocation.Right});
            //hre.AddEntry(1517, " ''()", PrintLocation.Right);
            hre.AddEntry(new LiteraryWorkEntry("The Prince", "Machiavelli") { StartValue = 1532, Location = PrintLocation.Right });
            hre.AddEntry(new LiteraryWorkEntry("Celestial Spheres", "Copernicus") { StartValue = 1543, Location = PrintLocation.Right });
            hre.AddEntry(1545, "Council of Trent(1545,1563)", PrintLocation.Right);
            hre.AddEntry(1555, "Peace of Augsburg(1555)", PrintLocation.Right);
            hre.AddEntry(1582, "Gregorian Calendar(1582)", PrintLocation.Right);
            hre.AddEntry(new ScienceAdvEntry("telescope", "Galileo") {StartValue = 1609, Location = PrintLocation.Right});
            hre.AddEntry(new ScienceAdvEntry("barometer", "Torricelli") { StartValue = 1643, Location = PrintLocation.Right });
            hre.AddEntry(1648, "Peace of Westphalia(1648)");

            hre.AddEntry(1618, 1648, "Thirty Years War(1618-1648)(g.)", PrintLocation.Right);

            var swd = new Block {Ruler = rule, Title = "Swedish Empire", StartValue = 1611};
            swd.AddEntry(new LeaderEntry("Gustavus Adolphus", new int?[,] { { 1611, 1632 } }) {StartValue = 1614, Location = PrintLocation.Left});
            swd.AddEntry(new LeaderEntry("Kristina", new int?[,] { { 1632, 1654 } }) { StartValue = 1632, Location = PrintLocation.Left });
            swd.AddEntry(new LeaderEntry("Karl X Gustav", new int?[,] { { 1654, 1660 } }) { StartValue = 1654, Location = PrintLocation.Left });
            swd.AddEntry(new LeaderEntry("Karl XI", new int?[,] { { 1660, 1697 } }) { StartValue = 1660, Location = PrintLocation.Left });

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire", Width = 34};
            ott.AddEntry(new LeaderEntry("Bayezid II", new int?[,] { { 1481, 1512 } }) { StartValue = 1503, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Selim I", new int?[,] { { 1512, 1520 } }) { StartValue = 1512, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Suleiman I", new int?[,] { { 1520, 1566 } }) { StartValue = 1520, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Selim II", new int?[,] { { 1566, 1574 } }) { StartValue = 1566, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Murad III", new int?[,] { { 1574, 1595 } }) { StartValue = 1570, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Mehmed III", new int?[,] { { 1595, 1603 } }) { StartValue = 1595, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Ahmed I", new int?[,] { { 1603, 1617 } }) { StartValue = 1603, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Mustafa I", new int?[,] { { 1617, 1618 }, { 1622, 1623 } }) { StartValue = 1617, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Osman II", new int?[,] { { 1618, 1622 } }) { StartValue = 1620, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Murad IV", new int?[,] { { 1623, 1640 } }) { StartValue = 1623, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Ibrahim", new int?[,] { { 1640, 1648 } }) { StartValue = 1640, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Mehmed IV", new int?[,] { { 1648, 1687 } }) { StartValue = 1648, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Suleiman II", new int?[,] { { 1687, 1691 } }) { StartValue = 1687, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Ahmed II", new int?[,] { { 1691, 1695 } }) { StartValue = 1691, Location = PrintLocation.Left });
            ott.AddEntry(new LeaderEntry("Mustafa II", new int?[,] { { 1695, 1703 } }) { StartValue = 1695, Location = PrintLocation.Left });

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
