namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE825to1075()
        {
            var rule = new Rule {StartValue = 825, EndValue = 1075, RuleLineSpacing = 7};

            var scdv = new Block {Ruler = rule, Title = "Scandinavia", Width = 40};
            scdv.AddEntry(new ExplorerEntry("Vikings", "Iceland") {StartValue = 830});
            scdv.AddEntry(new LeaderEntry("Harald Fairhair", new int?[,] {{872, 930}}));
            scdv.AddEntry(new LeaderEntry("Harold Bluetooth", new int?[,] {{970, 986}})
            {
                StartValue = 960
            });
            scdv.AddEntry(new LeaderEntry("Olaf Tryggvason", new int?[,] {{995, 1000}}));
            scdv.AddEntry(new LeaderEntry("Sweyn Forkbeard", new int?[,] {{986, 1014}}));
            scdv.AddEntry(new LeaderEntry("Olaf II Haraldsson", new int?[,] {{1015, 1028}})
            {
                StartValue = 1011
            });
            scdv.AddEntry(new LeaderEntry("Cnut the Great", new int?[,] {{1016, 1035}})
            {
                StartValue = 1018
            });
            scdv.AddEntry(1014, 1035, "North Sea Empire***");
            scdv.AddEntry(new LeaderEntry("Harald Sigurdsson", new int?[,] {{1046, 1066}}));
            scdv.AddEntry(new ExplorerEntry("Leif Erikson", "N.America") {StartValue = 1000});

            var engd = new Block {Ruler = rule, Title = "English Isles", Width = 34};
            engd.AddEntry(853, "Vikings found Dublin(853)");
            engd.AddEntry(new LeaderEntry("Alfred the Great", new int?[,] {{871, 899}}));
            engd.AddEntry(new LeaderEntry("AEthelred", new int?[,] {{1013, 1014}}) {StartValue = 1009});
            engd.AddEntry(new LeaderEntry("Edward the Confessor", new int?[,] {{1042, 1066}}));

            var hre = new Block {Ruler = rule, Title = "Holy Roman Empire", Width = 48};
            hre.AddEntry(831, "Archdiocese of Hamburg founded(831)");
            hre.AddEntry(new LeaderEntry("Louis I", new int?[,] {{814, 840}}) {StartValue = 835});
            hre.AddEntry(843, "Treaty of Verdun(843)");
            hre.AddEntry(846, "Lothair I(Italy);Louis(Germany);Charles(France)");

            hre.AddEntry(new LeaderEntry("Otto I", new int?[,] {{962, 973}}) {StartValue = 961});
            hre.AddEntry(963, "Otto has Pope John XII deposed(963)");
            hre.AddEntry(967, "Otto has Pope John XIII restored(966)");
            hre.AddEntry(972, "Otto's son married to Byz. princess(972)");
            hre.AddEntry(new LeaderEntry("Otto II", new int?[,] {{973, 983}}) {StartValue = 975});
            hre.AddEntry(new LeaderEntry("Otto III", new int?[,] {{996, 1002}}) {StartValue = 999});

            var norm = new Block {Ruler = rule, Title = "Normandy", Width = 34, StartValue = 904};
            norm.AddEntry(new LeaderEntry("Rollo", new int?[,] {{911, 927}}));
            norm.AddEntry(new LeaderEntry("William I", new int?[,] {{927, 942}}));
            norm.AddEntry(new LeaderEntry("Richard I", new int?[,] {{942, 966}}));
            norm.AddEntry(new LeaderEntry("Richard II", new int?[,] {{996, 1026}}));
            norm.AddEntry(new LeaderEntry("Robert I", new int?[,] {{1027, 1035}}));
            norm.AddEntry(new LeaderEntry("William III", new int?[,] {{1035, 1087}}));


            var byz = new Block {Ruler = rule, Title = "Byzantine Empire", Width = 32};
            byz.AddEntry(new LeaderEntry("Michael III", new int?[,] {{842, 867}}));
            byz.AddEntry(new LeaderEntry("Basil I", new int?[,] {{867, 886}}));
            byz.AddEntry(new LeaderEntry("Leo VI", new int?[,] {{886, 912}}));
            byz.AddEntry(new LeaderEntry("Constantine VII", new int?[,] {{913, 959}}));
            byz.AddEntry(960, "retake Island of Crete(960)");
            byz.AddEntry(new LeaderEntry("Nikephoros II Phokas", new int?[,] {{963, 969}}));
            byz.AddEntry(new LeaderEntry("Tzimisces", new int?[,] {{969, 976}}) {StartValue = 976});
            byz.AddEntry(new LeaderEntry("Basil II", new int?[,] {{976, 1025}}) {StartValue = 980});
            byz.AddEntry(new LeaderEntry("Constantine VIII", new int?[,] {{1025, 1028}}));
            byz.AddEntry(new LeaderEntry("Zoe", new int?[,] {{1028, 1050}}) {StartValue = 1030});
            byz.AddEntry(new LeaderEntry("Constantine IX", new int?[,] {{1042, 1055}}));
            byz.AddEntry(1045, "rebuild Holy Sepulchre(1042)");
            byz.AddEntry(new LeaderEntry("Constantine X", new int?[,] {{1059, 1067}}));
            byz.AddEntry(new LeaderEntry("Romanos IV", new int?[,] {{1067, 1071}}) {StartValue = 1068});

            var abb = new Block {Ruler = rule, Title = "Abbasid Caliphate", EndValue = 1055, Width = 30};
            abb.AddEntry(new LeaderEntry("Al-Mu'tasim", new int?[,] {{833, 842}}));
            abb.AddEntry(new LeaderEntry("Al-Wathiq", new int?[,] {{842, 847}}));
            abb.AddEntry(new LeaderEntry("Al-Mutawakkil", new int?[,] {{847, 861}}));
            abb.AddEntry(new LeaderEntry("Al-Mu'tamid", new int?[,] {{870, 892}}) {StartValue = 874});
            abb.AddEntry(new LeaderEntry("Al-Mu'tadid", new int?[,] {{892, 902}}));
            abb.AddEntry(new LeaderEntry("Al-Muktafi", new int?[,] {{902, 908}}));
            abb.AddEntry(new LeaderEntry("Al-Muqtadir", new int?[,] {{908, 932}}));
            abb.AddEntry(new LeaderEntry("Al-Qahir", new int?[,] {{932, 934}}));
            abb.AddEntry(new LeaderEntry("Al-Radi", new int?[,] {{934, 940}}) {StartValue = 935});
            abb.AddEntry(new LeaderEntry("Al-Muttaqi", new int?[,] {{940, 944}}));
            abb.AddEntry(new LeaderEntry("Al-Mustakfi", new int?[,] {{944, 946}}) {StartValue = 944});
            abb.AddEntry(new LeaderEntry("Al-Muti", new int?[,] {{946, 974}}) {StartValue = 947});
            abb.AddEntry(new LeaderEntry("Al-Ta'i", new int?[,] {{974, 991}}));
            abb.AddEntry(new LeaderEntry("Al-Qadir", new int?[,] {{991, 1031}}));
            abb.AddEntry(new LiteraryWorkEntry("Book of Optics","Alhazen",1021));
            abb.AddEntry(new LeaderEntry("Al-Qa'im", new int?[,] {{1031, 1075}}));
            abb.AddEntry(829, 856, "\"Islamic golden age\"");
            abb.AddEntry(861, 870, "Anarchy at Samarra(861-870)");

            var ftd = new Block {Ruler = rule, Title = "Fatimid Caliphate", StartValue = 909, Width = 32};
            ftd.AddEntry(972, "Cario est.(969)");
            ftd.AddEntry(new LeaderEntry("Al-Hakim bi-Amr", new int?[,] {{996, 1021}}) {StartValue = 996});
            ftd.AddEntry(1009, "destroys Holy Sepulchre(1009)");
            ftd.AddEntry(new LeaderEntry("Ali az-Zahir", new int?[,] {{1021, 1036}}) {StartValue = 1021});
            ftd.AddEntry(new LeaderEntry("Al-Mustansir", new int?[,] {{1036, 1094}}) {StartValue = 1036});
            ftd.AddEntry(new LiteraryWorkEntry("Arabian Nights","",1050));
            ftd.AddEntry(1065, "famine(1065 & 1072)");

            var slj = new Block {Ruler = rule, Title = "Seljuk Empire", StartValue = 1037};
            slj.AddEntry(new LeaderEntry("Tughril", new int?[,] {{1037, 1063}}) {StartValue = 1040});
            slj.AddEntry(new LeaderEntry("Alp Arslan", new int?[,] {{1063, 1072}}));

            var plate = new Plate {Ruler = rule, Name = "Viking Age"};
            plate.AddBlock(scdv);
            plate.AddBlock(engd);
            plate.AddBlock(norm);
            plate.AddBlock(hre);
            plate.AddBlock(ftd);
            plate.AddBlock(byz);
            plate.AddBlock(slj);
            plate.AddBlock(abb);

            plate.AddArrow(new Arrow(scdv, engd) {StartValue = 830, Text = "raid Ireland(812-853)"});
            plate.AddArrow(new Arrow(scdv, engd) {StartValue = 839, Text = "end Pict kingdom(839)"});
            plate.AddArrow(new Arrow(scdv, engd) {StartValue = 878, Text = "Battle of Edington(878)"});
            plate.AddArrow(new Arrow(scdv, norm) {StartValue = 908, Text = "settle Normandy(911)"});
            plate.AddArrow(new Arrow(hre, scdv) {StartValue = 950, Text = "Otto I defeat Bluetooth(950)"});
            plate.AddArrow(new Arrow(hre, byz) {StartValue = 968, Text = "Otto seize S.Italy(968)"});
            plate.AddArrow(new Arrow(ftd, abb) {StartValue = 972, Text = "seize Egypt(969)"});
            plate.AddArrow(new Arrow(scdv, engd) {StartValue = 1013, Text = "Danes England(1013)"});
            plate.AddArrow(new Arrow(hre, ftd) {StartValue = 1070, Text = "R.Guiscard Sicily(1070)"});
            plate.AddArrow(new Arrow(norm, engd) {StartValue = 1066, Text = "Norman conquest(1066)"});
            plate.AddArrow(new Arrow(hre, ftd) {StartValue = 982, Text = "Battle Cape Colonna(982)"});
            plate.AddArrow(new Arrow(slj, abb) {StartValue = 1055, Text = "capture Baghdad(1055)"});
            plate.AddArrow(new Arrow(slj, byz) {StartValue = 1071, Text = "Battle of Manzikert(1071)**"});

            plate.Notes.Add(
                "*  this guy was all over the place, fights alongside his brother Olaf Haraldsson, a.k.a. " +
                "Saint Olaf, who sought to reclaim the Danish throne from Cnut the Great - is defeated,");
            plate.Notes.Add(
                "   goes into exile to the Varangian (Viking) kingdom around Novgorod (Russia) working with Yaroslav " +
                "the Wise who unites Keiv and Novgorod then moves to Constantinople (1034)");
            plate.Notes.Add(
                "   becomes captain of Byzantine Varangian Guard (Viking bodyguards to the Emperor) gets rich, " +
                "leaves in 1042 and becomes King of Norway 1045, was caught up in war of succession in England,");
            plate.Notes.Add(
                "   goes there to become king and is killed at the Battle of Stamford Bridge(1066) " +
                "by Harold Godwinson's forces (which is considered the end of the Viking Age).");

            plate.Notes.Add("** during this war Palestine is taken from Fatimid Cali.- later catalyst for Crusades");

            plate.Notes.Add("*** Includes Norway, Denmark and England");

            return plate;
        }
    }
}
