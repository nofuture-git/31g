using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE105to325()
        {
            var rule = new Rule { StartValue = 105, EndValue = 325, RuleLineSpacing = 7 };

            var rome = new Block { Ruler = rule, Title = "Roman Empire", Width = 48 };
            rome.AddEntry(114, "[Histories(Tacitus)]" );
            rome.AddEntry(111, "+Dacia[Carpathian Mts]", PrintLocation.Left);
            rome.AddEntry(135, "+Arabia Petraea[Sinia]", PrintLocation.Left);
            rome.AddEntry(108, 114, "Trajan(98-117)", PrintLocation.Right);
            rome.AddEntry(117,135,"Hadrian(117-138)",PrintLocation.Right);
            rome.AddEntry(138,158,"Antoninus Pius(138-161)",PrintLocation.Right);
            rome.AddEntry(170,"[Meditations(Marcus Aurelius)]");
            rome.AddEntry(161,180,"Marcus Aurelius(161-180)", PrintLocation.Right);
            rome.AddEntry(107, 191, "Nervan-Antonian Dyn.", PrintLocation.Left);
            rome.AddEntry(193, 235, "Serveran Dyn.", PrintLocation.Left);
            rome.AddEntry(238, "Year of the Five Emperors(238)");
            rome.AddEntry(240, 282, "Illyrian emperors", PrintLocation.Left);
            rome.AddEntry(266,"Roman merchents in China(266)");
            rome.AddEntry(257, 260, "Valerian persecutions(257-260)", PrintLocation.Right);
            rome.AddEntry(303, 313, "Diocletian persecutions(303-313)", PrintLocation.Right);
            rome.AddEntry(284, 310, "Tetrarchy", PrintLocation.Left);
            rome.AddEntry(312, 325, "Constantinian Dyn", PrintLocation.Left);
            rome.AddEntry(315,"Battle of the Mulvian Bridge(312)");
            rome.AddEntry(319,"Edict of Milan(313)");

            
            var west = new Block {Ruler = rule, Title = "Latin West", Width = 60};
            west.AddEntry(107,"Pope Alexander I(107-115)",PrintLocation.Right);
            west.AddEntry(117, "Pope Xystus(115-125)", PrintLocation.Right);
            west.AddEntry(127, "Pope Telesphorus(125-138)", PrintLocation.Right);
            west.AddEntry(140, "Pope Hyginus(138-140)", PrintLocation.Right);
            west.AddEntry(144, "Pope Pius(142-155)", PrintLocation.Right);
            west.AddEntry(157, "Pope Anicetus(155-166)", PrintLocation.Right);
            west.AddEntry(168, "Pope Soter(166-175)", PrintLocation.Right);
            west.AddEntry(177, "Pope Eleuterus(175-189)", PrintLocation.Right);
            west.AddEntry(191, "Pope Victor I(189,199)", PrintLocation.Right);
            west.AddEntry(201, "Pope Zephyrinus(199-217)", PrintLocation.Right);
            west.AddEntry(219, "Pope Callixtus(217-222)", PrintLocation.Right);
            west.AddEntry(222, "Pope Urban(222-230)", PrintLocation.Right);
            west.AddEntry(233, "Pope Pontian(230-235)", PrintLocation.Right);
            west.AddEntry(236, "Pope Anterus(235-236)", PrintLocation.Right);
            west.AddEntry(239, "Pope Fabian(236-250)", PrintLocation.Right);

            west.AddEntry(257, "Pope Sixtus II(257-258)", PrintLocation.Right);

            west.AddEntry(150, "[H] Marcionism (Old Test. false)");
            west.AddEntry(156,"Irenaeus(130-202)", PrintLocation.Left);
            west.AddEntry(180, "[Against Heresies(Irenaeus)]");
            west.AddEntry(230, "Cyprian of Carthage(200-258)", PrintLocation.Left);
            west.AddEntry(210, "Tertullian(160-225)",PrintLocation.Left);
            west.AddEntry(193, "[Apologeticus(Tertullian)]");
            west.AddEntry(175, "[H] Valentinians (Aeons)");
            west.AddEntry(195, "[H] Adoptionism (χρ divine after baptism)");
            west.AddEntry(200, "[H] Sabellianism (no trinity)");
            west.AddEntry(312, "[H] Manichaeism (duality, east import)");
            west.AddEntry(250, "[H] Donatist (disallow lapsed)");

            var east = new Block { Ruler = rule, Title = "Greek East", Width = 48 };
            east.AddEntry(130,"Justin Martyr(100-165)",PrintLocation.Left);
            east.AddEntry(155, "[First Apology(Martyr)]");
            east.AddEntry(200,"Clement of Alexandria(150-215)",PrintLocation.Left);
            east.AddEntry(225,"Origen(185-254)",PrintLocation.Left);
            east.AddEntry(240,"[Hexapla(Origen)]");
            east.AddEntry(165, "[H] Montanism (frenzy prophets)");
            east.AddEntry(175, "[H] Docetism (χρ w/o body)");
            east.AddEntry(135,"[H] Ophites (serpent worship)");
            east.AddEntry(145,"[H] Egyptian Basilides (non-being creation)");

            
            var judea = new Block {Ruler = rule, EndValue = 142, Title = "Judea", Width = 64};
            judea.AddEntry(124,142,"Third Jewish-Roman War", PrintLocation.Left);
            judea.AddEntry(128,"Planned revolt begins, led by Simon bar Kokhba(132)");
            judea.AddEntry(130,"Judea an independent state(132-134)");
            judea.AddEntry(134,"12 legions total, Jerusalem lost, Betar fortress siege(135)");
            judea.AddEntry(137,"580,000 Jews killed, XXII legion destroyed(135)");
            judea.AddEntry(139,"Hadrian outlaws Judism, Judea renamed Palaestina(136)");
            var plate = new Plate { Ruler = rule, Name = "Second and Third Centuries CE (Ante-Nicene)" };

            plate.AddBlock(rome);
            plate.AddBlock(west);
            plate.AddBlock(east);
            plate.AddBlock(judea);

            return plate;
        }
    }
}
