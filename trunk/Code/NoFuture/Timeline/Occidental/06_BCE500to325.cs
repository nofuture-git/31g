namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE500to325()
        {
            var rule = new Rule {StartValue = 510, EndValue = 320, RuleLineSpacing = 7};

            var egypt = new Block {Ruler = rule, Title = "Egypt", Width = 30};
            egypt.AddEntry(460, "Rebellion of Inaros 460-454");
            egypt.AddEntry(505, 405, "Egypt under Persian rule");
            egypt.AddEntry(401, 344, "28th, 29th & 30th Dyn.");
            egypt.AddEntry(343, "reconquered by Persia(343)");
            egypt.AddEntry(332, "occupied by Alexander(332)");

            var judea = new Block {Ruler = rule, Title = "Judea", Width = 30};
            judea.AddEntry(new ProphetEntry("Obadiah ?") {StartValue = 495, Location = PrintLocation.Right});
            judea.AddEntry(new ProphetEntry("Malachi") {StartValue = 465, Location = PrintLocation.Right});
            judea.AddEntry(458, "Ezra's mission 458 ??");
            judea.AddEntry(428, "Ezra's mission 428 ?");
            judea.AddEntry(401, "Bagoas governor");
            judea.AddEntry(398, "Ezra's mission 398 ?");
            judea.AddEntry(445, 435, "Nehemiah governor(445-)");
            judea.AddEntry(331, "Alexander takes Jerusalem(331)");

            var persia = new Block {Ruler = rule, Title = "Persian Empire", Width = 22};
            persia.AddEntry(new LeaderEntry("Darius I", new int?[,] {{422, 486}}) {StartValue = 507});
            persia.AddEntry(new LeaderEntry("Xerxes", new int?[,] {{486, 465}}) {StartValue = 486});
            persia.AddEntry(new LeaderEntry("Artaxerxes I", new int?[,] {{465, 424}}) {StartValue = 465});
            persia.AddEntry(new LeaderEntry("Xerxes II", new int?[,] {{null, 423}}) {StartValue = 423});
            persia.AddEntry(new LeaderEntry("Darius II", new int?[,] {{423, 404}}) {StartValue = 419});
            persia.AddEntry(new LeaderEntry("Artaxerxes II", new int?[,] {{404, 358}}) {StartValue = 404});
            persia.AddEntry(new LeaderEntry("Artaxerxes III", new int?[,] {{358, 338}}) {StartValue = 358});
            persia.AddEntry(new LeaderEntry("Arses", new int?[,] {{338, 336}}) {StartValue = 338});
            persia.AddEntry(new LeaderEntry("Darius III", new int?[,] {{336, 331}}) {StartValue = 335});

            var greece = new Block {Ruler = rule, Title = "Classical Greece", Width = 36};
            greece.AddEntry(499, "Ionian revolt(499)");
            greece.AddEntry(490, "Battle of Marathon(490)");
            greece.AddEntry(480, "Battle of Thermopylae/Salamis(480)");
            greece.AddEntry(478, "Battle of Plataea(479)");

            //greece.AddEntry(, " -");
            greece.AddEntry(new LeaderEntry("Pericles", new int?[,] {{460, 429}}) {StartValue = 465});
            greece.AddEntry(449, "Peace of Callias");
            greece.AddEntry(438, "Parthenon built");
            greece.AddEntry(new PhilosopherEntry("Sophocles", 496, 406)
            {
                StartValue = 496,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Socrates", 470, 399)
            {
                StartValue = 470,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Herodotus", 484, 425)
            {
                StartValue = 484,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Hippocrates", 460, 377)
            {
                StartValue = 460,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Plato", 427, 347)
            {
                StartValue = 427,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Democritus", 460, 370)
            {
                StartValue = 460,
                Location = PrintLocation.Right
            });
            greece.AddEntry(new PhilosopherEntry("Aristotle", 384, 322)
            {
                StartValue = 384,
                Location = PrintLocation.Right
            });

            greece.AddEntry(new LiteraryWorkEntry("Histories", "Herodotus") {StartValue = 440});
            greece.AddEntry(431, 406, "Peloponnesian War");
            greece.AddEntry(475, 432, "Golden Age", PrintLocation.Left);
            greece.AddEntry(404, "Athens under 30 tyrants(404)");
            greece.AddEntry(new LiteraryWorkEntry("Theban plays", "Sophocles") {StartValue = 401});
            greece.AddEntry(new LiteraryWorkEntry("Socratic dialogue", "Plato") {StartValue = 399});
            greece.AddEntry(395, 387, "Corinthian War");
            greece.AddEntry(new LiteraryWorkEntry("Republic", "Plato") {StartValue = 380});
            greece.AddEntry(338, "Battle of Chaeronea(338)");
            greece.AddEntry(336, "Phillip II rules all Greece");
            greece.AddEntry(new LeaderEntry("Alexander the Great", new int?[,] {{336, 323}}) {StartValue = 330});

            var rome = new Block {Ruler = rule, Title = "Roman Republic", Width = 32};
            rome.AddEntry(496, "Battle of Lake Regillus");
            rome.AddEntry(494, "1st Secession(494)");
            rome.AddEntry(491, "Coriolanus joins Volscians");
            rome.AddEntry(458, "L.Q. Cincinnatus(dictator)");
            rome.AddEntry(449, "2nd Secession", PrintLocation.Left);
            rome.AddEntry(449, "Twelve Table(449)", PrintLocation.Right);
            rome.AddEntry(387, "Gauls sack Rome(387)");
            rome.AddEntry(367, "Licinian Rogations(367)");
            rome.AddEntry(348, "Treaty with Carthage(348)");
            rome.AddEntry(new TerritoryEntry("Latium") {StartValue = 338});
            rome.AddEntry(343, "First Samnite War(343-341)");
            rome.AddEntry(327, "Second Samnite War(327-304)");

            var plate = new Plate {Ruler = rule, Name = "Fifth and Forth Centuries BCE"};
            plate.AddArrow(new Arrow(persia, greece) {StartValue = 490});
            plate.AddArrow(new Arrow(persia, greece) {StartValue = 483});
            plate.AddArrow(new Arrow(greece, persia) {StartValue = 330, Text = "fall of Persepolis"});
            plate.AddBlock(egypt);
            plate.AddBlock(judea);
            plate.AddBlock(persia);
            plate.AddBlock(greece);
            plate.AddBlock(rome);

            return plate;
        }
    }
}
