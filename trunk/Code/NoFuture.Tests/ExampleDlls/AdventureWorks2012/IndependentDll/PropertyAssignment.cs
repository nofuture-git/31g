namespace IndependentDll
{
    public class PropertyAssignment
    {
        public static void ForIlInspection()
        {
            var poco00 = new AnemicPoco00 {FirstName = "Eugene", LastName = "Krabs", PhoneNumber = "555-451-7784"};
            var poco01 = new AnemicPoco01();
            poco01.Fname = poco00.FirstName;
            poco01.Lname = poco00.LastName;
            poco01.Phnum = poco00.PhoneNumber;
        }
    }

    public class AnemicPoco00
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AnemicPoco01
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Phnum { get; set; }
    }
}
