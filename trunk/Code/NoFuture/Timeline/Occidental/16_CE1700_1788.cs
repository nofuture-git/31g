using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1700to1788()
        {
            var rule = new Rule {StartValue = 1700, EndValue = 1790, RuleLineSpacing = 7};
            var amer = new Block {Ruler = rule, Title = "Americas"};

            var col = new Block {StartValue = 1703, EndValue = 1783, Title = "Colonies", Ruler = rule};
            col.AddEntry(1771, "Boston Massacre(1770)", PrintLocation.Left);
            col.AddEntry(1703, "Yale College (1700)");
            col.AddEntry(new ExplorerEntry("Bering", "Bering straight", 1728) {StartValue = 1727});
            col.AddEntry(1739, "Stono Rebellion(1739)");
            col.AddEntry(1730, "Baltimore MA(1730)");
            col.AddEntry(1764, "Mason Dixon survey(1764)");
            col.AddEntry(new ExplorerEntry("D.Boone", "Kentucky", 1769));
            col.AddEntry(1773, "Boston Tea Party(1773)");

            col.AddEntry(1704, 1713, "Queen Anne's War");
            col.AddEntry(1754, 1763, "French Indian War");
            col.AddEntry(1775, 1783, "American Rev.(1775-1783)", PrintLocation.Left);
            col.AddEntry(1777, "NY occupied(1776)");
            col.AddEntry(1778, "Savannah occupied(1778)");
            col.AddEntry(1780, "Charleston occupied(1780)");

            var que = new Block {StartValue = 1703, EndValue = 1763, Title = "New France", Ruler = rule, Width = 22};
            que.AddEntry(1703, "Fr.settle Mobile AL");
            que.AddEntry(1718, "New Orleans LA(1718)");

            var usa = new Block {StartValue = 1775, Title = "United States", Ruler = rule};
            usa.AddEntry(1776, "Lafayette in PA(1777)");
            usa.AddEntry(1781, "Cornwallis surrender(1781)");
            usa.AddEntry(1783, "Peace of Paris(1783)");
            usa.AddEntry(new LiteraryWorkEntry("U.S.Constitution", "",1789) {StartValue = 1787});

            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            engd.AddEntry(new LeaderEntry("Anne", new int?[,] { { 1702, 1714 } }) {StartValue = 1704 });
            engd.AddEntry(new LeaderEntry("George I", new int?[,] { { 1714, 1727 } }) { StartValue = 1720 });
            engd.AddEntry(new LeaderEntry("George II", new int?[,] { { 1727, 1760 } }));
            engd.AddEntry(new ScienceAdvEntry("railroad","",1730));
            engd.AddEntry(1739, "Wesley/Whitefield[Methodism]");
            engd.AddEntry(1745, "Jacobite Rebellion(1745)");
            engd.AddEntry(new LeaderEntry("George III", new int?[,] { { 1760, 1820 } }));
            engd.AddEntry(new ScienceAdvEntry("hydrogen", "H.Cavendish", 1766));
            engd.AddEntry(new LiteraryWorkEntry("Wealth of Nations", "A.Smith", 1776) {Location = PrintLocation.Right});
            engd.AddEntry(new LiteraryWorkEntry("Rise & Fall..Roman Empire", "Gibbon", 1777) { Location = PrintLocation.Right });
            engd.AddEntry(1781, "1st Sunday-school(R.Raikes)(1781)", PrintLocation.Right);
            engd.AddEntry(new ScienceAdvEntry("steam engine", "J.Watt", 1769));
            engd.AddEntry(new ScienceAdvEntry("spinning jenny", "J.Hargreaves", 1764) {Location = PrintLocation.Right});
            engd.AddEntry(1782, "Lord North resigns as PM(1782)");
            engd.AddEntry(1787, "Australia penal settlement(1788)");
            engd.AddEntry(new ExplorerEntry("Cook", "Hawaiian Islands", 1788) {StartValue = 1789});
            engd.AddEntry(1702, 1714, "Stuart(restored)", PrintLocation.Left);
            engd.AddEntry(1715, 1800, "Hanover", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(new LeaderEntry("Louis XIV", new int?[,] { { null, 1715 } }) {StartValue = 1704});
            fr.AddEntry(new LeaderEntry("Louis XV", new int?[,] { { 1715, 1774 } }) { StartValue = 1715 });
            fr.AddEntry(new LeaderEntry("Louis XVI", new int?[,] { { 1774, 1792 } }) { StartValue = 1774 });
            fr.AddEntry(1720, "France bankrupt(1721)");
            fr.AddEntry(new LiteraryWorkEntry("The Spirit of Laws", "Montesquieu", 1748) {Location = PrintLocation.Right});
            fr.AddEntry(new LiteraryWorkEntry("Social Contract", "Rousseau", 1761) { Location = PrintLocation.Right });
            fr.AddEntry(1756, "Diplomatic Revolution(1756)(c.)");
            fr.AddEntry(new ScienceAdvEntry("hot-air ballon", "",1783) {Location = PrintLocation.Right});
            fr.AddEntry(1702, 1790, "Capetian[Bourbon]", PrintLocation.Left);
            fr.AddEntry(1701, 1714, "War of Spanish Succ.(1701-1714)");

            var de = new Block {Ruler = rule, Title = "Prussia"};
            de.AddEntry(new LeaderEntry("Fredrick I", new int?[,] { { 1701, 1713 } }) {StartValue = 1702 });
            de.AddEntry(new LeaderEntry("Fredrick William I", new int?[,] { { 1713, 1740 } }));
            de.AddEntry(new LeaderEntry("Fredrick the Great", new int?[,] { { 1740, 1786 } }));
            de.AddEntry(new LeaderEntry("Fredrick William II", new int?[,] { { 1786, 1797 } }));
            de.AddEntry(new LiteraryWorkEntry("Critique Pure Reason", "E.Kant", 1781));
            de.AddEntry(new ScienceAdvEntry("thermometer", "Fahrenheit", 1714) {StartValue = 1715});
            de.AddEntry(1730, "'Toccata Fugue' Bach(~1730s)");
            de.AddEntry(1754, 1763, "Seven's Year War");

            var aus = new Block {Ruler = rule, Title = "Austrian Empire"};
            aus.AddEntry(new LeaderEntry("Leopold I", new int?[,] { { 1640, 1705 } }) {StartValue = 1701 });
            aus.AddEntry(new LeaderEntry("Joseph I", new int?[,] { { 1705, 1711 } }));
            aus.AddEntry(new LeaderEntry("Charles III", new int?[,] { { 1711, 1740 } }));
            aus.AddEntry(new LeaderEntry("Maria Theresa", new int?[,] { { 1740, 1780 } }) { StartValue = 1739 });
            aus.AddEntry(new LeaderEntry("Joseph II", new int?[,] { { 1780, 1790 } }));
            aus.AddEntry(1783, "Mozart in Vienna", PrintLocation.Right);
            aus.AddEntry(1741, 1748, "War of Austrian Succ.(1740-1748)");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire"};
            ott.AddEntry(new LeaderEntry("Ahmed III", new int?[,] { { 1703, 1730 } }) {StartValue = 1710 });
            ott.AddEntry(new LeaderEntry("Mahmud I", new int?[,] { { 1730, 1754 } }));
            ott.AddEntry(new LeaderEntry("Osman III", new int?[,] { { 1754, 1757 } }));
            ott.AddEntry(new LeaderEntry("Mustafa III", new int?[,] { { 1757, 1774 } }));
            ott.AddEntry(new LeaderEntry("Abdulhamid I", new int?[,] { { 1774, 1789 } }));
            ott.AddEntry(new LeaderEntry("Selim III", new int?[,] { { 1789, 1807 } }));

            var rus = new Block {Ruler = rule, Title = "Imperial Russia", StartValue = 1721};
            rus.AddEntry(new LeaderEntry("Peter the Great", new int?[,] { { 1695, 1725 } }) {StartValue = 1725 });
            rus.AddEntry(1727, 1760, "[internal intrigue]");
            rus.AddEntry(new LeaderEntry("[Catherine the Great", new int?[,] { { 1762, 1796 } }));
            rus.AddEntry(new TerritoryEntry("Crimea") {StartValue = 1783});
            rus.AddEntry(1768, 1774, "1st Russio-Turkish War", PrintLocation.Right);

            amer.AddInnerBlock(col);
            amer.AddInnerBlock(usa);
            amer.AddInnerBlock(que);

            amer.AddArrow(new Arrow(que, col) { StartValue = 1706, ArrowTail = ".." });
            amer.AddArrow(new Arrow(col, que) { StartValue = 1755, Text = "B. of the Monongahela", ArrowTail = ".." });
            amer.AddArrow(new Arrow(que, col) { StartValue = 1757, Text = "massacre @ F.William Henry", ArrowTail = ".." });

            var plate = new Plate {Ruler = rule, Name = "Early Modern Age"};
            plate.AddBlock(amer);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);
            plate.FileName = "16_CE1700_1788";

            plate.AddArrow(new Arrow(aus, fr) { StartValue = 1704, Text = "B. of Blenheim(1704)" });
            plate.AddArrow(new Arrow(fr, de) { StartValue = 1706, Text = "B. of Ramilies(1706)" });
            plate.AddArrow(new Arrow(fr, de) { StartValue = 1708, Text = "B. of Oudenarde(1708)" });
            plate.AddArrow(new Arrow(engd, fr) { StartValue = 1709, Text = "B. of Malpaquet(1709)" });
            plate.AddArrow(new Arrow(aus, ott) { StartValue = 1717, Text = "B. of Belgrad(1717)" });
            plate.AddArrow(new Arrow(de, aus) { StartValue = 1740, Text = "invasion of Silesia(1740)" });
            plate.AddArrow(new Arrow(engd, fr) { StartValue = 1743, Text = "B. of Dettingen(1743)(e.)" });
            plate.AddArrow(new Arrow(de, aus) { StartValue = 1757, Text = "B. of Prague(1757)" });
            plate.AddArrow(new Arrow(rus, ott) { StartValue = 1770, Text = "B.of Chesma & Kagus(1770)" });

            plate.AddArrow(new Arrow(engd, aus)
            {
                StartValue = 1714,
                Text = "Peace of Ulrecht(1714)(a.)",
                ArrowTail = "[..",
                FromLeftToRightArrowHead = "..]",
            });
            plate.AddArrow(new Arrow(aus, de)
            {
                StartValue = 1748,
                Text = "Treaty of Aix-la-Chapelle(1748)",
                ArrowTail = "..]",
                FromRightToLeftArrowHead = "[..",
            });

            plate.AddArrow(new Arrow(engd, fr)
            {
                StartValue = 1763,
                Text = "Treaty of Paris(1763)(b.)",
                ArrowTail = "[..",
                FromLeftToRightArrowHead = "..]",
            });

            plate.AddArrow(new Arrow(de, aus)
            {
                StartValue = 1788,
                Text = "War of Bavarian Succ.(1778\\79)",
                ArrowTail = "[..",
                FromLeftToRightArrowHead = "..]",
            });

            plate.AddArrow(new Arrow(engd, usa)
            {
                StartValue = 1771,
                Text = "B. of the Alamance(1771)"
            });
            plate.AddArrow(new Arrow(engd, usa)
            {
                StartValue = 1775,
                Text = "B. of Lexington(1775)",
            });
            plate.AddArrow(new Arrow(engd, usa)
            {
                StartValue = 1777,
                Text = "B. at Trenton(1777)",
            });

            plate.Notes.Add("(a.)marks the end of Wars of Spanish Succession");
            plate.Notes.Add("(b.)British supremacy in Canada and India");
            plate.Notes.Add("(c.)England allies with Prussia - Austria with France.");
            plate.Notes.Add("(d.)start of Wars of Austrian Succ.");
            plate.Notes.Add("(e.)fought on German soil");


            return plate;
        }
    }
}
