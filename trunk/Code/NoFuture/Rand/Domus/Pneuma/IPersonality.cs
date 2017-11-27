namespace NoFuture.Rand.Domus.Pneuma
{
    /// <summary>
    /// For making random decisions
    /// </summary>
    /// <remarks>
    /// some data at http://personality-testing.info/_rawdata/
    /// </remarks>
    public interface IPersonality
    {
        Openness Openness { get; }
        Conscientiousness Conscientiousness { get; }
        Extraversion Extraversion { get; }
        Agreeableness Agreeableness { get; }
        Neuroticism Neuroticism { get; }

        bool GetRandomActsIrresponsible();
        bool GetRandomActsStressed();
        bool GetRandomActsSpontaneous();
    }
}