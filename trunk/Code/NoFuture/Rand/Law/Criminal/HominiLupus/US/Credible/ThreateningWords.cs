namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Credible
{
    /// <summary>
    /// in words only communicate intent to do someone harm
    /// </summary>
    public class ThreateningWords : CredibleBase
    {
        public ThreateningWords() { }

        public ThreateningWords(string asSuch) { AsSuch = asSuch; }

        public string AsSuch { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var rslt = base.IsValid(persons);
            if (!rslt)
            {
                AddReasonEntry($"{nameof(ThreateningWords)} as such '{AsSuch}'");
            }
            return rslt;
        }
    }
}
