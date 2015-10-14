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
            usa.AddEntry(1787, "+DE,PA,NJ(1787)",PrintLocation.Right);
            usa.AddEntry(1788, "+GA,CT,MA,MD,SC,NH,VA,NY(1788)", PrintLocation.Right);
            usa.AddEntry(1789, "[Washington 1789-1797]", PrintLocation.Left);
            usa.AddEntry(1789, "+NC(1789)", PrintLocation.Right);
            usa.AddEntry(1790, "+RI(1790)", PrintLocation.Right);
            usa.AddEntry(1791, "+VT(1791)", PrintLocation.Right);
            usa.AddEntry(1792, "+KY(1792)", PrintLocation.Right);
            usa.AddEntry(1793, "E.Whitney[cottin gin](1793)(d.)");
            usa.AddEntry(1796, "+TN(1796)", PrintLocation.Right);
            usa.AddEntry(1797, "[John Adams -1801]", PrintLocation.Left);
            usa.AddEntry(1801, "[T.Jefferson -1809]", PrintLocation.Left);
            usa.AddEntry(1802, "West Point founded(1802)");
            usa.AddEntry(1803, "Louisiana Purchase(1803)", PrintLocation.Left);
            usa.AddEntry(1803, "+OH(1803)", PrintLocation.Right);
            usa.AddEntry(1804, "Lewis & Clark Expedition(1804)");
            usa.AddEntry(1809, "[James Madison -1817]", PrintLocation.Left);
            usa.AddEntry(1811, "B.of Tippecanoe(1811)(h.)");
            usa.AddEntry(1812, "+LA(1812)", PrintLocation.Right);
            usa.AddEntry(1815, "failed Canadian conquest");
            usa.AddEntry(1816, "+IN(1816)", PrintLocation.Right);
            usa.AddEntry(1816, "First Seminole War(1816-1819)", PrintLocation.Left);
            usa.AddEntry(1817, "[James Monroe -1825]", PrintLocation.Left);
            usa.AddEntry(1817, "+MS(1817)", PrintLocation.Right);
            usa.AddEntry(1818, "+IL(1818)", PrintLocation.Right);
            usa.AddEntry(1819, "+AL(1819)", PrintLocation.Right);
            usa.AddEntry(1820, "+ME(1820)", PrintLocation.Right);
            usa.AddEntry(1821, "+MO(1821)", PrintLocation.Right);
            usa.AddEntry(1825, "[John Quincy Adams -1829]", PrintLocation.Left);
            usa.AddEntry(1829, "[Andrew Jackson -1837]", PrintLocation.Left);
            usa.AddEntry(1835, "Second Seminole War(1835-1842)", PrintLocation.Left);
            usa.AddEntry(1836, "+AR(1836)", PrintLocation.Right);
            usa.AddEntry(1837, "[Martin Van Buren -1841]", PrintLocation.Left);
            usa.AddEntry(1837, "+MI(1837)", PrintLocation.Right);
            usa.AddEntry(1841, "[William H Harrison -1841]", PrintLocation.Left);
            usa.AddEntry(1842, "[John Tyler -1845]", PrintLocation.Left);
            usa.AddEntry(1845, "[James K Polk -1849]", PrintLocation.Left);
            usa.AddEntry(1845, "+FL,TX(1845)", PrintLocation.Right);
            usa.AddEntry(1846, "+IA(1846)", PrintLocation.Right);
            usa.AddEntry(1848, "+WI(1848)",PrintLocation.Right);
            usa.AddEntry(1849, "[Zachary Taylor -1850]", PrintLocation.Left);
            usa.AddEntry(1850, "[Millard Fillmore -1853]", PrintLocation.Left);
            usa.AddEntry(1850, "+CA(1850)", PrintLocation.Right);
            usa.AddEntry(1853, "[Franklin Pierce -1857]", PrintLocation.Left);
            usa.AddEntry(1852, "Perry Expedition to Japan(1852-54)");
            usa.AddEntry(1855, "Third Seminole War(1855-1858)", PrintLocation.Left);
            usa.AddEntry(1857, "[James Buchanan -1861]", PrintLocation.Left);
            usa.AddEntry(1858, "+MN(1858)", PrintLocation.Right);
            usa.AddEntry(1859, "+OR(1859)", PrintLocation.Right);
            usa.AddEntry(1860, "[Abraham Lincoln -1865]", PrintLocation.Left);
            usa.AddEntry(1861, "+KS(1861)", PrintLocation.Right);

            usa.AddEntry(1861, "Ft.Sumter fired on(e.)", PrintLocation.Left);
            usa.AddEntry(1863, "Lee's invasion of PA,", PrintLocation.Left);
            usa.AddEntry(1863, "Siege of Vicksburg, +WV(1863)", PrintLocation.Right);
            usa.AddEntry(1864, "Siege of Petersburg,",PrintLocation.Left);
            usa.AddEntry(1864, "Atlanta falls, +NV(1864)", PrintLocation.Right);
            usa.AddEntry(1865, "Richmond falls,",PrintLocation.Left);
            usa.AddEntry(1865, "surrender @ Appomattox(1865)(f.)", PrintLocation.Right);
            usa.AddEntry(1866, "Lincoln assassinated(1865)");


            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            engd.AddEntry(1798, "Jenner[vaccination](1798)");
            engd.AddEntry(1801, "[William Pitt -1801\\-1806]", PrintLocation.Left);
            engd.AddEntry(1804, "[Henry Addington -1804]", PrintLocation.Left);
            engd.AddEntry(1806, "[Baron Grenville -1807]", PrintLocation.Left);
            engd.AddEntry(1807, "[W. Cavendish-Bentinck -1809]", PrintLocation.Left);
            engd.AddEntry(1809, "[Spencer Perceval -1812]", PrintLocation.Left);
            engd.AddEntry(1812, "[R.B Jenkinson -1827]", PrintLocation.Left);
            engd.AddEntry(1826, "[G. Canning -1827]", PrintLocation.Left);
            engd.AddEntry(1827, "[Viscount Goderich -1828]", PrintLocation.Left);
            engd.AddEntry(1828, "[Arthur Wellesley -1830]", PrintLocation.Left);
            engd.AddEntry(1830, "[Charles Grey -1834]", PrintLocation.Left);
            engd.AddEntry(1836, "[R.Peel & W.Lamb(a.) -1846]", PrintLocation.Left);
            engd.AddEntry(1846, "[John Russel -1852]", PrintLocation.Left);
            engd.AddEntry(1848, "'Communist Manifesto'(1848)");
            engd.AddEntry(1855, "[G. Hamilton-Gordon -1852-55]", PrintLocation.Left);
            engd.AddEntry(1858, "[E. Smith-Stanley -1852\\1858-59]", PrintLocation.Left);
            engd.AddEntry(1860, "[H. John Temple 1855-58\\59-65]", PrintLocation.Left);
            engd.AddEntry(1833, "'Principles of Geology' Lyell(1833)", PrintLocation.Right);
            engd.AddEntry(1834, "Abolition Act(1833)", PrintLocation.Right);
            engd.AddEntry(1859, "'Origin of Species' Darwin(1859)", PrintLocation.Right);

            engd.AddEntry(1840, 1860, "Industrial Revolution", PrintLocation.Right);
            engd.AddEntry(1837, 1868, "Queen Victoria (1837-1901)");

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(1789, "Estates-General meets(1789)(b.)", PrintLocation.Left);
            fr.AddEntry(1791, "Storming of the Bastille(1789)", PrintLocation.Left);
            fr.AddEntry(1792, "Louis XVI executed(1793)", PrintLocation.Left);
            fr.AddEntry(1793, "Reign of Terror(1793\\94)", PrintLocation.Left);
            fr.AddEntry(1794, "Robespierre executed(1794)", PrintLocation.Left);
            fr.AddEntry(1795, "Napolean quells insurrection(1795)", PrintLocation.Left);
            fr.AddEntry(1848, "revolution of 1848 & 2nd Republic ");

            fr.AddEntry(1787, 1792, "French Revolution", PrintLocation.Right);
            fr.AddEntry(1793, 1804, "1st Republic", PrintLocation.Right);
            fr.AddEntry(1804, 1814, "Napolean(French Empire)");
            fr.AddEntry(1815, 1830, "Bourbon Restoration", PrintLocation.Right);
            fr.AddEntry(1831, 1848, "July Monarchy", PrintLocation.Right);
            fr.AddEntry(1852, 1866, "2nd French Empire", PrintLocation.Right);
            


            var de = new Block {Ruler = rule, Title = "Prussia"};
            de.AddEntry(1848, "revolution of 1848");
            de.AddEntry(1789, "[Frederick William II 1786-1797]", PrintLocation.Left);
            de.AddEntry(1795, "[Frederick William III 1797-1840]", PrintLocation.Left);
            de.AddEntry(1840, "[Frederick William IV 1840-1861]", PrintLocation.Left);
            de.AddEntry(1867, "'Capital' Marx(1867)");

            var aus = new Block { Ruler = rule, Title = "Austrian Empire" };
            aus.AddEntry(1788, "[Joseph II 1765-1790]", PrintLocation.Left);
            aus.AddEntry(1790, "[Leopold VII 1790-1792]", PrintLocation.Left);
            aus.AddEntry(1791, "[Francis II 1792-1804]", PrintLocation.Left);
            aus.AddEntry(1804, "[Francis I 1804-1835]", PrintLocation.Left);
            aus.AddEntry(1835, "[Ferdinand I 1835-1848]", PrintLocation.Left);
            aus.AddEntry(1847, "[Francis Joseph 1848-1916]", PrintLocation.Left);
            aus.AddEntry(1815,"Congress of Vienna(1815)");
            aus.AddEntry(1848, "revolution of 1848");

            var ott = new Block { Ruler = rule, Title = "Ottoman Empire" };

            var rus = new Block { Ruler = rule, Title = "Imperial Russia"};
            rus.AddEntry(1796,"[Paul I 1796-1801]", PrintLocation.Left);
            rus.AddEntry(1801,"[Alexander I 1801-1825]",PrintLocation.Left);
            rus.AddEntry(1825, "Decembrist revolt(1825)");
            rus.AddEntry(1824, "[Nicholas I 1825-1855]", PrintLocation.Left);
            rus.AddEntry(1855, "[Alexander II 1855-1881]",PrintLocation.Left);

            rus.AddEntry(1860, "Vladivostok founded(1860)");
            rus.AddEntry(1861, "serfs freed(1861)");

            var plate = new Plate { Ruler = rule, Name = "Modern Age(1)" };
            plate.AddBlock(usa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);

            plate.AddArrow(new Arrow(aus, ott){Text = "B.of Focsani(1789)", StartValue = 1789});
            plate.AddArrow(new Arrow(rus, ott) { StartValue = 1789, FromRightToLeftArrowHead = "<......" });
            plate.AddArrow(new Arrow(aus, fr) {StartValue = 1792 });
            plate.AddArrow(new Arrow(de, fr) { StartValue = 1793 });
            plate.AddArrow(new Arrow(fr, aus){StartValue = 1796, Text = "B.of Rivoli(1797)"});
            plate.AddArrow(new Arrow(fr, ott) {Text = "B.of the Pryamids(1798)(c.)" ,StartValue = 1797});
            plate.AddArrow(new Arrow(engd, fr){Text = "B.of the Nile(1798)(g.)", StartValue = 1798});
            plate.AddArrow(new Arrow(fr, aus){Text = "B.of Marengo(1800)", StartValue = 1800});
            plate.AddArrow(new Arrow(rus, fr){Text = "B.of Leipzig(1813)",StartValue = 1813});
            plate.AddArrow(new Arrow(engd, usa){Text = "DC sacked(1814)", StartValue = 1814});
            plate.AddArrow(new Arrow(engd, fr){Text = "Waterloo(1815)", StartValue = 1815});
            plate.AddArrow(new Arrow(fr, rus){StartValue = 1812});

            plate.Notes.Add("(a.)  last Prime Minister dismissed by a king,  (b.) last met in 1614");
            plate.Notes.Add("(c.)  intended to cut England off from India, (d.) pop. of black slaves goes from 700 thousand to 3 million");
            plate.Notes.Add("(e.)  April 12, 1861, (f.) April 9, 1865 (est. 620,000 killed)");
            plate.Notes.Add("(g.)  Napolean's Navy destroyed,  (h.) Gen. Harrison defeats confederacy of Native Americans");

            return plate;
        }
    }
}
