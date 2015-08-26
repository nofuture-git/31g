using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomethingShared;

namespace ThirdDll
{
    public class DbLike
    {
        public SomethingShared.Entity00 GetEntityByArgs(SomethingShared.ArgsObject args)
        {
            if (args.Property03)
            {
                return new Entity00() {FirstName = string.Empty, LastName = string.Empty, Id = -1};
            }
            if (args.Property02 > 0)
            {
                return new Entity00() {FirstName = "Booty", LastName = "GotSmack", Id = 99};
            }

            switch (args.Property01)
            {
                case "":
                    return new Entity00() {FirstName = "More", LastName = "Info"};
                case SomethingShared.Globals.SomeGlobal:
                    return new Entity00() {FirstName = "AGlobal", LastName = "Reference", Id = args.Property04};
                default:
                    return new Entity00() {FirstName = "Sepee", LastName = "sflkjfd"};
            }
        }

        public string GetEntityData(SomethingShared.ArgsObject args)
        {
            if (args.Property03)
            {
                return SomethingShared.Globals.SomeGlobal;
            }

            return string.Format("{0} {1} {2:0000}", args.Property00, args.Property01, args.Property04);
        }
    }
}
