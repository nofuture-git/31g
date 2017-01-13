﻿namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE325to27()
        {
            var rule = new Rule {StartValue = 330, EndValue = 20, RuleLineSpacing = 7};

            var rome = new Block {Ruler = rule, Title = "Roman Republic", Width = 44};
            rome.AddEntry(298, "Third Samnite War(298-290)");

            rome.AddEntry(new TerritoryEntry("Greek Italy") {StartValue = 283});
            rome.AddEntry(280, "Pyrrhic War (280-279)", PrintLocation.Right);
            rome.AddEntry(new TerritoryEntry("Lombardy & lower Italy") {StartValue = 272});
            rome.AddEntry(264, "First Punic War (264-241)");
            rome.AddEntry(248, "Hamilcar invades Sicily");
            rome.AddEntry(new TerritoryEntry("Sicily") {StartValue = 241});
            rome.AddEntry(new TerritoryEntry("Sardinia & Corsica") {StartValue = 239});
            rome.AddEntry(new TerritoryEntry("Cisalpine Gaul") {StartValue = 222});
            rome.AddEntry(219, "Second Punic War(219-201)");
            rome.AddEntry(215, "First Macedonian War(214-205)");
            rome.AddEntry(212, "Rome capture Syracuse(212)");
            rome.AddEntry(207, "Hasdrubal defeated @ Metaurus(207)");
            rome.AddEntry(202, "Scipio defeats Hannibal @ Zama(202)");
            rome.AddEntry(new TerritoryEntry("Spain") {StartValue = 199});
            rome.AddEntry(199, "Second Macedonian War(200-197)", PrintLocation.Right);
            rome.AddEntry(171, "Third Macedonian War(171-168)");
            rome.AddEntry(new TerritoryEntry("Greece") {StartValue = 167});
            rome.AddEntry(150, "Third Punic War(150-146)");
            rome.AddEntry(146, "Carthage destroyed", PrintLocation.Right);
            rome.AddEntry(new TerritoryEntry("Tusinia") {StartValue = 146});
            rome.AddEntry(new TerritoryEntry("Anatolia") {StartValue = 138});
            rome.AddEntry(139, "First Servile War(139)", PrintLocation.Right);
            rome.AddEntry(112, "The Jugurthine War(112-105)");
            rome.AddEntry(new TerritoryEntry("Allgeria") {StartValue = 108});
            rome.AddEntry(103, "Second Servile War(103-99)");
            rome.AddEntry(88, "First Mithridatic War(88-84)");
            rome.AddEntry(83, "Second Mithridatic War(83-81)");
            rome.AddEntry(new TerritoryEntry("Bithynia & Cyrene") {StartValue = 76});
            rome.AddEntry(71, "Third Servile War[Spartacus](73-71)");
            rome.AddEntry(63, "Third Mithridatic War(75-63)");
            rome.AddEntry(60, "First Triumvirate[Caesar,Crassus,Pompey]");
            rome.AddEntry(new TerritoryEntry("Gaul") {StartValue = 57});
            rome.AddEntry(new LiteraryWorkEntry("Commentarii de Bello Gallico", "Caesar") {StartValue = 50});
            rome.AddEntry(44, "Caesar assassinated(44)");
            rome.AddEntry(39, "Second Triumvirate[Antony,Octavian,Lepidus]");
            rome.AddEntry(new TerritoryEntry("Egypt") {StartValue = 30});
            rome.AddEntry(new LiteraryWorkEntry("The Aeneid", "Virgil") {StartValue = 25});

            var antigonid = new Block {Ruler = rule, EndValue = 166, Title = "Antigonid(Macedon)"};
            antigonid.AddEntry(new LeaderEntry("Antigonus I", new int?[,] {{306, 301}}) {StartValue = 306});
            antigonid.AddEntry(new LeaderEntry("Demetrius I", new int?[,] {{294, 287}}) {StartValue = 294});
            antigonid.AddEntry(new LeaderEntry("Antigonus II", new int?[,] {{276, 239}}) {StartValue = 276});
            antigonid.AddEntry(new LeaderEntry("Demetrius II", new int?[,] {{239, 229}}) {StartValue = 239});
            antigonid.AddEntry(new LeaderEntry("Antigonus III", new int?[,] {{229, 221}}) {StartValue = 229});
            antigonid.AddEntry(new LeaderEntry("Philip V", new int?[,] {{221, 179}}) {StartValue = 221});
            antigonid.AddEntry(new LeaderEntry("Perseus", new int?[,] {{179, 166}}) {StartValue = 179});

            var attalid = new Block {Ruler = rule, EndValue = 133, Title = "Attalid(Anatolia)"};
            attalid.AddEntry(new LeaderEntry("Philetaerus", new int?[,] {{282, 263}}) {StartValue = 282});
            attalid.AddEntry(new LeaderEntry("Eumenes I", new int?[,] {{263, 241}}) {StartValue = 263});
            attalid.AddEntry(new LeaderEntry("Attalus I", new int?[,] {{241, 197}}) {StartValue = 241});
            attalid.AddEntry(new LeaderEntry("Eumenes II", new int?[,] {{197, 159}}) {StartValue = 197});
            attalid.AddEntry(new LeaderEntry("Attalus II", new int?[,] {{159, 138}}) {StartValue = 159});
            attalid.AddEntry(new LeaderEntry("Attalus III", new int?[,] {{138, 133}}) {StartValue = 138});

            var ptolemaic = new Block {Ruler = rule, EndValue = 30, Title = "Ptolemaic(Egypt)", Width = 34};
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy I", new int?[,] {{303, 285}}) {StartValue = 290});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy II Philadelphus", new int?[,] {{285, 246}}) {StartValue = 285});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy III Euergetes", new int?[,] {{246, 221}}) {StartValue = 246});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy IV Philopator", new int?[,] {{221, 203}}) {StartValue = 221});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy V Epiphanes", new int?[,] {{203, 181}}) {StartValue = 204});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy VI Philometor", new int?[,] {{181, 145}}) {StartValue = 181});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy VII Euergetes II", new int?[,] {{145, 116}}) {StartValue = 145});
            ptolemaic.AddEntry(new LiteraryWorkEntry("Septuagint", "") {StartValue = 132 });
            ptolemaic.AddEntry(new LeaderEntry("Cleopatra III", new int?[,] {{116, 101}}) {StartValue = 116});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy X Alexander", new int?[,] {{107, 88}}) {StartValue = 107});
            ptolemaic.AddEntry(new LeaderEntry("Ptolemy XII Auletes", new int?[,] {{80, 51}}) {StartValue = 80});
            ptolemaic.AddEntry(new LeaderEntry("Cleopatra VII Philopator", new int?[,] {{51, 30}}) {StartValue = 51});

            var judea = new Block {Ruler = rule, Title = "Judea", Width = 42};
            judea.AddEntry(167, 158, "Maccabean Revolt", PrintLocation.Left);
            judea.AddEntry(164, "Temple Rededication(164)", PrintLocation.Right);
            judea.AddEntry(323, 205, "Jews under Ptolemies");
            judea.AddEntry(196, 168, "Jews under Seleucids");
            judea.AddEntry(160, 115, "semi-autonomous");
            judea.AddEntry(110, 68, "Kingdom of Israel");
            judea.AddEntry(66, "Civil War[Pharisees -vs- Sadducees]");
            judea.AddEntry(62, "Rome[Pompey] & Pharisee seige Jerusalem");

            var seleucid = new Block {Ruler = rule, EndValue = 63, Title = "Seleucid(Syria)", Width = 36};
            seleucid.AddEntry(new LeaderEntry("Seleucus I", new int?[,] {{312, 280}}) {StartValue = 312});
            seleucid.AddEntry(new LeaderEntry("Antiochus I", new int?[,] {{280, 261}}) {StartValue = 280});
            seleucid.AddEntry(new LeaderEntry("Antiochus II", new int?[,] {{261, 246}}) {StartValue = 261});
            seleucid.AddEntry(new LeaderEntry("Seleucus II", new int?[,] {{246, 226}}) {StartValue = 246});
            seleucid.AddEntry(new LeaderEntry("Seleucus III", new int?[,] {{226, 223}}) {StartValue = 226});
            seleucid.AddEntry(238, "Parthian Empire begins(238)", PrintLocation.Right);
            seleucid.AddEntry(new LeaderEntry("Antiochus (the Great) III", new int?[,] {{223, 187}}) {StartValue = 223});
            seleucid.AddEntry(new LeaderEntry("Seleucus IV", new int?[,] {{187, 175}}) {StartValue = 185});
            seleucid.AddEntry(new LeaderEntry("Antiochus IV (Epiphanes)", new int?[,] {{175, 163}}) {StartValue = 175});
            seleucid.AddEntry(new LeaderEntry("Antiochus V", new int?[,] {{163, 162}}) {StartValue = 163});
            seleucid.AddEntry(new LeaderEntry("Dimetrius", new int?[,] {{162, 150}}) {StartValue = 158});
            seleucid.AddEntry(new LeaderEntry("Demetrius II", new int?[,] {{145, 138}}) {StartValue = 150});
            seleucid.AddEntry(new LeaderEntry("Antiochus VII", new int?[,] {{125, 96}}) {StartValue = 125});

            var plate = new Plate {Ruler = rule, Name = "Third to First Centuries BCE"};
            plate.AddArrow(new Arrow(seleucid, ptolemaic) {StartValue = 200, Text = "Battle of Panium"});
            plate.AddArrow(new Arrow(rome, seleucid) {StartValue = 188, Text = "Treaty of Apamea"});
            plate.AddArrow(new Arrow(rome, antigonid) {StartValue = 171, Text = "Battle of Pydna"});
            plate.AddArrow(new Arrow(rome, ptolemaic) {StartValue = 31, Text = "Battle of Actium(31)"});

            plate.AddBlock(rome);
            plate.AddBlock(seleucid);
            plate.AddBlock(judea);
            plate.AddBlock(ptolemaic);
            plate.AddBlock(antigonid);
            plate.AddBlock(attalid);

            return plate;
        }
    }
}
