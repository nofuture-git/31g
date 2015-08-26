using System.Collections;

namespace NoFuture.Shared
{
    public class RegexCatalog
    {
        public string CppClassMember { get { return Properties.Resources.CppClassMember; } }
        public string CsClass { get { return Properties.Resources.CsClass; } }
        public string CsClassMember { get { return Properties.Resources.CsClassMember; } }
        public string CsFunction { get { return Properties.Resources.CsFunction; } }
        public string EmailAddress { get { return Properties.Resources.EmailAddress; } }
        public string EmbeddedHtml { get { return Properties.Resources.EmbeddedHtml; } }
        public string IPv4 { get { return Properties.Resources.IPv4; } }
        public string JsFunction { get { return Properties.Resources.JsFunction; } }
        public string PhoneNumber01 { get { return Properties.Resources.PhoneNumber01; } }
        public string PhoneNumber02 { get { return Properties.Resources.PhoneNumber02; } }
        public string SqlSelectValues { get { return Properties.Resources.SqlSelectValues; } }
        public string SqlServerTableName { get { return Properties.Resources.SqlServerTableName; } }
        public string SSN { get { return Properties.Resources.SSN; } }
        public string StringIsRegex { get { return Properties.Resources.StringIsRegex; } }
        public string StringLiteral { get { return Properties.Resources.StringLiteral; } }
        public string TimeValue { get { return Properties.Resources.TimeValue; } }
        public string Uri { get { return Properties.Resources.Uri; } }
        public string Url { get { return Properties.Resources.Url; } }
        public string USD { get { return Properties.Resources.USD; } }
        public string UsZipcode { get { return Properties.Resources.UsZipcode; } }
        public string VbClassMember { get { return Properties.Resources.VbClassMember; } }
        public string WindowsRootedPath { get { return Properties.Resources.WindowsRootedPath; } }
        public string LongDate { get { return Properties.Resources.LongDate; } }

        private static Hashtable _myRegex2Values = new Hashtable();

        public static Hashtable MyRegex2Values
        {
            get { return _myRegex2Values; }
            set { _myRegex2Values = value; }
        }
    }
}
