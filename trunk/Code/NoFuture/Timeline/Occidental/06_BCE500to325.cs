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
            judea.AddEntry(495, "(Obadiah ?)", PrintLocation.Right);
            judea.AddEntry(465, "(Malachi)", PrintLocation.Right);
            judea.AddEntry(458, "Ezra's mission 458 ??");
            judea.AddEntry(428, "Ezra's mission 428 ?");
            judea.AddEntry(401, "Bagoas governor");
            judea.AddEntry(398, "Ezra's mission 398 ?");
            judea.AddEntry(445, 435, "Nehemiah governor(445-)");
            judea.AddEntry(331, "Alexander takes Jerusalem(331)");

            var persia = new Block {Ruler = rule, Title = "Persian Empire", Width = 22};
            persia.AddEntry(507, "Darius I 422-486");
            persia.AddEntry(486, "Xerxes 486-465");
            persia.AddEntry(465, "Artaxerxes I 465-424");
            persia.AddEntry(423, "Xerxes II 423");
            persia.AddEntry(419, "Darius II 423-404");
            persia.AddEntry(404, "Artaxerxes II 404-358");
            persia.AddEntry(358, "Artaxerxes III 358-338");
            persia.AddEntry(338, "Arses 338-336");
            persia.AddEntry(335, "Darius III 336-331");

            var greece = new Block {Ruler = rule, Title = "Classical Greece", Width = 36};
            greece.AddEntry(499, "Ionian revolt(499)");
            greece.AddEntry(490, "Battle of Marathon(490)");
            greece.AddEntry(480, "Battle of Thermopylae/Salamis(480)");
            greece.AddEntry(478, "Battle of Plataea(479)");

            greece.AddEntry(465, "Pericles 460-429");
            greece.AddEntry(449, "Peace of Callias");
            greece.AddEntry(438, "Parthenon built");
            greece.AddEntry(496, "(Sophocles 496-406)", PrintLocation.Right);
            greece.AddEntry(470, "(Socrates 470-399)", PrintLocation.Right);
            greece.AddEntry(484, "(Herodotus 484-425)", PrintLocation.Right);
            greece.AddEntry(460, "(Hippocrates 460-377)", PrintLocation.Right);
            greece.AddEntry(427, "(Plato 427-347)", PrintLocation.Right);
            greece.AddEntry(460, "(Democritus 460-370)", PrintLocation.Right);
            greece.AddEntry(384, "(Aristotle 384-322)", PrintLocation.Right);

            greece.AddEntry(431, 406, "Peloponnesian War");
            greece.AddEntry(475, 432, "Golden Age", PrintLocation.Left);
            greece.AddEntry(404, "Athens under 30 tyrants(404)");
            greece.AddEntry(401, "[Theban plays(Sophocles)]");
            greece.AddEntry(399, "[Socratic dialogue (Plato)]");
            greece.AddEntry(395, 387, "Corinthian War");
            greece.AddEntry(380, "[Republic (Plato)]");
            greece.AddEntry(338, "Battle of Chaeronea(338)");
            greece.AddEntry(336, "Phillip II rules all Greece");
            greece.AddEntry(330, "Alexander the Great(336-323)");

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
            rome.AddEntry(338, "+Latium (area around Rome)", PrintLocation.Left);
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
