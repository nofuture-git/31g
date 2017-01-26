namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE325to550()
        {
            var rule = new Rule {StartValue = 325, EndValue = 550, RuleLineSpacing = 7};

            var rome = new Block {Ruler = rule, Title = "Roman Empire", EndValue = 479, Width = 60};
            rome.AddEntry(new LeaderEntry("Constantine the Great", new int?[,] {{306, 337}}) {StartValue = 327});
            rome.AddEntry(330, "Capital moved to Byzantium(330)");
            rome.AddEntry(341, "Council of Antioch(341)");
            rome.AddEntry(360, "Julian Apostocy(361-363)");
            rome.AddEntry(new LiteraryWorkEntry("Latin Vulgate", "Jerome") {StartValue = 405});
            rome.AddEntry(380, "Edict of Thessalonica(380)", PrintLocation.Right);
            rome.AddEntry(383, "Legions withdrawn from Britian(383)");
            rome.AddEntry(new LiteraryWorkEntry("Confessions", "Augustine") {StartValue = 397});
            rome.AddEntry(415, "Hypatia murdered in Alexandria(415)");
            rome.AddEntry(new LiteraryWorkEntry("City of God", "Augustine") {StartValue = 420});
            rome.AddEntry(new LeaderEntry("Theodosius[E]", new int?[,] {{379, 395}}));
            rome.AddEntry(new LeaderEntry("Honorius[W]", new int?[,] {{395, 423}}));
            rome.AddEntry(new LeaderEntry("Theodosius II[E]", new int?[,] {{408, 450}}));
            rome.AddEntry(new LeaderEntry("Valentinian III[W]", new int?[,] {{425, 455}}));
            rome.AddEntry(new LeaderEntry("Odoacer King of Italy", new int?[,] {{479, 493}}) {StartValue = 476});
            rome.AddEntry(337, 379, "Arian Controversy(337-381)");

            var vis = new Block {Ruler = rule, Title = "Visigoths", StartValue = 385, EndValue = 507};
            vis.AddEntry(new LeaderEntry("Alaric", new int?[,] {{395, 410}}) {StartValue = 390});
            vis.AddEntry(new LeaderEntry("Theodoric I", new int?[,] {{418, 451}}) {StartValue = 416});

            var ost = new Block {Ruler = rule, Title = "Ostrogoths", StartValue = 476, Width = 32};
            ost.AddEntry(490, "Odoacer defeated in Italy");
            ost.AddEntry(new LeaderEntry("Theoderic the Great", new int?[,] {{493, 526}}));
            ost.AddEntry(new LeaderEntry("Totila", new int?[,] {{541, 552}}));

            var van = new Block {Ruler = rule, Title = "Vandels", StartValue = 404, EndValue = 533, Width = 36};
            van.AddEntry(430, "Seige of Hippo(430)");
            van.AddEntry(439, "Seige of Carthage(439)");
            van.AddEntry(new LeaderEntry("Gaiseric", new int?[,] {{428, 477}}) {StartValue = 426});

            var hun = new Block {Ruler = rule, StartValue = 370, EndValue = 456, Title = "Huns", Width = 44};
            hun.AddEntry(372, "cross Volga(370)");
            hun.AddEntry(376, "subjugate Ostrogoths(376)");
            hun.AddEntry(382, "settle in Pannonia(382)");
            hun.AddEntry(398, "invade Sassanid Empire, fall back(398)");
            hun.AddEntry(435, "Treaty of Margus[w/ Rome](435)");
            hun.AddEntry(437, "Attila and Bleda co-rulers(437)");
            hun.AddEntry(440, "exact tribute from Constantinople(443)");
            hun.AddEntry(453, "Battle of Nedao(453)[collapse]", PrintLocation.Right);
            hun.AddEntry(new LeaderEntry("Attila", new int?[,] {{444, 453}}) {Location = PrintLocation.Right});

            var byz = new Block {Ruler = rule, StartValue = 476, Title = "Byzantine Empire", Width = 36};
            byz.AddEntry(new LeaderEntry("Zeno", new int?[,] {{474, 491}}) {StartValue = 480});
            byz.AddEntry(529, "Nika riots(532)", PrintLocation.Right);
            byz.AddEntry(540, "Hagia Sophia completed(537)", PrintLocation.Right);
            byz.AddEntry(new LeaderEntry("Justinian I", new int?[,] {{527, 565}}));
            byz.AddEntry(532, "capture Carthage(534)", PrintLocation.Right);
            byz.AddEntry(542, "capture Ravenna(540)", PrintLocation.Right);
            byz.AddEntry(545, "retakes Syria [Sasanian Empire]", PrintLocation.Right);

            var fnk = new Block {Ruler = rule, StartValue = 482, Title = "Franks"};
            fnk.AddEntry(490, "Paris made as capital");
            fnk.AddEntry(new LeaderEntry("Clovis I", new int?[,] {{509, 534}}));
            fnk.AddEntry(new LeaderEntry("Sigebert I", new int?[,] {{535, 575}}) {StartValue = 533});

            var plate = new Plate {Ruler = rule, Name = "Fall of Roman Empire"};

            plate.AddArrow(new Arrow(van, rome) {StartValue = 407, Text = "Vandels x Rhine(407)"});
            plate.AddArrow(new Arrow(vis, rome) {StartValue = 396, Text = "Greece/Italy"});
            plate.AddArrow(new Arrow(vis, rome) {StartValue = 409, Text = "Rome sacked(409)"});
            plate.AddArrow(new Arrow(van, rome) {StartValue = 457, Text = "Rome sacked(455)"});
            plate.AddArrow(new Arrow(vis, van) {StartValue = 420, Text = "take back Spain(420)"});
            plate.AddArrow(new Arrow(van, rome) {StartValue = 430, Text = "invade Africa(430)"});
            plate.AddArrow(new Arrow(byz, van) {StartValue = 533, Text = "Battle of Tricamarum(533)"});
            plate.AddArrow(new Arrow(fnk, vis) {StartValue = 507, Text = "Battle of Vouille(507)"});
            plate.AddArrow(new Arrow(hun, rome) {StartValue = 451, Text = "Battle of Catalaunian Fields(451)"});
            plate.AddArrow(new Arrow(hun, rome) {StartValue = 395, Text = "pillage Cappodocia(395)"});
            plate.AddArrow(new Arrow(hun, rome) {StartValue = 443, Text = "Battle of Chersonesus(443)"});
            plate.AddArrow(new Arrow(hun, rome) {StartValue = 453, Text = "invade Italy, Aquileia razed(452)**"});
            plate.AddArrow(new Arrow(byz, ost) {StartValue = 535, Text = "Gothic Wars(535-554)"});
            plate.AddBlock(rome);
            plate.AddBlock(vis);
            plate.AddBlock(ost);
            plate.AddBlock(fnk);
            plate.AddBlock(van);
            plate.AddBlock(hun);
            plate.AddBlock(byz);
            plate.FileName = "10_CE325_550";

            plate.Notes.Add("* Valentinian II[W] sudden death 392");
            plate.Notes.Add("  Theodusius[E] installs Flavius Eugenis in [W] who favors old pagan Roman religion 392");
            plate.Notes.Add(
                "  Flavius pulls legions from Gaul leaving it unprotected - later allowing for Vandel invasion");
            plate.Notes.Add("  At Battle of Frigidas both Alaric & Stilicho are amoung Theodusius generals 394");
            plate.Notes.Add(
                "  Alaric bitter concerning recognition at battle breaks treaty with Rome and is made King of Visigoths 395");
            plate.Notes.Add("  Alaric invades Greece and is defeated by Stilicho 396");
            plate.Notes.Add("  Alaric invades Italy, Stilicho rescues Honorius[W] 398");
            plate.Notes.Add("  Honorius[W] has Stilicho executed, in outrage Alaric invades 408");
            plate.Notes.Add("");
            plate.Notes.Add(
                "** On road to Rome Atilla meets with Pope Leo and two Seneators, agrees to withdraw from Italy(452)");
            plate.Notes.Add("   Valentinian III[W] persuaded by Petronius to assisinate Aetius (Rome's defender)(455)");
            plate.Notes.Add(
                "   Petronius then persuades Aetius's followers to assisinate Valentinian III[W], names himself Emperor(455)");
            plate.Notes.Add(
                "   Petronius forces Eudoxia, Valentinian III's widow to marry him.  She appeals to Gaiseric who sails from Carthage, sacks Rome and kills Petronias(455).");

            return plate;

        }
    }
}
