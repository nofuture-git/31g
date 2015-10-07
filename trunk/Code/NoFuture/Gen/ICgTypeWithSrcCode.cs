using System.Collections.Generic;

namespace NoFuture.Gen
{
    public interface ICgTypeWithSrcCode
    {
        CgType CgType { get; }

        /// <summary>
        /// The original assembly used whose .pdb fueled all work hitherto.
        /// </summary>
        string AssemblyPath { get; set; }

        /// <summary>
        /// Use this to discern what went wrong.
        /// </summary>
        List<string> ErrorMessages { get; set; }
    }
}