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
            var rule = new Rule {StartValue = 1785, EndValue = 1868, RuleLineSpacing = 7};
            var plate = new Plate { Ruler = rule, Name = "Modern Age(1)" };

            var usa = new Block {Ruler = rule, Title = "United States"};
            usa.AddEntry(new TerritoryEntry("DE,PA,NJ", 1787) {Location = PrintLocation.Right});
            usa.AddEntry(new TerritoryEntry("GA,CT,MA,MD,SC,NH,VA,NY", 1788) {Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("Washington", new int?[,] { { 1789, 1797 } }) {StartValue = 1789 });
            usa.AddEntry(new TerritoryEntry("NC", 1789) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("RI", 1790) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("VT", 1791) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("KY", 1792) { Location = PrintLocation.Right });
            usa.AddEntry(new ScienceAdvEntry("cottin gin(d.)", "E.Whitney", 1793));
            usa.AddEntry(new TerritoryEntry("TN", 1796) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("John Adams", new int?[,] { { 1797, 1801 } }) { StartValue = 1797 });
            usa.AddEntry(new LeaderEntry("T.Jefferson", new int?[,] { { 1801, 1809 } }) { StartValue = 1801 });
            usa.AddEntry(1802, "West Point founded(1802)");
            usa.AddEntry(1803, "Louisiana Purchase(1803)");
            usa.AddEntry(new TerritoryEntry("OH", 1803) { Location = PrintLocation.Right });
            usa.AddEntry(new ExplorerEntry("Lewis & Clark", "Expedition", 1804) {Location = PrintLocation.Center});
            usa.AddEntry(new LeaderEntry("[James Madison", new int?[,] { { 1809, 1817 } }) { StartValue = 1809 });
            usa.AddEntry(1811, "B.of Tippecanoe(1811)(h.)");
            usa.AddEntry(new TerritoryEntry("LA", 1812) { Location = PrintLocation.Right });
            usa.AddEntry(1815, "failed Canadian conquest");
            usa.AddEntry(new TerritoryEntry("IN", 1816) { Location = PrintLocation.Right });
            usa.AddEntry(1816, "First Seminole War(1816-1819)");
            usa.AddEntry(new LeaderEntry("James Monroe", new int?[,] { { 1817, 1825 } }) { StartValue = 1817 });
            usa.AddEntry(new TerritoryEntry("MS", 1817) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("IL", 1818) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("AL", 1819) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("ME", 1820) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("MO", 1821) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("John Quincy Adams", new int?[,] { { 1825, 1829 } }) { StartValue = 1825 });
            usa.AddEntry(new LiteraryWorkEntry("Last of the Mohicans", "J.F.Cooper", 1826));
            usa.AddEntry(new LeaderEntry("Andrew Jackson", new int?[,] { { 1829, 1837 } }) { StartValue = 1829 });
            usa.AddEntry(1835, "Second Seminole War(1835-1842)");
            usa.AddEntry(new TerritoryEntry("AR", 1836) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("Martin Van Buren", new int?[,] { { 1837, 1841 } }) { StartValue = 1837 });
            usa.AddEntry(new ScienceAdvEntry("telegraph", "Morse", 1837) {StartValue = 1839});
            usa.AddEntry(new TerritoryEntry("MI", 1837) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("William H Harrison", new int?[,] { { null, 1841 } }) { StartValue = 1841 });
            usa.AddEntry(new LeaderEntry("John Tyler", new int?[,] { { 1842, 1845 } }) { StartValue = 1842 });
            usa.AddEntry(new LeaderEntry("James K Polk", new int?[,] { { 1845, 1849 } }) { StartValue = 1845 });
            usa.AddEntry(new TerritoryEntry("FL,TX", 1845) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("IA", 1846) { Location = PrintLocation.Right });
            usa.AddEntry(new ScienceAdvEntry("anesthesia", "W.Morton", 1846) { StartValue = 1844 });
            usa.AddEntry(1847, "U.S.-Mexican War(1846-1848)");
            usa.AddEntry(new TerritoryEntry("WI", 1848) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("Zachary Taylor", new int?[,] { { 1849, 1850 } }) { StartValue = 1849 });
            usa.AddEntry(new LiteraryWorkEntry("The Scarlet Letter", "N.Hawthorne", 1849) {Location = PrintLocation.Right});
            usa.AddEntry(new LeaderEntry("Millard Fillmore", new int?[,] { { 1850, 1853 } }) { StartValue = 1850 });
            usa.AddEntry(new TerritoryEntry("CA", 1850) { Location = PrintLocation.Right });
            usa.AddEntry(new LiteraryWorkEntry("Moby-Dick", "H.Melville", 1851) {Location = PrintLocation.Right });
            usa.AddEntry(new LiteraryWorkEntry("Uncle Tom's Cabin", "H.B.Stowe", 1852) { StartValue = 1851, Location = PrintLocation.Right });
            usa.AddEntry(1852, "Perry Expedition to Japan(1852-54)");
            usa.AddEntry(new LeaderEntry("Franklin Pierce", new int?[,] { { 1853, 1857 } }) { StartValue = 1853 });
            usa.AddEntry(1855, "Third Seminole War(1855-1858)");
            usa.AddEntry(new LeaderEntry("James Buchanan", new int?[,] { { 1857, 1861 } }) { StartValue = 1857 });
            usa.AddEntry(new TerritoryEntry("MN", 1858) { Location = PrintLocation.Right });
            usa.AddEntry(new TerritoryEntry("OR", 1859) { Location = PrintLocation.Right });
            usa.AddEntry(new LeaderEntry("Abraham Lincoln", new int?[,] { { 1861, 1865 } }) { StartValue = 1860 });
            usa.AddEntry(new TerritoryEntry("KS", 1861) { Location = PrintLocation.Left });
            usa.AddEntry(new TerritoryEntry("WV", 1863) { Location = PrintLocation.Left });
            usa.AddEntry(new TerritoryEntry("NV", 1864) { Location = PrintLocation.Left });

            var csa = new Block { Ruler = rule, Title = "Confederate States", StartValue = 1860,EndValue = 1866, Width = 48};
            csa.AddEntry(1861, "New Orleans captured");
            csa.AddEntry(1863, "Vicksburg\\Petersburg under seige");
            csa.AddEntry(1864,"Atlanta razed(1864)", PrintLocation.Right);
            csa.AddEntry(1865, "fall of Richmond", PrintLocation.Left);
            csa.AddEntry(1865, "surrender @ Appomattox(1865)(f.)", PrintLocation.Right);
            plate.Arrows.Add(new Arrow(csa, usa) {StartValue = 1861, Text = "Ft.Sumter fired on(e.)" });
            plate.Arrows.Add(new Arrow(usa, csa) {StartValue = 1863, FromLeftToRightArrowHead = "......>"});
            plate.Arrows.Add(new Arrow(csa, usa) { StartValue = 1863, Text = "Lee's invasion of PA", ArrowTail = ".." });
            plate.Arrows.Add(new Arrow(usa,csa) {StartValue = 1864,Text = "Sherman's March" });
            usa.AddEntry(1866, "Lincoln assassinated(1865)");

            usa.AddEntry(new ScienceAdvEntry("rotary press", "R.M.Hoe") {StartValue = 1843});
            usa.AddEntry(1836, 1845, "Republic of Texas");

            var engd = new Block {Ruler = rule, Title = "United Kingdom"};
            engd.AddEntry(new ScienceAdvEntry("vaccination", "Jenner", 1798) {Location = PrintLocation.Left});
            engd.AddEntry(new ScienceAdvEntry("battery", "Volta", 1800) {Location = PrintLocation.Left });
            engd.AddEntry(new LeaderEntry("William Pitt", new int?[,] { {1783, 1801 }, {1804, 1806 } }) {StartValue = 1786 });
            engd.AddEntry(new ScienceAdvEntry("atomic theory", "Dalton", 1803));
            engd.AddEntry(new LeaderEntry("Henry Addington", new int?[,] { {1801, 1804} }));
            engd.AddEntry(new LeaderEntry("Baron Grenville", new int?[,] { { 1804, 1807 } }));
            engd.AddEntry(new LeaderEntry("W. Cavendish-Bentinck", new int?[,] { { 1807, 1809 } }));
            engd.AddEntry(new LeaderEntry("Spencer Perceval", new int?[,] { { 1809, 1812 } }));
            engd.AddEntry(new LeaderEntry("R.B Jenkinson", new int?[,] { { 1812, 1827 } }));
            engd.AddEntry(new LiteraryWorkEntry("Pride & Prejudice", "J.Austen", 1813) {StartValue = 1815, Location = PrintLocation.Left});
            engd.AddEntry(new LeaderEntry("G. Canning", new int?[,] { { 1826, 1827 } }));
            engd.AddEntry(new LeaderEntry("Viscount Goderich", new int?[,] { { 1827, 1828 } }));
            engd.AddEntry(new LeaderEntry("Arthur Wellesley", new int?[,] { { 1828, 1830 } }));
            engd.AddEntry(new LeaderEntry("Charles Grey", new int?[,] { { 1830, 1834 } }));
            engd.AddEntry(new ScienceAdvEntry("law of induction", "Faraday", 1831));
            engd.AddEntry(new LeaderEntry("R.Peel & W.Lamb(a.)", new int?[,] { { 1836, 1846 } }));
            engd.AddEntry(new ScienceAdvEntry("1st law thermodyn", "J.P.Joule", 1843));
            engd.AddEntry(new ExplorerEntry("Franklin", "lost expedition", 1845) {StartValue = 1844});
            engd.AddEntry(new LeaderEntry("John Russel", new int?[,] { { 1846, 1852 } }));
            engd.AddEntry(new ScienceAdvEntry("Kelvin", "absolute zero", 1848) {Location = PrintLocation.Left});
            engd.AddEntry(new LiteraryWorkEntry("Communist Manifesto", "", 1848) { Location = PrintLocation.Right});
            engd.AddEntry(new LeaderEntry("G. Hamilton-Gordon", new int?[,] { { 1852, 1855 } }) { StartValue = 0 });
            engd.AddEntry(new LeaderEntry("E. Smith-Stanley", new int?[,] { { 1858, 1859 } }) { StartValue = 0 });
            engd.AddEntry(new LeaderEntry("H. John Temple", new int?[,] { { 1855, 1858 }, { 1859, 1865 } }) { StartValue = 1860 });
            engd.AddEntry(new LiteraryWorkEntry("Principles of Geology", "Lyell", 1833) {Location = PrintLocation.Right });
            engd.AddEntry(1834, "Abolition Act(1833)", PrintLocation.Right);
            engd.AddEntry(new LiteraryWorkEntry("Oliver Twist", "Dickens", 1838));
            engd.AddEntry(new LiteraryWorkEntry("Origin of Species", "Darwin", 1859) { Location = PrintLocation.Right });
            engd.AddEntry(new LiteraryWorkEntry("Great Expectations", "Dickens", 1861));
            engd.AddEntry(new ScienceAdvEntry("electromag.", "Maxwell") {StartValue = 1864});
            engd.AddEntry(1865, "First Subway(1864)");
            engd.AddEntry(1840, 1860, "Industrial Revolution", PrintLocation.Right);
            engd.AddEntry(1839, 1866, "Queen Victoria (1837-1901)", PrintLocation.Left);

            var fr = new Block {Ruler = rule, Title = "France"};
            fr.AddEntry(1789, "Estates-General meets(1789)(b.)", PrintLocation.Left);
            fr.AddEntry(1791, "Storming of the Bastille(1789)", PrintLocation.Left);
            fr.AddEntry(1792, "Louis XVI executed(1793)", PrintLocation.Left);
            fr.AddEntry(1793, "Reign of Terror(1793\\94)", PrintLocation.Left);
            fr.AddEntry(1794, "Robespierre executed(1794)", PrintLocation.Left);
            fr.AddEntry(1795, "Napolean quells insurrection(1795)", PrintLocation.Left);
            fr.AddEntry(new ScienceAdvEntry("extinction", "Cuvier", 1796));
            fr.AddEntry(new ScienceAdvEntry("heat engine", "Carnot", 1824));
            fr.AddEntry(new ScienceAdvEntry("enzymes", "A.Payen", 1833));
            fr.AddEntry(new LiteraryWorkEntry("Count of Monte Cristo", "A.Dumas", 1845));
            fr.AddEntry(1848, "revolution of 1848 & 2nd Republic ");
            fr.AddEntry(new ScienceAdvEntry("germ theory", "Pasteur", 1861));
            fr.AddEntry(new LiteraryWorkEntry("Les Miserables", "V.Hugo", 1862));
            fr.AddEntry(1787, 1792, "French Revolution", PrintLocation.Right);
            fr.AddEntry(1793, 1804, "1st Republic", PrintLocation.Right);
            fr.AddEntry(new LeaderEntry("Napolean", new int?[,] { { 1804, 1814 } }) {Location = PrintLocation.Left});
            fr.AddEntry(1804, 1814, "(French Empire)", PrintLocation.Right);
            fr.AddEntry(1815, 1830, "Bourbon Restoration", PrintLocation.Right);
            fr.AddEntry(1831, 1848, "July Monarchy", PrintLocation.Right);
            fr.AddEntry(1852, 1866, "2nd French Empire", PrintLocation.Right);

            var de = new Block {Ruler = rule, Title = "Prussia"};
            de.AddEntry(1848, "revolution of 1848");
            de.AddEntry(new LeaderEntry("Frederick William II", new int?[,] { { 1786, 1797 } }) {StartValue = 1789 });
            de.AddEntry(new LeaderEntry("Frederick William III", new int?[,] { { 1797, 1840 } }) { StartValue = 1795 });
            de.AddEntry(new LiteraryWorkEntry("Faust", "Goethe", 1808) {StartValue = 1809 });
            de.AddEntry(new ScienceAdvEntry("electromagnetism", "H.C.Orsted", 1820));
            de.AddEntry(new ScienceAdvEntry("Ohm's law", "Ohm", 1827));
            de.AddEntry(1834, "German free-trade zone(1834)");
            de.AddEntry(new ScienceAdvEntry("plant cells", "M.Schleiden", 1838));
            de.AddEntry(new LeaderEntry("Frederick William IV", new int?[,] { { 1840, 1861 } }));
            de.AddEntry(new ScienceAdvEntry("Neptune discovered", "", 1846));
            de.AddEntry(new LeaderEntry("Otto V.Bismarck(PM)", new int?[,] { { 1862, 1890 } }));
            de.AddEntry(new LiteraryWorkEntry("Capital", "Marx", 1867) { StartValue = 1866 });
            de.AddEntry(new ScienceAdvEntry("inheritance", "Mendel", 1865));


            var aus = new Block {Ruler = rule, Title = "Austrian Empire", Width = 30};
            aus.AddEntry(new LeaderEntry("Joseph II", new int?[,] { { 1765, 1790 } }) {StartValue = 1788 });
            aus.AddEntry(new LeaderEntry("Leopold VII", new int?[,] { { 1790, 1792 } }));
            aus.AddEntry(new LeaderEntry("Francis II", new int?[,] { { 1792, 1804 } }) {Location = PrintLocation.Right});
            aus.AddEntry(new LeaderEntry("Francis I", new int?[,] { { 1804, 1835 } }));
            aus.AddEntry(new LeaderEntry("K.V.Metternich(PM)", new int?[,] { { 1821, 1848 } }));
            aus.AddEntry(new LeaderEntry("Ferdinand I", new int?[,] { { 1835, 1848 } }));
            aus.AddEntry(new LeaderEntry("Francis Joseph", new int?[,] { { 1848, 1916 } }) {StartValue = 1846});
            aus.AddEntry(1815, "Congress of Vienna(1815)");
            aus.AddEntry(new ScienceAdvEntry("Doppler efct", "Doppler", 1842));
            aus.AddEntry(1848, "revolution of 1848");

            var ott = new Block {Ruler = rule, Title = "Ottoman Empire"};
            ott.AddEntry(new LeaderEntry("Selim III", new int?[,] { { 1789, 1807 } }));
            ott.AddEntry(new LeaderEntry("Mustafa IV", new int?[,] { { 1807, 1808 } }));
            ott.AddEntry(new LeaderEntry("Mahmud II", new int?[,] { { 1808, 1839 } }) { StartValue = 1809 });
            ott.AddEntry(new LeaderEntry("Abdulmecid I", new int?[,] { { 1839, 1861 } }));
            ott.AddEntry(1852, "Epirus Revolt(1854)");
            ott.AddEntry(new LeaderEntry("Abdulaziz I", new int?[,] { { 1861, 1876 } }));
            ott.AddEntry(1821, 1832, "Greek War of Independence");

            var rus = new Block {Ruler = rule, Title = "Imperial Russia"};
            rus.AddEntry(new LeaderEntry("Paul I", new int?[,] { { 1796, 1801 } }));
            rus.AddEntry(new LeaderEntry("Alexander I", new int?[,] { { 1801, 1825 } }));
            rus.AddEntry(1825, "Decembrist revolt(1825)");
            rus.AddEntry(new LeaderEntry("Nicholas I", new int?[,] { { 1825, 1855 } }) { StartValue = 1824 });
            rus.AddEntry(new ScienceAdvEntry("Non-Euclidean geo", "Lobachevsky", 1830));
            rus.AddEntry(new LiteraryWorkEntry("A Sportsman Sketches","I.Turgenev",1852) {Location = PrintLocation.Left});
            rus.AddEntry(new LeaderEntry("Alexander II", new int?[,] { { 1855, 1881 } }));
            rus.AddEntry(1860, "Vladivostok founded(1860)");
            rus.AddEntry(1861, "serfs freed(1861)");
            rus.AddEntry(1854, 1856, "Crimean War", PrintLocation.Right);


            plate.AddBlock(usa);
            plate.AddBlock(csa);
            plate.AddBlock(engd);
            plate.AddBlock(fr);
            plate.AddBlock(de);
            plate.AddBlock(aus);
            plate.AddBlock(ott);
            plate.AddBlock(rus);

            plate.AddArrow(new Arrow(aus, ott) {Text = "B.of Focsani(1789)", StartValue = 1789});
            plate.AddArrow(new Arrow(rus, ott) {StartValue = 1789, FromRightToLeftArrowHead = "<......"});
            plate.AddArrow(new Arrow(aus, fr) {StartValue = 1792});
            plate.AddArrow(new Arrow(de, fr) {StartValue = 1793});
            plate.AddArrow(new Arrow(fr, aus) {StartValue = 1796, Text = "B.of Rivoli(1797)"});
            plate.AddArrow(new Arrow(fr, ott) {Text = "B.of the Pryamids(1798)(c.)", StartValue = 1797});
            plate.AddArrow(new Arrow(engd, fr) {Text = "B.of the Nile(1798)(g.)", StartValue = 1798});
            plate.AddArrow(new Arrow(fr, aus) {Text = "B.of Marengo(1800)", StartValue = 1800});
            plate.AddArrow(new Arrow(rus, fr) {Text = "B.of Leipzig(1813)", StartValue = 1813});
            plate.AddArrow(new Arrow(engd, usa)
            {
                Text = "DC sacked(1814)",
                StartValue = 1814,
                ArrowTail = ".",
                FromRightToLeftArrowHead = "<...."
            });
            plate.AddArrow(new Arrow(engd, fr) {Text = "Waterloo(1815)", StartValue = 1815});
            plate.AddArrow(new Arrow(fr, rus) {StartValue = 1812});
            plate.AddArrow(new Arrow(rus, ott) {StartValue = 1853, Text = "B.of Sinop(1853)"});
            plate.AddArrow(new Arrow(engd, rus) {StartValue = 1854, Text = "B.of Balaclava(1854)", FromLeftToRightArrowHead = "..>"});

            plate.Notes.Add("(a.)  last Prime Minister dismissed by a king,  (b.) last met in 1614");
            plate.Notes.Add(
                "(c.)  intended to cut England off from India, (d.) pop. of black slaves goes from 700 thousand to 3 million");
            plate.Notes.Add("(e.)  April 12, 1861, (f.) April 9, 1865 (est. 620,000 killed)");
            plate.Notes.Add(
                "(g.)  Napolean's Navy destroyed,  (h.) Gen. Harrison defeats confederacy of Native Americans");

            return plate;
        }
    }
}
