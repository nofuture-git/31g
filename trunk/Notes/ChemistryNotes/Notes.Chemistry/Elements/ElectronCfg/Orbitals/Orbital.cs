using System;
using System.Text;

namespace Notes.Chemistry.Elements.ElectronCfg.Orbitals
{
    public class Orbital
    {
        public Orbital(IOrbitals of)
        {
            Of = of ?? throw new ArgumentNullException(nameof(of));
            SpinUp = new Electron();
            SpinDown = new Electron();
        }

        public IOrbitals Of { get; }

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
