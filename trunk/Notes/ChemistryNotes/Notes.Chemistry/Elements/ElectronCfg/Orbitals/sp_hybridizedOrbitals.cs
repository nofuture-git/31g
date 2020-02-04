using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class sp_hybridizedOrbitals : OrbitalsBase
    {
        private int _superscriptNumber;

        public sp_hybridizedOrbitals(ShellBase myShell, int hybridizedCount) : this(myShell, hybridizedCount, 4)
        {

        }

        public sp_hybridizedOrbitals(ShellBase myShell, int hybridizedCount, int count) : base(myShell, count)
        {
            hybridizedCount = hybridizedCount < 0 ? 0 : hybridizedCount;
            hybridizedCount = hybridizedCount > count ? count : hybridizedCount;
            _superscriptNumber = hybridizedCount;

            for (var i = hybridizedCount; i < count; i++)
            {
                AssignedElectrons[i].Abbrev = "p";
            }
        }

        /// <summary>
        /// is called sp^3 because it a combination of one s-orbital and three p-orbitals
        /// </summary>
        protected internal override string Abbrev
        {
            get
            {
                string superscript;
                switch (_superscriptNumber)
                {
                    case 3:
                        superscript = "²";
                        break;
                    case 4:
                        superscript = "³";
                        break;
                    default:
                        superscript = "";
                        break;
                }

                return $"sp{superscript}";
            }
        }
        public override int CompareTo(OrbitalsBase other)
        {
            var bc = base.CompareShells(other);
            if (bc != null)
                return bc.Value;

            switch (other)
            {
                case s_Orbitals _:
                    return 1;
                case sp_hybridizedOrbitals _:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
