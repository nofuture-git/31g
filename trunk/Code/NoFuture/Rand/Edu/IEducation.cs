﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Edu
{
    /// <summary>
    /// A simple form of a personal education whose properties 
    /// are the schools which issued the degree.
    /// </summary>
    public interface IEducation : IObviate
    {
        OccidentalEdu EduFlag { get; }
        Tuple<IHighSchool, DateTime?> HighSchool { get; }
        Tuple<IUniversity, DateTime?> College { get; }
    }
}
