using System;
using System.Text;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    /// <summary>
    /// A subdivision of an electron&apos;s <see cref="IShell"/>
    /// in which each <see cref="IShell"/> will have some
    /// number of orbitals and within each orbital is two
    /// electrons which have opposite spins.
    /// </summary>
    /// <remarks>
    /// The shape of an orbital shows where an electron _could_ be 90% of the time
    /// </remarks>
    public class Orbital
    {
        public Orbital(IOrbitalGroup of)
        {
            Of = of ?? throw new ArgumentNullException(nameof(of));
            SpinUp = new Electron();
            SpinDown = new Electron();
        }

        public IOrbitalGroup Of { get; }

        public string Abbrev { get; set; }
        public Electron SpinUp { get; }
        public Electron SpinDown { get; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine(".--.");
            str.Append("|");
            str.Append(SpinUp.IsPresent ? "↑" : " ");
            str.Append(SpinDown.IsPresent ? "↓" : " ");
            str.AppendLine("|");
            str.AppendLine("`--`");
            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            var o = obj as Orbital;
            if(o == null)
                return base.Equals(obj);

            return obj?.GetType() == GetType() && Of.Equals(o.Of);
        }

        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode() + Of.GetHashCode();
        }
    }
}
