namespace NoFuture.Rand.Pneuma
{
    /// <summary>
    /// For making random decisions
    /// description from [https://en.wikipedia.org/wiki/Big_Five_personality_traits]
    /// </summary>
    /// <remarks>
    /// some data at http://personality-testing.info/_rawdata/
    /// </remarks>
    public interface IPersonality
    {
        /// <summary>
        /// Inventive/Curious vs. Consistent/Cautious
        /// </summary>
        Openness Openness { get; }

        /// <summary>
        /// Efficient/Organized vs. Easy-going/Careless
        /// </summary>
        Conscientiousness Conscientiousness { get; }

        /// <summary>
        /// Outgoing/Energetic vs. Solitary/Reserved
        /// </summary>
        Extraversion Extraversion { get; }

        /// <summary>
        /// Friendly/Compassionate vs. Challenging/Detached
        /// </summary>
        Agreeableness Agreeableness { get; }

        /// <summary>
        /// Sensitive/Nervous vs. Secure/Confident
        /// </summary>
        Neuroticism Neuroticism { get; }

        /// <summary>
        /// Gets a random truth-value of an irresponsible decision based on this personality
        /// </summary>
        /// <returns></returns>
        bool GetRandomActsIrresponsible();

        /// <summary>
        /// Gets a random truth-value of a decision-under-stress based on this personality
        /// </summary>
        /// <returns></returns>
        bool GetRandomActsStressed();

        /// <summary>
        /// Gets a random truth-value of a spontaneous decision based on this personality
        /// </summary>
        /// <returns></returns>
        bool GetRandomActsSpontaneous();
    }
}