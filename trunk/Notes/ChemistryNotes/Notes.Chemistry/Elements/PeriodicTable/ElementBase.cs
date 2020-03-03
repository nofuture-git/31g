using System;
using System.Collections.Generic;
using System.Linq;
using Notes.Chemistry.Elements.ElectronCfg.Shells;

namespace Notes.Chemistry.Elements.PeriodicTable
{
    public abstract class ElementBase : IElement
    {
        protected ElementBase()
        {
            AtomId = Guid.NewGuid();
            Shells.Add(new KShell(this));
            if (AtomicNumber > 2)
                Shells.Add(new LShell(this));
            if (AtomicNumber > 10)
                Shells.Add(new MShell(this));
            if (AtomicNumber > 28)
                Shells.Add(new NShell(this));
            if (AtomicNumber > 60)
                Shells.Add(new OShell(this));
            for (var i = 0; i < AtomicNumber; i++)
            {
                AddElectron();
            }

        }
        /// <summary>
        /// The id of an individual atom of this element
        /// </summary>
        public Guid AtomId { get; }

        /// <summary>
        /// The number of protons in the
        /// atom - the defining property of an element
        /// </summary>
        public abstract byte AtomicNumber { get; }
        public abstract string Symbol { get; }
        public virtual string Name => GetType().Name;
        public abstract double AtomicMass { get; }
        public abstract double Electronegativity { get; }
        public virtual Phase RoomTempPhase => Phase.solid;
        public virtual bool IsArtificial => false;
        public virtual bool IsRadioactive => false;

        public SortedSet<IShell> Shells { get; } =
            new SortedSet<IShell>();

        public IShell ValenceShell => Shells.Last();

        public int MaxElectrons
        {
            get
            {
                var max = 0;
                foreach (var shell in Shells)
                {
                    max += shell.ShellMaxElectrons;
                }

                return max;
            }
        }

        /// <summary>
        /// mnemonic &apos;Cats have paws&apos; as &apos;Cations are pawsitive&apos;
        /// </summary>
        public bool IsCation => AtomicNumber > CountElectrons;

        /// <summary>
        /// mnemonic &apos;A negative ion&apos; as &apos;An-egative-ion&apos;
        /// </summary>
        public bool IsAnion => AtomicNumber < CountElectrons;
        public bool IsIon => IsCation || IsAnion;

        public int? AddElectron()
        {
            foreach (var shell in Shells)
            {
                var added = shell.AddElectron();
                if (added != null)
                    return added;
            }

            return null;
        }

        public int? RemoveElectron()
        {
            foreach (var shell in Shells.Reverse())
            {
                var removed = shell.RemoveElectron();
                if (removed != null)
                    return removed;
            }

            return null;
        }

        public virtual int CountElectrons
        {
            get
            {
                var count = 0;
                foreach (var shell in Shells)
                    count += shell.GetCountElectrons();
                return count;
            }
        }

        public virtual int CountValences => (ValenceShell.ShellMaxElectrons - ValenceShell.GetCountElectrons()) % 8;

        public virtual double GetGrams(double countOfAtoms)
        {
            return (AtomicMass / 6.0221409e+23) * countOfAtoms;
        }

        public virtual double GetAtoms(double grams)
        {
            return (6.0221409e+23 / AtomicMass) * grams;
        }

        public virtual string PrintElectronShellCfg(bool shortVersion = true)
        {
            var strs = new List<string>();
            foreach (var iShell in Shells)
            {
                var shell = iShell as ShellBase;
                if (shell == null)
                    continue;
                var cfgs = shortVersion ? shell.GetElectronCfgShort() : shell.GetElectronCfgLong();
                strs.AddRange(cfgs);
            }

            return string.Join(" ", strs);
        }

        public override int GetHashCode()
        {
            return AtomId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var atom = obj as IElement;
            if (atom == null)
                return base.Equals(obj);

            return AtomId == atom.AtomId;
        }
    }
}
