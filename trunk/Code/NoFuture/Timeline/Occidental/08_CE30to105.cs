using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE30to105()
        {
            var rule = new Rule { StartValue = 30, EndValue = 105, RuleLineSpacing = 7 };

            var wRome = new Block { Ruler = rule, Title = "Roman Empire(West)", Width = 48 };
            wRome.AddEntry(32,"Tiberius(14BCE-37)",PrintLocation.Left);
            wRome.AddEntry(46,"+Britain", PrintLocation.Left);
            wRome.AddEntry(37, "Caligula(37-41)", PrintLocation.Left);
            wRome.AddEntry(41, "Claudius(41-54)", PrintLocation.Left);
            wRome.AddEntry(54, "Nero(54-68)", PrintLocation.Left);
            wRome.AddEntry(64, "Great Fire of Rome");
            wRome.AddEntry(67, "Martyrdom of Peter & Paul @ Rome(67)", PrintLocation.Right);
            wRome.AddEntry(68,"'Year of Four Emperors'");
            wRome.AddEntry(70, "Vespasian(69-79)", PrintLocation.Left);
            wRome.AddEntry(79, "Titus(79-81)", PrintLocation.Left);
            wRome.AddEntry(81, "Domitian(81-96)", PrintLocation.Left);
            wRome.AddEntry(97, "Nerva(96-98)", PrintLocation.Left);
            wRome.AddEntry(98, "Trajan(98-117)", PrintLocation.Left);
            wRome.AddEntry(79, "Pompeii destroyed(79)");
            wRome.AddEntry(80, "Colosseum completed(80)");
            wRome.AddEntry(82, "Arch of Titus erected-for capture of Jerusalem");
            wRome.AddEntry(69, 79, "Pope Linus(67-79)", PrintLocation.Right);
            wRome.AddEntry(81, 92, "Pope Anacletus(79-92)", PrintLocation.Right);
            wRome.AddEntry(90,"[The Jewish War(Josephus)]");
            wRome.AddEntry(93, 99, "Pope Clement I(92-99)", PrintLocation.Right);
            wRome.AddEntry(100, 105, "Pope Evaristus(99-107)", PrintLocation.Right);

            wRome.AddEntry(new Entry { StartValue = 69, EndValue = 95, Text = "Flavian Dyn.", Location = PrintLocation.Left });
            wRome.AddEntry(new Entry { StartValue = 31, EndValue = 68, Text = "Julio-Claudian Dyn.", Location = PrintLocation.Left });
            wRome.AddEntry(new Entry { StartValue = 96, EndValue = 105, Text = "Nervan-Antonian Dyn.", Location = PrintLocation.Left });

            var eRome = new Block { Ruler = rule, Title = "Roman Empire(East)", Width = 48 };
            eRome.AddEntry(34, "Conversion of Saul(34)");
            eRome.AddEntry(35, 37, "Paul in Arabia(35-37)");
            eRome.AddEntry(43, "Church of Antioch Founded(43)");
            eRome.AddEntry(48, "First Missionary Journey(48)", PrintLocation.Left);
            eRome.AddEntry(49,52,"Second Missionary Journey(49-52)", PrintLocation.Left);
            eRome.AddEntry(50, "Paul est. Church @ Corinth(50)", PrintLocation.Right);
            eRome.AddEntry(51, "Paul in Athens", PrintLocation.Right);
            eRome.AddEntry(53, 57, "Third Missionary Journey(53-57)", PrintLocation.Left);
            eRome.AddEntry(58,"[Epistle to Romans(Paul)]");
            eRome.AddEntry(67, "[Gospel of Mark (John Mark of Jerusalem)]");
            eRome.AddEntry(70, "[Gospel of Matthew]");
            eRome.AddEntry(85, "[Gospel of Luke]");
            eRome.AddEntry(89, "Polycarp of Smyrna(69-155)");
            eRome.AddEntry(75, "Ignatius of Antioch(35-106)");
            eRome.AddEntry(95, "[Gospel of John]");
            eRome.AddEntry(98, "John the Apostle dies(98)");
            
            var judea = new Block {Ruler = rule, Title = "Judea", Width = 60};
            judea.AddEntry(31, "Pontius Pilate(26-36)", PrintLocation.Left);
            judea.AddEntry(36, "Marcellus(36-37)", PrintLocation.Left);
            judea.AddEntry(37, "Marullus(37-41)", PrintLocation.Left);
            judea.AddEntry(41, "Herod Agrippa I [king](41-44)", PrintLocation.Left);
            judea.AddEntry(43, "Cuspius Fadus(44-46)", PrintLocation.Left);
            judea.AddEntry(46, "T.Julius Alexander(46-48)", PrintLocation.Left);
            judea.AddEntry(48, "Ventidius Cumanus(48-52)", PrintLocation.Left);
            judea.AddEntry(52, "M. Antonius Felixs(52-60)", PrintLocation.Left);
            judea.AddEntry(60, "Porcius Festus(60-62)", PrintLocation.Left);
            judea.AddEntry(62, "Lucceius Albinus(62-64)", PrintLocation.Left);
            judea.AddEntry(64, "Gessius Florus(64-66)", PrintLocation.Left);
            judea.AddEntry(74, "Lucius Flavius Silva(72-81)", PrintLocation.Left);
            judea.AddEntry(81, "M. Salvidenus(81-85)", PrintLocation.Left);
            judea.AddEntry(86, "Cnaeus Pompeius Longinus(c.86)", PrintLocation.Left);
            judea.AddEntry(99, "T. Claudius Atticus Herodes(99-102)", PrintLocation.Left);
            judea.AddEntry(102, "G. Julius Quadratus Bassus(102-104)", PrintLocation.Left);

            judea.AddEntry(66, "Battle of Beth Horan - 21st legion defeated(66)");
            judea.AddEntry(67, "Vespasian & Titus: 10th, 5th & 15th legions arrive(67)");
            judea.AddEntry(68, "Rebels @ Galilee crushed-leaders flee to Jerusalem(68)");
            judea.AddEntry(69, "Vespasian to Rome, Titus advances on Jerusalem(69)");
            judea.AddEntry(70, "Siege of Jerusalem-Temple destroyed(70)");
            judea.AddEntry(71, "Titus to Rome w\\ spoils-triumph procession held(71)");
            judea.AddEntry(72, "Siege of stronghold Masada(72)");

            judea.AddEntry(32, "Stephen's Stoning(32)", PrintLocation.Right);
            judea.AddEntry(42, "James (Zebedee) Killed by Herod(42?)", PrintLocation.Right);
            judea.AddEntry(44, "Paul & Barnabas @ Jerusalem - Famine relief(44)", PrintLocation.Right);
            judea.AddEntry(49, "First Christian Council @ Jerusalem(49)", PrintLocation.Right);
            judea.AddEntry(62, "Martyrdom of James (brother)(62)", PrintLocation.Right);

            judea.AddEntry(31,40,"[prefects]");
            judea.AddEntry(43,64,"[procurators]");
            judea.AddEntry(65, 72, "First Jewish-Roman War(66-72)", PrintLocation.Left);
            judea.AddEntry(73,105,"[legates]");

            var plate = new Plate { Ruler = rule, Name = "First Century CE" };

            plate.AddBlock(wRome);
            plate.AddBlock(eRome);
            plate.AddBlock(judea);

            return plate;
        }
    }
}
