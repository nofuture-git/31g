using System;

namespace NoFuture.Rand.Edu
{
    public interface IUniversity
    {
        string Name { get; set; }
        string CampusName { get; set; }
        float? CrimeRate { get; set; }
        Uri Website { get; set; }
    }
}