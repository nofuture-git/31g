using System.Runtime.Remoting.Proxies;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate BCE325to27()
        {
            var rule = new Rule { StartValue = 330, EndValue = 20, RuleLineSpacing = 7 };

            var rome = new Block {Ruler = rule, Title = "Roman Republic",Width = 44};
            rome.AddEntry(298,"Third Samnite War(298-290)");

            rome.AddEntry(new Entry() { StartValue = 283, Text = "+Greek Italy", Location = PrintLocation.Left }); 
            rome.AddEntry(new Entry() { StartValue = 280, Text = "Pyrrhic War (280-279)", Location = PrintLocation.Right });
            rome.AddEntry(new Entry() { StartValue = 272, Text = "+Lombardy & lower Italy", Location = PrintLocation.Left });
            rome.AddEntry(264, "First Punic War (264-241)");
            rome.AddEntry(248,"Hamilcar invades Sicily");
            rome.AddEntry(new Entry() { StartValue = 241, Text = "+Sicily", Location = PrintLocation.Left });
            rome.AddEntry(new Entry() { StartValue = 239, Text = "+Sardinia & Corsica", Location = PrintLocation.Left });
            rome.AddEntry(new Entry() { StartValue = 222, Text = "+Cisalpine Gaul", Location = PrintLocation.Left });
            rome.AddEntry(219, "Second Punic War(219-201)");
            rome.AddEntry(215,"First Macedonian War(214-205)");
            rome.AddEntry(212, "Rome capture Syracuse(212)");
            rome.AddEntry(207, "Hasdrubal defeated @ Metaurus(207)");
            rome.AddEntry(202, "Scipio defeats Hannibal @ Zama(202)");
            rome.AddEntry(new Entry() { StartValue = 199, Text = "+Spain", Location = PrintLocation.Left });
            rome.AddEntry(new Entry() {StartValue = 199, Text = "Second Macedonian War(200-197)", Location = PrintLocation.Right});
            rome.AddEntry(171,"Third Macedonian War(171-168)");
            rome.AddEntry(new Entry(){StartValue = 167,Text = "+Greece", Location =  PrintLocation.Left});
            rome.AddEntry(150,"Third Punic War(150-146)");
            rome.AddEntry(new Entry() { StartValue = 146, Text = "Carthage destroyed", Location = PrintLocation.Right });
            rome.AddEntry(new Entry() { StartValue = 146, Text = "+Tusinia", Location = PrintLocation.Left });
            rome.AddEntry(new Entry() { StartValue = 138, Text = "+Anatolia", Location = PrintLocation.Left });
            rome.AddEntry(new Entry(){StartValue = 139,Text = "First Servile War(139)", Location = PrintLocation.Right});
            rome.AddEntry(112,"The Jugurthine War(112-105)");
            rome.AddEntry(new Entry(){StartValue = 108,Text = "+Allgeria",Location = PrintLocation.Left});
            rome.AddEntry(103,"Second Servile War(103-99)");
            rome.AddEntry(88, "First Mithridatic War(88-84)");
            rome.AddEntry(83, "Second Mithridatic War(83-81)");
            rome.AddEntry(new Entry(){StartValue = 76, Text = "+Bithynia & Cyrene", Location = PrintLocation.Left});
            rome.AddEntry(new Entry(){StartValue = 71, Text = "Third Servile War[Spartacus](73-71)"});
            rome.AddEntry(63,"Third Mithridatic War(75-63)");
            rome.AddEntry(60, "First Triumvirate[Caesar,Crassus,Pompey]");
            rome.AddEntry(new Entry(){StartValue = 57,Text = "+Gaul",Location = PrintLocation.Left});
            rome.AddEntry(50, "[Commentarii de Bello Gallico(Caesar)]");
            rome.AddEntry(44, "Caesar assassinated(44)");
            rome.AddEntry(39, "Second Triumvirate[Antony,Octavian,Lepidus]");
            rome.AddEntry(new Entry(){StartValue = 30,Text = "+Egypt",Location = PrintLocation.Left});
            rome.AddEntry(25,"[The Aeneid(Virgil)]");


            var antigonid = new Block { Ruler = rule, EndValue = 166, Title = "Antigonid(Macedon)" };
            antigonid.AddEntry(306, "Antigonus I(306-301)");
            antigonid.AddEntry(294, "Demetrius I(294-287)");
            antigonid.AddEntry(276, "Antigonus II(276-239)");
            antigonid.AddEntry(239, "Demetrius II(239-229)");
            antigonid.AddEntry(229,"Antigonus III(229-221)");
            antigonid.AddEntry(221,"Philip V(221-179)");
            antigonid.AddEntry(179,"Perseus(179-166)");

            var attalid = new Block { Ruler = rule, EndValue = 133, Title = "Attalid(Anatolia)" };
            attalid.AddEntry(282, "Philetaerus(282-263)");
            attalid.AddEntry(263, "Eumenes I(263-241)");
            attalid.AddEntry(241, "Attalus I(241-197");
            attalid.AddEntry(197, "Eumenes II(197-159)");
            attalid.AddEntry(159, "Attalus II(159-138)");
            attalid.AddEntry(138, "Attalus III(138-133)");

            var ptolemaic = new Block { Ruler = rule, EndValue = 30, Title = "Ptolemaic(Egypt)",Width = 34};
            ptolemaic.AddEntry(290,"Ptolemy I(303-285)");
            ptolemaic.AddEntry(285,"Ptolemy II Philadelphus(285-246)");
            ptolemaic.AddEntry(246,"Ptolemy III Euergetes(246-221)");
            ptolemaic.AddEntry(221,"Ptolemy IV Philopator(221-203)");
            ptolemaic.AddEntry(204,"Ptolemy V Epiphanes(203-181)");
            ptolemaic.AddEntry(181,"Ptolemy VI Philometor(181-145)");
            ptolemaic.AddEntry(145,"Ptolemy VII Euergetes II(145-116)");
            ptolemaic.AddEntry(132,"Septuagint complete(132)");
            ptolemaic.AddEntry(116,"Cleopatra III(116-101)");
            ptolemaic.AddEntry(107,"Ptolemy X Alexander(107-88)");
            ptolemaic.AddEntry(80,"Ptolemy XII Auletes(80-51)");
            ptolemaic.AddEntry(51,"Cleopatra VII Philopator(51-30)");

            var judea = new Block { Ruler = rule, Title = "Judea", Width = 42};
            judea.AddEntry(new Entry(){StartValue = 167, EndValue = 158, Text = "Maccabean Revolt",Location = PrintLocation.Left});
            judea.AddEntry(new Entry() { StartValue = 164, Text = "Temple Rededication(164)", Location = PrintLocation.Right });
            judea.AddEntry(323, 205, "Jews under Ptolemies");
            judea.AddEntry(196, 168, "Jews under Seleucids");
            judea.AddEntry(160,115,"semi-autonomous");
            judea.AddEntry(110,68,"Kingdom of Israel");
            judea.AddEntry(66,"Civil War[Pharisees -vs- Sadducees]");
            judea.AddEntry(62,"Rome[Pompey] & Pharisee seige Jerusalem");

            var seleucid = new Block { Ruler = rule, EndValue = 63, Title = "Seleucid(Syria)", Width = 36};
            seleucid.AddEntry(312,"Seleucus I(312-280)");
            seleucid.AddEntry(280,"Antiochus I(280-261)");
            seleucid.AddEntry(261,"Antiochus II(261-246)");
            seleucid.AddEntry(246,"Seleucus II(246-226)");
            seleucid.AddEntry(226,"Seleucus III(226-223)");
            seleucid.AddEntry(new Entry() { StartValue = 238, Text = "Parthian Empire begins(238)", Location = PrintLocation.Right });
            seleucid.AddEntry(223,"Antiochus (the Great) III(223-187)");
            seleucid.AddEntry(185,"Seleucus IV(187-175)");
            seleucid.AddEntry(175,"Antiochus IV (Epiphanes)(175-163)");
            seleucid.AddEntry(163,"Antiochus V(163-162)");
            seleucid.AddEntry(158,"Dimetrius(162-150)");
            seleucid.AddEntry(150,"Demetrius II(145-138)");
            seleucid.AddEntry(125,"Antiochus VII(125-96)");

            var plate = new Plate() { Ruler = rule, Name = "Third to First Centuries BCE" };
            plate.AddArrow(new Arrow(seleucid,ptolemaic){StartValue = 200,Text = "Battle of Panium"});
            plate.AddArrow(new Arrow(rome,seleucid){StartValue = 188,Text = "Treaty of Apamea"});
            plate.AddArrow(new Arrow(rome,antigonid){StartValue = 171, Text = "Battle of Pydna"});
            plate.AddArrow(new Arrow(rome,ptolemaic){StartValue = 31, Text = "Battle of Actium(31)"});
            
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
