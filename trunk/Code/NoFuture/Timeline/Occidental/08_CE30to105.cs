namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE30to105()
        {
            var rule = new Rule {StartValue = 30, EndValue = 105, RuleLineSpacing = 7};

            var wRome = new Block {Ruler = rule, Title = "Roman Empire(West)", Width = 48};
            wRome.AddEntry(new LeaderEntry("Tiberius", new int?[,] {{null, 37}}) {StartValue = 32});
            wRome.AddEntry(new TerritoryEntry("Britain") {StartValue = 46});
            wRome.AddEntry(new LeaderEntry("Caligula", new int?[,] {{37, 41}}) {StartValue = 37});
            wRome.AddEntry(new LeaderEntry("Claudius", new int?[,] {{41, 54}}) {StartValue = 41});
            wRome.AddEntry(new LeaderEntry("Nero", new int?[,] {{54, 68}}) {StartValue = 54});
            wRome.AddEntry(64, "Great Fire of Rome");
            wRome.AddEntry(67, "Martyrdom of Peter & Paul @ Rome(67)", PrintLocation.Right);
            wRome.AddEntry(68, "'Year of Four Emperors'");
            wRome.AddEntry(new LeaderEntry("Vespasian", new int?[,] {{69, 79}}) {StartValue = 70});
            wRome.AddEntry(new LeaderEntry("Titus", new int?[,] {{79, 81}}) {StartValue = 79});
            wRome.AddEntry(new LeaderEntry("Domitian", new int?[,] {{81, 96}}) {StartValue = 81});
            wRome.AddEntry(new LeaderEntry("Nerva", new int?[,] {{96, 98}}) {StartValue = 97});
            wRome.AddEntry(new LeaderEntry("Trajan", new int?[,] {{98, 117}}) {StartValue = 98});
            wRome.AddEntry(79, "Pompeii destroyed(79)");
            wRome.AddEntry(80, "Colosseum completed(80)");
            wRome.AddEntry(82, "Arch of Titus erected-for capture of Jerusalem");
            wRome.AddEntry(new LeaderEntry("Pope Linus", new int?[,] {{67, 79}})
            {
                StartValue = 69,
                Location = PrintLocation.Right
            });
            wRome.AddEntry(new LeaderEntry("Pope Anacletus", new int?[,] {{79, 92}})
            {
                StartValue = 81,
                Location = PrintLocation.Right
            });
            wRome.AddEntry(new LiteraryWorkEntry("The Jewish War", "Josephus") {StartValue = 90});
            wRome.AddEntry(new LeaderEntry("Pope Clement I", new int?[,] {{92, 99}})
            {
                StartValue = 93,
                Location = PrintLocation.Right
            });
            wRome.AddEntry(new LeaderEntry("Pope Evaristus", new int?[,] {{99, 107}})
            {
                StartValue = 100,
                Location = PrintLocation.Right
            });

            wRome.AddEntry(69, 95, "Flavian Dyn.", PrintLocation.Left);
            wRome.AddEntry(31, 68, "Julio-Claudian Dyn.", PrintLocation.Left);
            wRome.AddEntry(96, 105, "Nervan-Antonian Dyn.", PrintLocation.Left);

            var eRome = new Block {Ruler = rule, Title = "Roman Empire(East)", Width = 48};
            eRome.AddEntry(34, "Conversion of Saul(34)");
            eRome.AddEntry(35, 37, "Paul in Arabia(35-37)");
            eRome.AddEntry(43, "Church of Antioch Founded(43)");
            eRome.AddEntry(48, "First Missionary Journey(48)", PrintLocation.Left);
            eRome.AddEntry(49, 52, "Second Missionary Journey(49-52)", PrintLocation.Left);
            eRome.AddEntry(50, "Paul est. Church @ Corinth(50)", PrintLocation.Right);
            eRome.AddEntry(51, "Paul in Athens", PrintLocation.Right);
            eRome.AddEntry(53, 57, "Third Missionary Journey(53-57)", PrintLocation.Left);
            eRome.AddEntry(new LiteraryWorkEntry("Epistle to Romans", "Paul of Tarsus") {StartValue = 58});
            eRome.AddEntry(new LiteraryWorkEntry("Gospel of Mark", "John Mark of Jerusalem") {StartValue = 67});
            eRome.AddEntry(new LiteraryWorkEntry("Gospel of Matthew", "") {StartValue = 70});
            eRome.AddEntry(new LiteraryWorkEntry("Gospel of Luke", "") {StartValue = 85});
            eRome.AddEntry(89, "Polycarp of Smyrna(69-155)");
            eRome.AddEntry(75, "Ignatius of Antioch(35-106)");
            eRome.AddEntry(new LiteraryWorkEntry("Gospel of John", "") {StartValue = 95});
            eRome.AddEntry(98, "John the Apostle dies(98)");

            var judea = new Block {Ruler = rule, Title = "Judea", Width = 60};
            judea.AddEntry(new LeaderEntry("Pontius Pilate", new int?[,] {{26, 36}})
            {
                StartValue = 31,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Marcellus", new int?[,] {{36, 37}})
            {
                StartValue = 36,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Marullus", new int?[,] {{37, 41}})
            {
                StartValue = 37,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Herod Agrippa I [king]", new int?[,] {{41, 44}})
            {
                StartValue = 41,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Cuspius Fadus", new int?[,] {{44, 46}})
            {
                StartValue = 43,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("T.Julius Alexander", new int?[,] {{46, 48}})
            {
                StartValue = 46,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Ventidius Cumanus", new int?[,] {{48, 52}})
            {
                StartValue = 48,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("M. Antonius Felixs", new int?[,] {{52, 60}})
            {
                StartValue = 52,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Porcius Festus", new int?[,] {{60, 62}})
            {
                StartValue = 60,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Lucceius Albinus", new int?[,] {{62, 64}})
            {
                StartValue = 62,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Gessius Florus", new int?[,] {{64, 66}})
            {
                StartValue = 64,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Lucius Flavius Silva", new int?[,] {{72, 81}})
            {
                StartValue = 74,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("M. Salvidenus", new int?[,] {{81, 85}})
            {
                StartValue = 81,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("Cnaeus Pompeius Longinus", new int?[,] {{null, 86}})
            {
                StartValue = 86,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("T. Claudius Atticus Herodes", new int?[,] {{99, 102}})
            {
                StartValue = 99,
                Location = PrintLocation.Left
            });
            judea.AddEntry(new LeaderEntry("G. Julius Quadratus Bassus", new int?[,] {{102, 104}})
            {
                StartValue = 102,
                Location = PrintLocation.Left
            });

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

            judea.AddEntry(31, 40, "[prefects]");
            judea.AddEntry(43, 64, "[procurators]");
            judea.AddEntry(65, 72, "First Jewish-Roman War(66-72)", PrintLocation.Left);
            judea.AddEntry(73, 105, "[legates]");

            var plate = new Plate {Ruler = rule, Name = "First Century CE"};

            plate.AddBlock(wRome);
            plate.AddBlock(eRome);
            plate.AddBlock(judea);

            return plate;
        }
    }
}
