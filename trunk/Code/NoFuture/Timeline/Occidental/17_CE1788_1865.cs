using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1788to1865()
        {
            var rule = new Rule { StartValue = 1785, EndValue = 1867, RuleLineSpacing = 7 };

            var usa = new Block {Ruler = rule, Title = "United States"};
            usa.AddEntry(1789, "[Washington 1789-1797]", PrintLocation.Left);
            usa.AddEntry(1797, "[John Adams -1801]", PrintLocation.Left);
            usa.AddEntry(1801, "[T.Jefferson -1809]", PrintLocation.Left);
            usa.AddEntry(1809, "[James Madison -1817]", PrintLocation.Left);
            usa.AddEntry(1817, "[James Monroe -1825]", PrintLocation.Left);
            usa.AddEntry(1825, "[John Quincy Adams -1829]", PrintLocation.Left);
            usa.AddEntry(1829, "[Andrew Jackson -1837]", PrintLocation.Left);
            usa.AddEntry(1837, "[Martin Van Buren -1841]", PrintLocation.Left);
            usa.AddEntry(1841, "[William H Harrison -1841]", PrintLocation.Left);
            usa.AddEntry(1842, "[John Tyler -1845]", PrintLocation.Left);
            usa.AddEntry(1845, "[James K Polk -1849]", PrintLocation.Left);
            usa.AddEntry(1849, "[Zachary Taylor -1850]", PrintLocation.Left);
            usa.AddEntry(1850, "[Millard Fillmore -1853]", PrintLocation.Left);
            usa.AddEntry(1853, "[Franklin Pierce -1857]", PrintLocation.Left);
            usa.AddEntry(1857, "[James Buchanan -1861]", PrintLocation.Left);
            usa.AddEntry(1861, "[Abraham Lincoln -1865]", PrintLocation.Left);

            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            engd.AddEntry(1789, "[William Pitt -1801\\-1806]", PrintLocation.Left);
            engd.AddEntry(1801, "[Henry Addington -1804]", PrintLocation.Left);
            engd.AddEntry(1806, "[Baron Grenville -1807]", PrintLocation.Left);
            engd.AddEntry(1807, "[W. Cavendish-Bentinck -1809]", PrintLocation.Left);
            engd.AddEntry(1809, "[Spencer Perceval -1812]", PrintLocation.Left);
            engd.AddEntry(1812, "[R.B Jenkinson -1827]", PrintLocation.Left);
            engd.AddEntry(1826, "[G. Canning -1827]", PrintLocation.Left);
            engd.AddEntry(1827, "[Viscount Goderich -1828]", PrintLocation.Left);
            engd.AddEntry(1828, "[Arthur Wellesley -1830]", PrintLocation.Left);
            engd.AddEntry(1830, "[Charles Grey -1834]", PrintLocation.Left);
            engd.AddEntry(1834, "[R.Peel & W.Lamb* -1846]", PrintLocation.Left);
            engd.AddEntry(1846, "[John Russel -1852]", PrintLocation.Left);
            engd.AddEntry(1855, "[G. Hamilton-Gordon -1852-55]", PrintLocation.Left);
            engd.AddEntry(1858, "[E. Smith-Stanley -1852\\1858-59]", PrintLocation.Left);
            engd.AddEntry(1860, "[H. John Temple 1855-58\\59-65]", PrintLocation.Left);


            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(1789, "Estates-General meets(1789)**");
            fr.AddEntry(1791, "Storming of the Bastille(1789)");
            fr.AddEntry(1792, "Louis XVI executed(1793)");
            fr.AddEntry(1793, "Reign of Terror(1793\\94)");
            fr.AddEntry(1794, "Robespierre executed(1794)");
            fr.AddEntry(1795, "Napolean quells insurrection(1795)");
            var de = new Block {Ruler = rule, Title = "Prussia"};

            var aus = new Block { Ruler = rule, Title = "Austrian Empire" };

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire" };

            var rus = new Block { Ruler = rule, Title = "Imperial Russia"};

            var plate = new Plate { Ruler = rule, Name = "Modern Age(1)" };
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);

            plate.AddArrow(new Arrow(aus, ott){Text = "B.of Focsani(1789)"});
            plate.AddArrow(new Arrow(rus, ott));

            plate.Notes.Add("(*)   last Prime Minister dismissed by a king");
            plate.Notes.Add("(**)  last met in 1614");


            return plate;
        }
    }
}
