namespace NoFuture.Util.Etymological.Psy
{
    public class Openness : NomenclatureBase
    {
        public Openness()
        {
            Synonyms.AddRange(new[]
            {
                "\bopen",
                "\binvent(e|ive)",
                "\bcurio(us|sity)",
                "\badventur(e|ous)",
                "unusual",
                "creativ(e|ity)",
                "novel",
                "\bvariety",
                "\bimagina(tive|tion)",
                "independent",
                "\bwilling",
            });

            Antonyms.AddRange(new[]
            {
                "\bconsistent",
                "\bcautious",
                "strict",
                "\bconventional",
                "\btradition",
                "plain",
                "straight(-)?forward",
                "\bsuspicio(n|us)",
                "\bfamiliar",
                "\bconservative",
                "\bresist(ant|ed)?",
                "\brestrain(t|ed)?",
            });
        }
    }

    public class Conscientiousness : NomenclatureBase
    {
        public Conscientiousness()
        {
            Synonyms.AddRange(new[]
            {
                "\befficien(t(ly)?|cy)",
                "\borganize",
                "\bdependable",
                "\bdiscipline",
                "\bdut(y|iful)",
                "\bplan(ned|ning)",
                "\bcontrol",
                "\bregulat(e(d)?|ion)",
                "\bcareful",
            });
            Antonyms.AddRange(new[]
            {
                "unorganize",
                "\beasy(-)?going",
                "\bcareless",
                "\bspontaneous",
                "\bimpulsive",
                "undiscipline",
                "undependable",
            });
        }
    }

    public class Extraversion : NomenclatureBase
    {
        public Extraversion()
        {
            Synonyms.AddRange(new[]
            {
                "\boutgoing", 
                "\benergetic", 
                "\bsurgency", 
                "\bassertive", 
                "\btalkative", 
                "\benthusiastic", 
                "action(-)?oriented",
            });
            Antonyms.AddRange(new[]
            {
                "solitary", 
                "\breserved", 
                "quiet", 
                "low(-)?key", 
                "\bdeliberate"
            });
        }
    }

    public class Agreeableness : NomenclatureBase
    {
        public Agreeableness()
        {
            Synonyms.AddRange(new[]
            {
                "\bfriendly", 
                "\bcompassion", 
                "\bcooperat(ing|ive|e(d)?)", 
                "\btrusting", 
                "\bharmon(y|iously)", 
                "\bconsiderate", 
                "\bkind", 
                "\bgenerous",
                "\bcompromis(e(d)?|ing)", 
                "\boptimistic"
            });
            Antonyms.AddRange(new[]
            {
                "\banalytical", 
                "\bdetached", 
                "antagonist", 
                "disagree(able|ing)", 
                "\bskeptic"
            });
        }
    }

    public class Neuroticism : NomenclatureBase
    {
        public Neuroticism()
        {
            Synonyms.AddRange(new[] 
            { 
                "\bsensitive", 
                "nervous(ly)?", 
                "\bvulnerab(e|ility)", 
                "anger(y|ed)?", 
                "anxi(ety|ous)", 
                "\bdepress(ion|ed|ing)", 
                "(in|un)stab(e|ility)" });
            Antonyms.AddRange(new[]
            {
                "\bsecure", 
                "\bconfident", 
                "cop(e|ing)", 
                "\bcontent", 
                "calm", 
                "\bstabl(e|ity)"
            });
        }
    }
}