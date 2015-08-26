namespace NoFuture.Hbm.Templates
{
    public class HbmCommand
    {
        public static class ParamNames
        {
            public const string TypeFullName = "typeFullName";
            public const string IdTypeFullName = "idTypeFullName";
        }

        public string ClassName { get; private set; }
        public string IdTypeFullName { get; private set; }
        public string IdTypeTest { get; private set; }
        public string TypeFullName { get; private set; }
        public string OuputNamespace { get; private set; }

        public HbmCommand(string typeFullName, string idTypeFullName)
        {
            TypeFullName = typeFullName;
            IdTypeFullName = idTypeFullName;
            OuputNamespace = Util.TypeName.GetTypeNameWithoutNamespace(TypeFullName);
            IdTypeTest = Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest(IdTypeFullName, "DataId");
            ClassName = Util.TypeName.GetTypeNameWithoutNamespace(TypeFullName);
        }
    }
}
