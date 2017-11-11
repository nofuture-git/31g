using System;

namespace NoFuture.Rand.Data.Endo.Grps
{
    /// <summary>
    /// The Classification of Instructional Programs (CIP) provides a taxonomic scheme 
    /// that supports the accurate tracking and reporting of fields of study 
    /// and program completions activity.
    /// </summary>
    /// <remarks>
    /// [https://nces.ed.gov/ipeds/cipcode/Default.aspx?y=55]
    /// </remarks>
    [Serializable]
    public class ClassificationOfInstructionalPrograms : StandardOccupationalClassification
    {
        public override string LocalName => "cip-code";

        public override string Abbrev => "CIP";
    }
}