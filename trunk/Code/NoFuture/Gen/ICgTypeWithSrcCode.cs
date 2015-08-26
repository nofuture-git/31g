using System.Collections.Generic;

namespace NoFuture.Gen
{
    public interface ICgTypeWithSrcCode
    {
        CgType CgType { get; }

        /// <summary>
        /// The root directory of all the various symbols written to disk using <see cref="CgTypeFiles.SavePdbLinesToFile"/>
        /// </summary>
        string RootPdbOutDirectory { get; set; }

        /// <summary>
        /// The original assembly used whose .pdb fueled all work hitherto.
        /// </summary>
        string AssemblyPath { get; set; }

        /// <summary>
        /// The original source code file in the same location as is recorded in the .pdb.
        /// </summary>
        string OriginalSrcFile { get; set; }

        /// <summary>
        /// Use this to discern what went wrong.
        /// </summary>
        List<string> ErrorMessages { get; set; }
    }
}