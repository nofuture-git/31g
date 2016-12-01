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
            col.AddEntry(1727, "[Bering] Bering straight(1728)");
            col.AddEntry(1739, "Stono Rebellion(1739)");
            col.AddEntry(1730, "Baltimore MA(1730)");
            col.AddEntry(1764, "Mason Dixon survey(1764)");
            col.AddEntry(1769, "[D.Boone]-Kentucky(1769)");
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
            usa.AddEntry(1787, "U.S.Constitution(1787)");
            amer.AddInnerBlock(col);
            amer.AddInnerBlock(usa);
            amer.AddInnerBlock(que);
            amer.AddArrow(new Arrow(que, col) {StartValue = 1706, ArrowTail = "."});
            amer.AddArrow(new Arrow(col, que) {StartValue = 1755, Text = "B. of the Monongahela"});
            amer.AddArrow(new Arrow(que, col) {StartValue = 1757, Text = "massacre @ F.William Henry"});

            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            engd.AddEntry(1704, "[Anne 1702-1714]", PrintLocation.Left);
            engd.AddEntry(1720, "[George I 1714-1727]", PrintLocation.Left);
            engd.AddEntry(1727, "[George II 1727-1760]", PrintLocation.Left);
            engd.AddEntry(1730, "first railroad(1730)");
            engd.AddEntry(1739, "Wesley/Whitefield[Methodism]");
            engd.AddEntry(1745, "Jacobite Rebellion(1745)");
            engd.AddEntry(1760, "[George III 1760-1820]", PrintLocation.Left);
            engd.AddEntry(1766, "H.Cavendish[hydrogen](1766)", PrintLocation.Right);
            engd.AddEntry(1776, "'Wealth of Nations' A.Smith(1776)", PrintLocation.Right);
            engd.AddEntry(1777, " 'Rise & Fall..Roman Empire'Gibbon(1777)", PrintLocation.Right);
            engd.AddEntry(1781, "1st Sunday-school(R.Raikes)(1781)", PrintLocation.Right);
            engd.AddEntry(1769, "J.Watt[steam engine](1769)", PrintLocation.Right);
            engd.AddEntry(1764, "J.Hargreaves[spinning jenny](1764)", PrintLocation.Right);
            engd.AddEntry(1782, "Lord North resigns as PM(1782)");
            engd.AddEntry(1787, "Australia penal settlement(1788)");
            engd.AddEntry(1789, "[Cook]Hawaiian Islands(1788)");
            engd.AddEntry(1702, 1714, "Stuart(restored)", PrintLocation.Left);
            engd.AddEntry(1715, 1800, "Hanover", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(1601, "[Louis XIV 1643-1715]", PrintLocation.Left);
            fr.AddEntry(1715, "[Louis XV 1715-1774]", PrintLocation.Left);
            fr.AddEntry(1774, "[Louis XVI 1774-1792]", PrintLocation.Left);
            fr.AddEntry(1720, "France bankrupt(1721)");
            fr.AddEntry(1748, "'The Spirit of Laws' Montesquieu(1748)", PrintLocation.Right);
            fr.AddEntry(1761, "'Social Contract' Rousseau(1761)", PrintLocation.Right);
            fr.AddEntry(1756, "Diplomatic Revolution(1756)(c.)");
            fr.AddEntry(1783, "1st hot-air ballon(1783)", PrintLocation.Right);
            fr.AddEntry(1702, 1790, "Capetian[Bourbon]", PrintLocation.Left);
            fr.AddEntry(1701, 1714, "War of Spanish Succ.(1701-1714)");

            var de = new Block {Ruler = rule, Title = "Prussia"};
            de.AddEntry(1702, "[Fredrick I 1701-1713]", PrintLocation.Left);
            de.AddEntry(1713, "[Fredrick William I 1713-1740]", PrintLocation.Left);
            de.AddEntry(1740, "[Fredrick the Great 1740-1786]", PrintLocation.Left);
            de.AddEntry(1786, "[Fredrick William II 1786-1797]", PrintLocation.Left);
            de.AddEntry(1781, "'Critique of Pure Reason' E.Kant(1781)", PrintLocation.Right);
            de.AddEntry(1715, "Fahrenheit[thermometer](1714)");
            de.AddEntry(1730, "'Toccata Fugue' Bach(~1730s)");
            de.AddEntry(1754, 1763, "Seven's Year War");

            var aus = new Block {Ruler = rule, Title = "Austrian Empire"};
            aus.AddEntry(1701, "[Leopold I 1640-1705]", PrintLocation.Left);
            aus.AddEntry(1705, "[Joseph I 1705-1711]", PrintLocation.Left);
            aus.AddEntry(1711, "[Charles III 1711-1740]", PrintLocation.Left);
            aus.AddEntry(1739, "[Maria Theresa 1740-1780]", PrintLocation.Left);
            aus.AddEntry(1780, "[Joseph II 1780-1790]", PrintLocation.Left);
            aus.AddEntry(1783, "Mozart in Vienna", PrintLocation.Right);
            aus.AddEntry(1741, 1748, "War of Austrian Succ.(1740-1748)");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire"};
            ott.AddEntry(1710, "[Ahmed III 1703-1730]", PrintLocation.Left);
            ott.AddEntry(1730, "[Mahmud I 1730-1754]", PrintLocation.Left);
            ott.AddEntry(1754, "[Osman III 1754-1757]", PrintLocation.Left);
            ott.AddEntry(1757, "[Mustafa III 1757-1774]", PrintLocation.Left);
            ott.AddEntry(1774, "[Abdulhamid I 1774-1789]", PrintLocation.Left);
            ott.AddEntry(1789, "[Selim III 1789-1807]", PrintLocation.Left);

            var rus = new Block {Ruler = rule, Title = "Imperial Russia", StartValue = 1721};
            rus.AddEntry(1725, "[Peter the Great 1695-1725]");
            rus.AddEntry(1727, 1760, "[internal intrigue]");
            rus.AddEntry(1762, "[Catherine theGreat 1762-1796]");
            rus.AddEntry(1783, "(+)Crimea(1783)");
            rus.AddEntry(1768, 1774, "1st Russio-Turkish War", PrintLocation.Right);


            var plate = new Plate {Ruler = rule, Name = "Early Modern Age(2)"};
            plate.AddBlock(amer);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);

            plate.AddArrow(new Arrow(aus, fr) {StartValue = 1704, Text = "B. of Blenheim(1704)"});
            plate.AddArrow(new Arrow(fr, de) {StartValue = 1706, Text = "B. of Ramilies(1706)"});
            plate.AddArrow(new Arrow(fr, de) {StartValue = 1708, Text = "B. of Oudenarde(1708)"});
            plate.AddArrow(new Arrow(engd, fr) {StartValue = 1709, Text = "B. of Malpaquet(1709)"});
            plate.AddArrow(new Arrow(aus, ott) {StartValue = 1717, Text = "B. of Belgrad(1717)"});
            plate.AddArrow(new Arrow(de, aus) {StartValue = 1740, Text = "invasion of Silesia(1740)"});
            plate.AddArrow(new Arrow(engd, fr) {StartValue = 1743, Text = "B. of Dettingen(1743)(e.)"});
            plate.AddArrow(new Arrow(de, aus) {StartValue = 1757, Text = "B. of Prague(1757)"});
            plate.AddArrow(new Arrow(rus, ott) {StartValue = 1770, Text = "B.of Chesma & Kagus(1770)"});

            plate.AddArrow(new Arrow(engd, aus)
            {
                StartValue = 1714,
                Text = "Peace of Ulrecht(1714)(a.)",
                ArrowTail = "[..........",
                FromLeftToRightArrowHead = "......]",
            });
            plate.AddArrow(new Arrow(aus, de)
            {
                StartValue = 1748,
                Text = "Treaty of Aix-la-Chapelle(1748)",
                ArrowTail = "........]",
                FromRightToLeftArrowHead = "[.....",
            });

            plate.AddArrow(new Arrow(engd, fr)
            {
                StartValue = 1763,
                Text = "Treaty of Paris(1763)(b.)",
                ArrowTail = "[..........",
                FromLeftToRightArrowHead = "......]",
            });

            plate.AddArrow(new Arrow(de, aus)
            {
                StartValue = 1788,
                Text = "War of Bavarian Succ.(1778\\79)",
                ArrowTail = "[.........",
                FromLeftToRightArrowHead = "]",
            });

            plate.AddArrow(new Arrow(engd, amer)
            {
                StartValue = 1771,
                Text = "B. of the Alamance(1771)",
                FromRightToLeftArrowHead = "<..............................."
            });
            plate.AddArrow(new Arrow(engd, amer)
            {
                StartValue = 1775,
                Text = "B. of Lexington(1775)",
                FromRightToLeftArrowHead = "<........................",
                ArrowTail = "."
            });
            plate.AddArrow(new Arrow(engd, amer)
            {
                StartValue = 1777,
                Text = "B. at Trenton(1777)",
                FromRightToLeftArrowHead = "<........................",
                ArrowTail = "."
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
