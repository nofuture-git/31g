namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE105to325()
        {
            var rule = new Rule {StartValue = 105, EndValue = 325, RuleLineSpacing = 7};

            var rome = new Block {Ruler = rule, Title = "Roman Empire", Width = 48};
            rome.AddEntry(new LiteraryWorkEntry("Histories", "Tacitus") {StartValue = 114 });
            rome.AddEntry(new TerritoryEntry("Dacia[Carpathian Mts]") {StartValue = 111 });
            rome.AddEntry(new TerritoryEntry("Arabia Petraea[Sinia]") { StartValue = 135 });
            rome.AddEntry(new LeaderEntry("Trajan", new int?[,] { { 98, 117 } }) {StartValue = 108, Location = PrintLocation.Right});
            rome.AddEntry(new LeaderEntry("Hadrian", new int?[,] { { 117, 138 } }));
            rome.AddEntry(new LeaderEntry("Antoninus Pius", new int?[,] { { 138, 161 } }));
            rome.AddEntry(144, 158, "\"Roman Golden Age\"", PrintLocation.Right);
            rome.AddEntry(166, "Roman emissaries in China (166)");
            rome.AddEntry(new LiteraryWorkEntry("Meditations", "Marcus Aurelius") { StartValue = 170 });
            rome.AddEntry(new LeaderEntry("Marcus Aurelius", new int?[,] { { 161, 180 } }));
            rome.AddEntry(169, 180, "Antonine Plague (165-180)", PrintLocation.Right);
            rome.AddEntry(107, 191, "Nervan-Antonian Dyn.", PrintLocation.Left);
            rome.AddEntry(224, "(Parthian -> Sassanid Empire (224))");
            rome.AddEntry(193, 235, "Serveran Dyn.", PrintLocation.Left);
            rome.AddEntry(238, "Year of the Five Emperors(238)");
            rome.AddEntry(new LeaderEntry("Aurelian", new int?[,] { { 270, 275 } }));
            rome.AddEntry(240, 282, "'Crisis of the 3rd Century'**", PrintLocation.Left);
            rome.AddEntry(257, 260, "Valerian persecutions(257-260)");
            rome.AddEntry(250, 270, "Cyprian Plague (250-270)", PrintLocation.Right);
            rome.AddEntry(new ScienceAdvEntry("crankshaft","") {StartValue = 300});
            rome.AddEntry(303, 313, "Diocletian persecutions(303-313)", PrintLocation.Right);
            rome.AddEntry(284, 310, "Tetrarchy***", PrintLocation.Left);
            rome.AddEntry(312, 325, "Constantinian Dyn", PrintLocation.Left);
            rome.AddEntry(315, "Battle of the Mulvian Bridge(312)");
            rome.AddEntry(319, "Edict of Milan(313)");


            var west = new Block {Ruler = rule, Title = "Latin West", Width = 60};
            west.AddEntry(new LeaderEntry("Pope Alexander I", new int?[,] { { 107, 115 } }));
            west.AddEntry(new LeaderEntry("Pope Xystus", new int?[,] { { 115, 125 } }));
            west.AddEntry(new LeaderEntry("Pope Telesphorus", new int?[,] { { 125, 138 } }));
            west.AddEntry(new LeaderEntry("Pope Hyginus", new int?[,] { { 138, 140 } }));
            west.AddEntry(new LeaderEntry("Pope Pius", new int?[,] { { 142, 155 } }));
            west.AddEntry(new LeaderEntry("Pope Anicetus", new int?[,] { { 155, 166 } }));
            west.AddEntry(new LeaderEntry("Pope Soter", new int?[,] { { 166, 175 } }));
            west.AddEntry(new LeaderEntry("Pope Eleuterus", new int?[,] { { 175, 189 } }));
            west.AddEntry(new LeaderEntry("Pope Victor I", new int?[,] { { 189, 199 } }));
            west.AddEntry(new LeaderEntry("Pope Zephyrinus", new int?[,] { { 199, 217 } }) { StartValue = 201 });
            west.AddEntry(new LeaderEntry("Pope Callixtus", new int?[,] { { 217, 222 } }));
            west.AddEntry(new LeaderEntry("Pope Urban", new int?[,] { { 222, 230 } }));
            west.AddEntry(new LeaderEntry("Pope Pontian", new int?[,] { { 230, 235 } }));
            west.AddEntry(new LeaderEntry("Pope Anterus", new int?[,] { { 235, 236 } }));
            west.AddEntry(new LeaderEntry("Pope Fabian", new int?[,] { { 236, 250 } }) { StartValue = 239 });
            west.AddEntry(new LeaderEntry("Pope Sixtus II", new int?[,] { { 257, 258 } }));

            west.AddEntry(new HeresyEntry("Marcionism", "Old Test. false") {StartValue = 150});
            west.AddEntry(new PhilosopherEntry("Irenaeus", 130, 202) {StartValue = 160});
            west.AddEntry(new LiteraryWorkEntry("Against Heresies", "Irenaeus") { StartValue = 180 });
            west.AddEntry(new PhilosopherEntry("Cyprian of Carthage", 200, 258) { StartValue = 225});
            west.AddEntry(new PhilosopherEntry("Tertullian", 160, 225) { StartValue = 210 });
            west.AddEntry(new LiteraryWorkEntry("Apologeticus", "Tertullian") { StartValue = 193 });
            west.AddEntry(new HeresyEntry("Valentinians", "Aeons") { StartValue = 173 });
            west.AddEntry(new HeresyEntry("Adoptionism", "christ divine after baptism") { StartValue = 195 });
            west.AddEntry(new HeresyEntry("Sabellianism", "no trinity") { StartValue = 200 });
            west.AddEntry(new HeresyEntry("Manichaeism", "duality, east import") { StartValue = 312 });
            west.AddEntry(new HeresyEntry("Donatist", "disallow lapsed") { StartValue = 280 });
            west.AddEntry(260, 274, "Gallic Empire (260-274)");

            var east = new Block {Ruler = rule, Title = "Greek East", Width = 48};
            east.AddEntry(new PhilosopherEntry("Justin Martyr", 100, 165) { StartValue = 130 });
            east.AddEntry(new LiteraryWorkEntry("First Apology", "Martyr") {StartValue = 155 });
            east.AddEntry(new PhilosopherEntry("Clement of Alexandria", 150, 215) { StartValue = 200 });
            east.AddEntry(new PhilosopherEntry("Origen", 185, 254) { StartValue = 225 });
            east.AddEntry(new LiteraryWorkEntry("Hexapla", "Origen") { StartValue = 245 });

            east.AddEntry(new HeresyEntry("Montanism", "frenzy prophets") {StartValue = 165 });
            east.AddEntry(new HeresyEntry("Docetism", "christ w/o body") { StartValue = 175 });
            east.AddEntry(new HeresyEntry("Ophites", "serpent worship") { StartValue = 135 });
            east.AddEntry(new HeresyEntry("Egyptian Basilides", "non-being creation") { StartValue = 145 });
            east.AddEntry(269, 274, "Palmyrene Empire (269-274)");

            var judea = new Block {Ruler = rule, EndValue = 142, Title = "Judea", Width = 64};
            judea.AddEntry(124, 142, "Third Jewish-Roman War", PrintLocation.Left);
            judea.AddEntry(128, "Planned revolt begins, led by Simon bar Kokhba(132)");
            judea.AddEntry(130, "Judea an independent state(132-134)");
            judea.AddEntry(134, "12 legions total, Jerusalem lost, Betar fortress siege(135)");
            judea.AddEntry(137, "580,000 Jews killed, XXII legion destroyed(135)");
            judea.AddEntry(139, "Hadrian outlaws Judism, Judea renamed Palaestina(136)");
            var plate = new Plate {Ruler = rule, Name = "Second and Third Centuries CE (Ante-Nicene)"};

            plate.AddBlock(rome);
            plate.AddBlock(west);
            plate.AddBlock(east);
            plate.AddBlock(judea);
            plate.FileName = "09_CE105to325";

            plate.Notes.Add(" *** Empire ruled by four different tetrarchs in four different capitals.");
            plate.Notes.Add(
                " ** A period in which the empire nearly collapsed by the combined affects of invasion, civil war, plague, hyperinflation.");
            return plate;
        }
    }
}
