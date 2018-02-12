using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
    //added for LINQ to SQL - see Example3
using System.Data.Linq;
using System.Data.Linq.Mapping;
//------------------------------------
//added for LINQ to XML - see Example4
using System.Xml.Linq;
//------------------------------------

namespace Notes_LINQ_Common
{
	public static class CommonLinqExpressions
	{
		public static void Examples()
		{
			//determine if all one list contains all the same element of the other
			var oneList = new List<int> {1,2,3};
			var anotherList = new List<int> {3,2,1};
			var eachItemPresentInBoth = oneList.Any(itemOne => anotherList.All(otherItem => otherItem != itemOne));
			
			//powershell'esque array splice
			var myList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var lastIndex = myList.Count - 1;
            var myI = 7;

            var left = myList.Take(myI).ToList();
            var right = myList.Skip(myI+1).Take(lastIndex).ToList();
            left.AddRange(right);

            var printMySkipTakeList = string.Join(",", left); //1,2,3,4,5,6,7,9,10,11,12,13,14,15,16
			
			//Basic Set operations using a Dictionary
			var A = new Dictionary<string, List<string>>
            {
                {"dbo.Table00", new List<string> {"column10", "column11", "column12", "column00"}}
            };
            var B = new Dictionary<string, List<string>>
            {
                {"dbo.Table10", new List<string> {"column10", "column11", "column02"}}
            };

            System.Diagnostics.Debug.WriteLine("Union");

            var union = A.SelectMany(x => x.Value).Union(B.SelectMany(y => y.Value));
            foreach (var r in union)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Intesect");
            var intersect = A.SelectMany(x => x.Value).Intersect(B.SelectMany(y => y.Value));
            foreach(var r in intersect)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Set Difference");
            var setDiff = A.SelectMany(x => x.Value).Except(B.SelectMany(y => y.Value));
            foreach(var r in setDiff)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Symetric Difference");
            var symetricDiff =
                A.SelectMany(x => x.Value)
                    .Except(B.SelectMany(y => y.Value))
                    .Union(B.SelectMany(y => y.Value).Except(A.SelectMany(x => x.Value)));

            foreach(var r in symetricDiff)
                System.Diagnostics.Debug.WriteLine(r);
			
			//use Alist.Select(x => x).Except(BList.Select(y => y)).ToList(); like
			// pattern when the set ops are just on simple lists
            
            //get an initialized array of integers
            var myInts = System.Linq.Enumerable.Range(0,30); //makes array like {0,1,2,...29};
            
            //zip array's
            var mynames = new[]{"first","second","third", "forth"};
            var myValues = new[]{0.22,0.56,0.15,0.07};
            var zipped = mynames.Zip(myValues, (n,v) => new Tuple<string,double>(n,v)).ToList();
            
		}
	}
}


namespace Notes_basic_LINQ
{
    public static class MyBasicLinq
    {
        /*============Example 1 LINQ to Objects========*/
        public static void Example1()
        {
            List<int> myList = new List<int> { 1, 2, 3 };

            //var intends that return type is not known and the compiler needs to figure it out
            //LINQ query expressions always begin with 'from' contextual keyword
            //'select' is after 'from' so the compiler may determine the type before looking for anything
            var qry = from number in myList where number < 3 select number;
            foreach (var num in qry) {}

            //this is valid but considered bad practice since opportunities are lost
            //IEnumerable<T> is what LINQ to Object queries run against
            IEnumerable<int> iQry = from number in myList where number < 3 select number;

            //this is also valid but bad manners
            List<int>.Enumerator e = myList.GetEnumerator();
            while (e.MoveNext()) {}
        }
        /*=============================================*/


        /*==Example 2 LINQ to Objects (Anonymous Types)*/
        /*example class*/
        public class Bucket { public double Prop1 { get; set; } public double Prop2 { get; set; } public double Prop3 { get; set; } }
        public static void Example2()
        {
            List<Bucket> buckets = new List<Bucket> { new Bucket { Prop1 = 1.1, Prop2 = 1.2, Prop3 = 1.3 }, 
                                                      new Bucket { Prop1 = 2.1, Prop2 = 2.2, Prop3 = 2.3 }, 
                                                      new Bucket { Prop1 = 3.1, Prop2 = 3.2, Prop3 = 3.3 } };
            //a repeat of Example1 but the 'where' is now testing on a property thereof
            var qry1 = from bucket in buckets where bucket.Prop3 == 3.3 select bucket;

            //this seems unrelated but its not - Anonymous Types are important to the 'where' portion of LINQ
            List<object> blank = new List<object> { new {Prop = "wtf," }, 
                                                    new {Prop = "where is the type?" }, 
                                                    new {Prop = "power of Anonymous Types - booyah" } };
            //qry2 has Anonymous Types - both the first and second Bucket meet the criteria 
            //the Anonymous Type is not a 'Bucket' but a compiler made type with a single property called 'AnyName' that returns a 'double'
            var qry2 = from bucket in buckets where bucket.Prop1 > 2.0 select new { AnyName = bucket.Prop1};
            //this will return the two matching bucket objects with all three properties in scope
            var qry3 =  from bucket in buckets where bucket.Prop1 > 2.0 select bucket;
        
        }
        /*=============================================*/


        /*============Example 3 LINQ to SQL============*/
        /*'Entity' - the name given to this class type that defines a database table
          - these are to map directly to a database table */
        [Table(Name = "dbo.KFIUserGroup")]
        public class UserGroup { 
            //these map to the fields and thier types of the given table
            [Column]public int UserGroup_PK; 
            [Column] public string UserGroupDesc;}
        [Table(Name = "WICPROGRAM")]
        public class WicProgram
        {
            [Column]
            public int WICPROGRAMID;
            [Column]
            public string NAME;

        }

        public static void Example3()
        {
            //defacto connection class for LINQ to Database
            DataContext db = new DataContext(@"Data Source=CSCDBSOVR010;Initial Catalog=KFILIVE;User Id=wbAdmin;Password=8ank1ng;");
            //'GetTable' is a method, spoken as 'Get Table of UserGroup'
            var qry1 = from c in db.GetTable<UserGroup>() where c.UserGroup_PK > 1 select new { UserGroupId = c.UserGroup_PK, GroupType = c.UserGroupDesc };
            foreach (var grp in qry1) { Console.WriteLine(grp); }


            Console.ReadLine();
        }
        /*=============================================*/


        /*============Example 4 LINQ to XML (Read)=====*/
        public static void Example4()
        {
            //here the entire document is going to be selected
            XDocument xmlWithoutNamespace = XDocument.Load(@"C:\Documents and Settings\bstarrett.CVNS\My Documents\Notes\Notes LINQ\Notes LINQ\01_IclRefTestFile.xml");
            var noNs = from x in xmlWithoutNamespace.Descendants() select x;
            Console.WriteLine("Select without namespace and no 'where' statement");
            foreach (var x in noNs) { Console.WriteLine(x); }

            //on this one there is a where statement limiting what is selected
            var noNs2 = from x in xmlWithoutNamespace.Descendants("Items") where x.Attribute("IclSeq").Value == "3" select x;
            Console.WriteLine("Select without namespace and 'where' statement on Items attr 'IclSeq' equals 3");
            foreach (var x in noNs2) { Console.WriteLine(x); }

            //namespace being present means that the item must have it as part of its name literal
            XDocument myXml = XDocument.Load(@"C:\Documents and Settings\bstarrett.CVNS\My Documents\Notes\Notes LINQ\Notes LINQ\02_IclRefTestFile.xml");
            //this is being cast as Anonymous Type using the XNamespace's static Get method.
            XNamespace csc = "http://www.csc.com";
            //the 'Items' node is now qualified with the namespace
            var xml = from x in myXml.Descendants(csc + "Items") where x.Attribute("IclSeq").Value == "3" select x;
            foreach (var x in xml){Console.WriteLine(x);}
            Console.ReadLine();
        }
        /*=============================================*/


        /*============Example 5 LINQ to XML (write)====*/
        public static void Example5()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                              new XElement("Customers", new XElement("Customer", new XAttribute("ContactName", "Maria Anders"),
                                                                                 new XAttribute("City", "Berlin")),
                                                        new XElement("Customer", new XAttribute("ContactName", "Ana Trujillo"),
                                                                                 new XAttribute("City", "Mexico D.F.")),
                                                        new XElement("Customer", new XAttribute("ContactName", "Antonio Moreno"),
                                                                                 new XAttribute("City", "Mexico D.F."))));
        }
        /*=============================================*/


        /*====Example 6 LINQ to Dataset via XML========*/
        public static void Example6()
        {
            var myDoc = XDocument.Load("");
            var qry = from x in myDoc.Elements(XName.Get("something")) select x;
            foreach (var m in qry) Console.WriteLine(m.Value);
            Console.ReadLine();

        }
    }


    class MyConsole
    {
        static void Main(string[] args)
        {
            try{
                MyBasicLinq.Example6();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
namespace original_Events
{
    /// <summary>
    /// Represents the effort of the work but not the work itself.
    /// </summary>
    /// <returns></returns>
    [Serializable]
    public delegate int GetToWork();

    /// <summary>
    /// Message event from Workers to Supervisor
    /// </summary>
    /// <param name="message"></param>
    [Serializable]
    public delegate void WorkerReportingInEvent(WorkerMessage message);

    /// <summary>
    /// Message from Supervisor to thier workers
    /// </summary>
    /// <param name="message"></param>
    [Serializable]
    public delegate void MessageFromSupervisor(SupervisorMessage message);


    /// <summary>
    /// Represents the container which sends out each <see cref="MyWorker"/> to do thier <see cref="WorkerLabor"/>.
    /// While keeping a duplex communication channel open.
    /// </summary>
    public class MySupervisor
    {
        internal object msgLock = new object();
        private readonly List<WorkerLabor> _workForce;

        public event MessageFromSupervisor CommLinkToWorkers;

        public MySupervisor()
        {
            _workForce = new List<WorkerLabor>();
        }

        /// <summary>
        /// This is the method invoked by a calling assembly to launch 
        /// a worker per call.  Call it five times and five threads are launched,
        /// call it four times then four threads, etc.
        /// </summary>
        public void StartWorker()
        {
            var workerId = string.Format("Worker #{0}", _workForce.Count);
            var newWorker = new MyWorker(workerId, this);
            Console.WriteLine("Sending {0} to work",workerId);
            var sentToWork = new WorkerLabor(newWorker);
            newWorker.MyEvent += MessageFromAWorker;
            sentToWork.MyEvent += MessageFromAWorker;
            _workForce.Add(sentToWork);
        }

        /// <summary>
        /// Each worker is a subscriber to the Supervisor's <see cref="CommLinkToWorkers"/>
        /// and are expected to handle messages sent from here.
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastToMyWorkers(SupervisorMessage message)
        {
            var subscribers = CommLinkToWorkers.GetInvocationList();
            var enumerable = subscribers.GetEnumerator();
            while (enumerable.MoveNext())
            {
                var handler = enumerable.Current as MessageFromSupervisor;
                if (handler == null)
                    continue;
                handler.Invoke(message);
            }
        }

        /// <summary>
        /// Each worker may send a message back to thier supervisor - messages 
        /// from workers are handled here.
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>
        /// While the communication between worker and supervisor is duplex, the 
        /// is no communication between distinct workers.
        /// </remarks>
        public void MessageFromAWorker(WorkerMessage message)
        {
            lock(msgLock)
            {
                var workerReporting = _workForce.FirstOrDefault(wkr => wkr.Worker.Name == message.Name);
                if (workerReporting == null || workerReporting.Worker == null)
                    return;
                Console.WriteLine(message.Message);
                workerReporting.Worker.Status = message.State;
            }
        }

        /// <summary>
        /// A summary kind of report to list the state of all workers
        /// </summary>
        public void ListAllWorkersStatus()
        {
            foreach (var wkr in _workForce)
            {
                Console.WriteLine("{0} is '{1}'", wkr.Worker.Name, Enum.GetName(typeof(WorkerState), wkr.Worker.Status));
            }
        }
    }

    [Serializable]
    public enum WorkerState
    {
        Starting,
        Working,
        Done,
        Exited
    }

    [Serializable]
    public enum SupervisorState
    {
        ReportIn,
        PrepareToQuit,
        Abort,
    }

    [Serializable]
    public class WorkerMessage
    {
        public string Message;
        public string Name;
        public WorkerState State;
    }

    [Serializable]
    public class SupervisorMessage
    {
        public string Message;
        public SupervisorState AnOrder;
    }

    /// <summary>
    /// A container class reprsenting the async effort of any given <see cref="MyWorker"/> instance.
    /// </summary>
    /// <remarks>
    /// The <see cref="GetToWork"/> delegate is bound to its respective EndInvoke handler by this class.
    /// It represents the effort of the work but not the work itself - the work itself is performed by the
    /// <see cref="MyWorker"/>
    /// </remarks>
    public class WorkerLabor
    {
        public event WorkerReportingInEvent MyEvent;

        private readonly GetToWork _workStation;
        private readonly MyWorker _myWorker;

        public MyWorker Worker { get { return _myWorker; } }

        public WorkerLabor(MyWorker theWorker)
        {
            _myWorker = theWorker;
            _workStation = theWorker.DoMyWork;
            var rslt = _workStation.BeginInvoke(CallBackHere, theWorker.Name);
        }

        /// <summary>
        /// Since the <see cref="MyWorker"/> is contained by this type, a view to the thread closing is possiable.
        /// The worker themselves could never report this since they are bound to the very thread that is closing.
        /// </summary>
        /// <param name="z"></param>
        private void CallBackHere(IAsyncResult z)
        {
            _workStation.EndInvoke(z);
            MyEvent.Invoke(new WorkerMessage { Message = string.Format("{0} is exiting", _myWorker.Name), Name = _myWorker.Name, State = WorkerState.Exited });
        }

    }

    /// <summary>
    /// Represents the general worker who performs whatever effort.
    /// </summary>
    public class MyWorker
    {
        public event WorkerReportingInEvent MyEvent;
        private readonly string _name;
        private readonly MySupervisor _mySupervisor;
        public WorkerState Status { get; set; }

        /// <summary>
        /// This ctor defines the worker and who they report to; however, the worker still needs to be instructed to start working.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="supervisor"></param>
        public MyWorker(string name, MySupervisor supervisor)
        {
            _name = name;
            _mySupervisor = supervisor;
            _mySupervisor.CommLinkToWorkers += ReceiveSupervisorMessage;
            this.Status = WorkerState.Starting;
        }

        /// <summary>
        /// An identifier of the given worker
        /// </summary>
        public string Name { get { return _name; }}

        /// <summary>
        /// This is the very target of the parallel processing.
        /// </summary>
        /// <returns></returns>
        public int DoMyWork()
        {
            Thread.CurrentThread.Name = Name;

            //this the worker reporting back to the controller thier status
            this.Status = WorkerState.Working;
            MyEvent.Invoke(new WorkerMessage { Message = string.Format("{0} is working hard", Name), Name = Name, State = this.Status });

            //represents the work being done
            System.Threading.Thread.Sleep(1000);//a second

            this.Status = WorkerState.Done;
            MyEvent.Invoke(new WorkerMessage { Message = string.Format("{0} is done working", Name), Name = Name, State = this.Status });
            return 1;
        }

        /// <summary>
        /// This is where <see cref="SupervisorMessage"/>(s), broadcast to all workers from the <see cref="MySupervisor"/>, are handled.
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveSupervisorMessage(SupervisorMessage message)
        {
            switch (message.AnOrder)
            {
                case SupervisorState.Abort:
                    break;

                case SupervisorState.PrepareToQuit:
                    break;

                case SupervisorState.ReportIn:
                    MyEvent.Invoke(new WorkerMessage { Message = string.Format("{0} reporting in", Name), Name = Name, State = this.Status });
                    break;
            }
        }
    }
}

namespace basic_Lamda
{
    public class OldSchoolDelegate
    {
        //In C this is like: typedef bool (*myDelegate) ();
        private delegate bool myDelegate();
        
        //In C this is like:  myDelegate myDelRef; 
        private myDelegate myDelRef;

        private delegate string myComplexDelegate(string delParam1, string[] delParam2);

        private myComplexDelegate myComplexDelRef;

        public void EncloseTheAction()
        {
            //In C this is like:  myDelRef = FunctionBeingPointedTo;
            myDelRef = new myDelegate(FunctionBeingPointedTo);
            //In C this is like:  bool aReturnValue = myDelRef();
            var aReturnValue = myDelRef.Invoke();

            myComplexDelRef = new myComplexDelegate(ComplexFunctionBeingPointedTo);
            var stringReturn = myComplexDelRef.Invoke("param1", new string[] {"param2", "param3"});
        }

        public bool FunctionBeingPointedTo()
        {
            return true;
        }

        public string ComplexFunctionBeingPointedTo(string param1, string[] param2)
        {
            return "whatever";
        }
    }
	
	//new with .NET 2.0
	public class AnonymousDelegate
	{
		//still need the delegates but no the functions being pointed to
		delegate bool simpleFunctionPointer();
		delegate string complexFunctionPointer(string param1, string[] param2);
		public void EncloseTheAction()
		{
			//the body of the functions being pointed to have been implemented inline
			simpleFunctionPointer myDelRef = delegate(){ return true; };
			
			complexFunctionPointer myComplexDelRef = delegate(string param1, string[] param2){return "whatever"; };
		}
	}

    //new with .NET 3.5
    public class TheFunc
    {
		//delegates are gone but still need the functions being pointed to
        public void EncloseTheAction()
        {
			//delegate declaration, followed by invoke
            Func<bool> simpleFunc = FunctionBeingPointedTo;
            var aReturnType = simpleFunc();
			//declaration and invocation in a single line
            aReturnType = (new Func<bool>(FunctionBeingPointedTo)());

            Func<string, string[], string> complexFunc = ComplexFunctionBeingPointedTo;//args, then return type
            var complexReturnType = complexFunc("param1", new string[] {"param2", "param3"});
			complexReturnType = (new Func<string, string[], string>(ComplexFunctionBeingPointedTo)("param1", new string[] { "param2", "param3" }));
        }

        public bool FunctionBeingPointedTo()
        {
            return true;
        }
        public string ComplexFunctionBeingPointedTo(string param1, string[] param2)
        {
            return "whatever";
        }
    }
	
	public class LamdaOperator
	{
		//the delegates and the functions being pointed to are all gone.
		public void EncloseTheAction()
		{
			Func<bool> myDelRef = () => {return true; };
			Func<string,string[],string> myComplexDelRef = (x,y) => {return "whatever"; };
		}
		
	}
}

namespace Notes_advanced_LINQ
{
	/*
		LINQ Unleashed for C#
		By: Paul Kimmel
		Publisher: Sams
		Pub. Date: August 19, 2008
		Print ISBN-10: 0-672-32983-2
		Print ISBN-13: 978-0-672-32983-8
		Web ISBN-10: 0-7686-8538-9
		Web ISBN-13: 978-0-7686-8538-1
		Pages in Print Edition: 552
	*/
	
	/*
	History
	- Brian Kernighan and Dennis Ritchie created function-pointers in C.
	- Function-pointers led to events and event-handlers
	- Event-handlers evolved into delegates in .NET
	- Delegates were compressed into Anonymous delegates in .NET 2.0
	- Anonymous delegates evolved into LINQ
	*/
	public class MyClass
    {
        public int Number { get; set; }
        public string Letters { get; set; }

        public MyClass()
        {
            Number = 0;
            Letters = String.Empty;
        }

        public MyClass(int Number, string Letters)
        {
            this.Number = Number;
            this.Letters = Letters;
        }

        public string ProcessIt()
        {
            return "done!";
        }
    }
	
	public class AndPredicate
	{
		public void EncloseTheAction()
		{
			MyClass[] args = new MyClass[]
                                 {
                                     new MyClass(1, "1"), new MyClass(2, "2"), new MyClass(3, "3"),
                                     new MyClass(4, "you found me")
                                 };
            //predicate is a delegate - used mostly for arrays
            Predicate<MyClass> myPredicate = new Predicate<MyClass>(FindSomething);
            var myClass = Array.Find(args, myPredicate);
			
			//Func, Action and Predicate may all be assigned to LINQ literals
			 Predicate<MyClass> myOtherPredicate = x => x.Letters == "you found me";
			 myClass = Array.Find(args, myOtherPredicate);
			
		}
	
		public static bool FindSomething(MyClass arg)
		{
			if (arg.Letters == "you found me") return true;
			return false;
		}
	}
	
	public class TheLamdaOperator
	{
		//read '=>' as 'goes to'
		//Func, Action and Predicate may all be assigned to Lamda literals
		public void EncloseTheAction()
		{
			//returns true or false and only ever has one type in generic declaration
			Predicate<MyClass> myPredicate = x => x.Letters == "you found me";
			//predicate indicates a question on a subject, this is a function
			//Predicate<double, double> willNotCompile = (x,y) => x > y; 
			
			//performs something using one of the args it recieves
			//this is simply as named an action that returns void.
			Action<string, MyClass> doStuff = (s,myCls) => myCls.Letters = s;

            //returns something (don't forget to include the return type at the end of Func<...>
		    Func<string, MyClass,string> getFunc = (s, myCls) => { return "whatever"; };
		}

        internal delegate void MyEvent();

	    internal MyEvent myEvent;

        public void LamdaAndEventSubscription()
        {
            //add a subscriber to an event using lamda
            myEvent += () => Console.WriteLine("an event");
        }
	}

    public class YieldReturn
    {
        //instead of having another array to copy the matching items
        //this syntax allows for a quick, "return just the matches"
        public static IEnumerable<MyClass> TheYeildReturn()
        {
            var args = new MyClass[]
                                 {
                                     new MyClass(1, "1"), new MyClass(2, "2"), new MyClass(3, "3"),
                                     new MyClass(4, "you found me")
                                 };

            foreach (var myClass in args)
            {
                if(myClass.Letters == "1" || myClass.Letters == "2")
                {
                    //RULES
                    //must return the Iterator type
                    //must appear in a loop (or other iteration construct)
                    //not allowed within 'unsafe' enclosures
                    //not allowed within try/catch enclosures
                    yield return myClass;
                }
                    
            }
        }
    }

    public class Closures
    {
       //when local variables are used in a lambda expression a 'Closure' is required
       //this is performed perchance the expression is returned from a function having
        // the expression dependent on a local variable 

        public Func<string, MyClass,string> ThisWillRequireAClosure()
        {
            string aLocal = "ing";
            return (x, y) => { return aLocal; };
        }

    }
	//any function which may take or return (or both) a function
	public class HigherOrderFunctions
	{
		/* mathematically:
			y = f(x)
			z = g(y)
			
			z = g(f(x))
			
			could be defined in C# as:
		*/
		
		//this is returning a function which, in turn, needs to be invoked
		public static Func<X, Z> Compose<X, Y, Z>(Func<X, Y> f, Func<Y, Z> g)
		{
			return (x) => g(f(x));
		}
		
		public static UseExpressions(){
			Func<double, double> sin = Math.Sin; //these are functions, not static values
			Func<double, double> exp = Math.Exp;
			Func<double, double> expSin = Compose(sin, exp);
			
			double y = expSin(3);
		}
	}
	
	//a curried function is a function that returns a function as its result
	// this is a kind of Higher-Order function 
    public class Currying
    {
        public void EncloseExample()
        {
            Func<int, Func<int, int>> myCurry = x => y => y + x;
			//note: Func<Func<int, int>, int> 
			// is not a curried function since it does not return a function but, instead returns an int
			// it is, however, a Higher-Order function
			
			Func<double, double, Func<double, double, double>> mySecondCurry = (x, y) => (i, j) => i * x + j * y;

            //strange but true
            var curryResult = myCurry(1)(2);
			var mySecondCurryResult = mySecondCurry(4.0D, 5.0D)(-1.0D, -2.0D);

            Func<List<string>, Func<List<string>, List<string>>> myComplexCurry = x => y => x.Where(i => y.Contains(i)).ToList();

            var stringArgs = (new [] {"hello", "goodbye", "nevermind"}).ToList();
            var string2Args = (new[] {"adios","tre bein","goodbye"}).ToList();
			
			//yep,
            var complexCurryResult = myComplexCurry(stringArgs)(string2Args);
        }
    }
	
    public class MyExpression
    {
        public void MyExpressionExample()
        {
            //define code from code - known as expression tree
            BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(8D));

            //to resolve the expression outright we must convert to a lamda and compile
            Expression<Func<double>> myLamda = Expression.Lambda<Func<double>>(be);

            Func<double> myCompiledExpression = myLamda.Compile();

            //now have a function pointer to resolve upon
            double myResult = myCompiledExpression();
        }

        public void NextExample()
        {
            //define x + y as code in code, first define x and y
            ParameterExpression pe = Expression.Parameter(typeof(int), "x");
            ParameterExpression p2 = Expression.Parameter(typeof(int), "y");

            //having the operands, define the operator
            BinaryExpression mySumExpression = Expression.Add(pe, p2);

            //to get the expression into a callable form it must become a lambda 
            Expression<Func<int, int, int>> mySumFxPtrExpression = Expression.Lambda<Func<int, int, int>>(mySumExpression,
                                                                                                new ParameterExpression
                                                                                                    [] { pe, p2 });
            //and that lambda must be compiled
            Func<int, int, int> mySumFxPtr = mySumFxPtrExpression.Compile();

            //resulting in a function made from code
            int result = mySumFxPtr(2, 34);
        }

        public void AnotherExample()
        {
            //a literal lambda expression returning a bool taking an int
            Func<int, bool> literalIsOdd = i => (i & 1) == 1;

            //an expression defined as a lambda using a lambda literal which doesn't have a expression body
            Expression<Func<int, bool>> isOdd = i => (i & 1) == 1;

            //notice:  these do not compile
            //Expression<Func<int, bool>> isOdd = literalIsOdd;//direct assignment
            //Expression<Func<object, object>> hasAnExpressionBody = x => {return x;}
        }
		
		/*
		Programming Microsoft® LINQ in Microsoft .NET Framework 4
		By: Paolo Pialorsi and Marco Russo
		Publisher: Microsoft Press
		Pub. Date: November 23, 2010
		Print ISBN-10: 0-7356-4057-2
		Print ISBN-13: 978-0-7356-4057-3
		Web ISBN-10: 0-7356-5676-2
		Web ISBN-13: 978-0-7356-5676-5
		Pages in Print Edition: 650		
		*/
		
        public static void ExampleTreesAreImmutable()
        {
            //as immutable each node above to replacement-target will have to be itself replace up to the top
            Expression<Func<int, int>> myFormula = (n) => (n * 2 + 1) * 4;

            Console.WriteLine(myFormula.ToString());//will print the body literal adding paraenths for order 'n => (((n * 2) + 1) * 4)'
            /*
    (
      (
       (
        n  --(A) ParameterExpression --*
        *                              |-- (C) BinaryExpression [Multiply] --*
        2  --(B) ConstantExpression ---*                                     |
       )                                                                     |-- (E) BinaryExpression [Addition] ---*
       +                                                                     |                                      |
       1 --(D) ConstantExpression  ------------------------------------------*                                      |-- (G) BinaryExpression [Multiply]
      )                                                                                                             |
      *                                                                                                             |
      4 -- (F) ConstantExpression ----------------------------------------------------------------------------------*
    )
             */

            var top = myFormula.Body; //this must be (G)

            var replace4With5 = Expression.Constant(5); //this is the replacement for (F)

            var leftSide = (top as BinaryExpression).Left; //this is a reference to (E)

            var treesAreImmutable = Expression.MakeBinary(top.NodeType, leftSide, replace4With5);

            var replace2with3 = Expression.Constant(3); //this is the replacement for (D)

            var refTo_3 = ((top as BinaryExpression).Left as BinaryExpression).Left;//this is a ref to (C)

            var newSum = Expression.MakeBinary((top as BinaryExpression).Left.NodeType, refTo_3, replace2with3); //this a new version of (E)

            var newTop = Expression.MakeBinary(top.NodeType, newSum, (top as BinaryExpression).Right);//this a new version of (G)
        }
        public static void ExampleDiassembleExpressionTree()
        {
            Expression<Func<double, double, double>> TriangleAreaExp = (b, h) => b * h / 2;

            //An expression tree from a lambda expression always has a top level node of Expression<T>
            // - this top-lvl expression has two parts,
            //   - the Body 
            //   - Parameters, which is an array

            //BinaryExpression concerns the number of operands (binary ~ 2), not the actual types, 
            // and therefore has the properties Left and Right

            //ConditionalExpression has three child nodes, and therefore three properties
            // viz. Test, IfTrue and IfFalse
            // as such Expression Trees are not binary trees 

            var body = TriangleAreaExp.Body;
            var expectingBinaryExpression = body.NodeType;
            Console.WriteLine(expectingBinaryExpression);//Divide

            //System.Linq.Expressions.ExpressionVisitor is not straight-foward regarding how its 
            // supposed to be used, an older 3.5 implemenation is available at 
            //http://msdn.microsoft.com/en-us/library/bb882521(v=vs.90).aspx
            //another example from the text is at
            //http://blogs.msdn.com/b/jomo_fisher/archive/2007/05/23/dealing-with-linq-s-immutable-expression-trees.aspx
        }
    }
	
	/*
	 EXAMPLE, this could be used to avoid all the null-ref checking but is very limited to this object graph
	*/
	public class EmailAddress
    {
        public string Address;
        public EmailTypeEnum Type;
    }
    public class MyEntityData
    {
        public string Code;
        public string AnotherCode;
        public EmailAddress[] Emails;
        public string Name;
    }
	public class MyTransactionData
	{
		public string FirstName;
		public string LastName;
		public MyEntityData Requestor;
	}
	
	public static class MyExtensionMethods
	{
        public static object OrDbNull(this object anyThing, Expression<Func<object, object>> someEval)
        {
            if (someEval == null)
                return DBNull.Value;

            var body = someEval.Body;
            
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                if (((MemberExpression)body).Expression.NodeType == ExpressionType.ArrayIndex)
                {
                    var shouldBeIndex = ((BinaryExpression)((MemberExpression)body).Expression).Right;
                    var shouldBeThirdPropery =
                        ((MemberExpression)((BinaryExpression)((MemberExpression)body).Expression).Left).Member
                            .Name;
                    var shouldBeSecondProperty =
                        ((MemberExpression)
                            ((MemberExpression)((BinaryExpression)((MemberExpression)body).Expression).Left)
                                .Expression).Member.Name;
                    var secondPropertyValue = anyThing.GetType().GetField(shouldBeSecondProperty).GetValue(anyThing);
                    if (secondPropertyValue == null)
                        return DBNull.Value;
                    var shouldBeAnArray = secondPropertyValue.GetType().GetField(shouldBeThirdPropery).GetValue(secondPropertyValue);
                    if (shouldBeAnArray == null || ((Array) shouldBeAnArray).Length == 0)
                        return DBNull.Value;
                    var shouldBeThirdPropertyType = ((Array) shouldBeAnArray).GetValue((int) ((ConstantExpression) shouldBeIndex).Value);
                    var shouldBeThirdPropertyValue = shouldBeThirdPropertyType.GetType().GetField(((MemberExpression) body).Member.Name);
                    return shouldBeThirdPropertyValue;
                }
            }

            return DBNull.Value;
        }
		
		public static void TestProof()
		{
		    var testData = new MyTransactionData {FirstName ="Fname", LastName="Lname"};
			
			var andThis = testData.OrDbNull(x => ((MyTransactionData)x).Requestor.Emails[0].Address);
		}
		
	}
	
    public class MyQueryableExpression
    {
        public void ExpressionExample()
        {
            //the operands
            var myTestNull = new MyClass[]
                                 {
                                     new MyClass {Letters = "someValue"},
                                     new MyClass {Letters = "someotherValue"},
                                     new MyClass {Letters = "athirdValue"}
                                 };

            //transform operands into an IQueryable using the extension method
            IQueryable<MyClass> myQry = myTestNull.AsQueryable<MyClass>();

            //parameter expression gives the other expression a sense of context
            ParameterExpression pe = Expression.Parameter(typeof(MyClass), "t");

            //define the operands proper
            Expression left = Expression.Property(pe, typeof(MyClass).GetProperty("MyProp"));
            Expression right = Expression.Constant("someValue");

            //define the operator upon which resolution is found
            Expression eqSomeValue = Expression.Equal(left, right);

            //many choices for manner of the operands
            left = Expression.Call(pe, typeof(MyClass).GetMethod("ProcessIt", System.Type.EmptyTypes));

            //as well as the operators
            Expression eqProcessIt = Expression.NotEqual(left, right);

            //operators may be used in a compound fashion, linking statement blocks (i.e. operands and operator) as well as individual operands
            Expression predicateBody = Expression.OrElse(eqSomeValue, eqProcessIt);

            //effectively composed code from code 
            MethodCallExpression myWhere = Expression.Call(typeof(Queryable),
                                                           "Where",
                                                           new Type[] { myQry.ElementType },
                                                           myQry.Expression,
                                                           Expression.Lambda<Func<MyClass, bool>>(predicateBody,new ParameterExpression[] { pe }));


            //now resolve the generated expression upon the original operands - without the use of lambda.Compile()
            IQueryable<MyClass> results = myQry.Provider.CreateQuery<MyClass>(myWhere);

        }
    }

    public class MockingAnExpression
	{
		public void EncloseAnExample()
		{
		    var dataContextMock = new DataContextMock();
		    var myExample = new MyClass {Letters = "uk pop brigade", Number = 12};
            Expression<Func<MyClass, bool>> expectedExpression = mc =>
                                            mc.Letters == "don't...stop...me...now"
                                            && mc.Number == 12;

            /*linchpen :: 
             * and lambda expression (expression) which takes onto itself a lambda expression (x) and 
             * returns a bool and in which, therefore, the former is the recipient of the latter.
            */
            Expression<Func<Expression<Func<MyClass, bool>>, bool>> expression =
                x => x.Compile().Invoke(myExample) == expectedExpression.Compile().Invoke(myExample);

            //finalize test expectations
            var mockGetWhereResults = new[] { myExample };
            dataContextMock.Setup(d => d.GetWhere(It.Is(expression))).Returns(mockGetWhereResults.AsQueryable<MyClass>());
		}
	}

    public static class It
    {
        public static object Is(Expression<Func<Expression<Func<MyClass, bool>>, bool>> expression)
        {
            return new object();
        }
    }

    public class DataContextMock
    {
        public DataContextMock Setup(Func<ActualContext, object> func)
        {
            return new DataContextMock();
        }
        public void Returns(object arg)
        {
            return;
        }
    }
    public class ActualContext
    {
        public MyClass GetWhere(object o)
        {
            return new MyClass();
        }
    }
}

namespace using_LINQ_with_generics
{
    [Serializable]
    public class Agency
    {
        public string Name { get; set; }
        public string Code;
        public string Phone;

    }

    [Serializable]
    public class Applicant
    {
        public string FirstName;
        public string LastName;
    }

    [Serializable]
    public class Order
    {
        public Agency Agency;
        public Applicant Applicant;
    }

    public class Registry<TEntity, TData> where TEntity : new()
    {
        private readonly TEntity _entity;
        private readonly PropertyInfo[] _pis;
        private readonly FieldInfo[] _fis;
        private Dictionary<string, Func<TData, object>> _propToExprHash;

        public Registry()
        {
            _entity = new TEntity();
            _pis = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            _fis = typeof(TEntity).GetFields(BindingFlags.Instance | BindingFlags.Public);
            _propToExprHash = new Dictionary<string, Func<TData, object>>();
        }

        public TEntity Entity { get { return _entity; } }

        public void MyMapping(Expression<Func<TEntity, object>> target, Func<TData, object> source)
        {
            
            var body = target.Body;
            string memberName = null;
            if(body.NodeType == ExpressionType.MemberAccess)
            {
                var linqMemberExpr = body as MemberExpression;
                if (linqMemberExpr == null)
                    return;

                memberName = linqMemberExpr.Member.Name;
            }
            if (body.NodeType == ExpressionType.Convert)
            {
                var linqUnaryExpr = body.Reduce() as UnaryExpression;
                if (linqUnaryExpr == null)
                    return;

                var unaryOpand = linqUnaryExpr.Operand.Reduce();
                var memberExpr = unaryOpand as MemberExpression;
                if (memberExpr == null)
                    return;

                memberName = memberExpr.Member.Name;
            }

            if (string.IsNullOrEmpty(memberName) || _propToExprHash.Keys.Any(x => x == memberName))
                return;

            _propToExprHash.Add(memberName, source);

        }

        public TEntity ResolveToEntity(TData data)
        {
            foreach (var memberName in _propToExprHash.Keys)
            {
                var result = _propToExprHash[memberName](data);
                var pi = _pis.FirstOrDefault(p => p.Name == memberName);
                if (pi != null)
                {
                    pi.SetValue(_entity, result, null);
                    continue;
                }
                var fi = _fis.FirstOrDefault(f => f.Name == memberName);
                if (fi != null)
                {
                    fi.SetValue(_entity, result);
                }
            }
            return _entity;
        }
    }
     
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                var testData = @"AY01      AGENCY NAME HERE.                           8885550170            8885550270                                              ";
                var testRegistry = new Registry<Agency, string>();

                testRegistry.MyMapping(agy => agy.Name, d => d.Substring(11,43));
                testRegistry.MyMapping(agy => agy.Phone, d => d.Substring(54,10));

                var testResult = testRegistry.ResolveToEntity(testData);
                Console.WriteLine(testResult.Name);
                Console.WriteLine(testResult.Phone);

                var testRegContainer = new Registry<Order, string[]>();

                var testOrderData = new[]
                                    {
                                        "OR01V01OO2805900                       CASEONE                                                                                N     ",
                                        "AN01single                        vscombo             F                         55555555         FC                                 ",
                                        "AN028205 S CASS TEST                        8885550170       8885550170            United States       US   8885550170              ",
                                        "AN03                                                                                ANY TOWNNAME               IL77777              ",
                                        "AN04                                                                     OOOOOO@DOMAINNAME.COM                                      ",
                                        "CO01IOOP4830      Some long name appears here                           1555555                                                     ",
                                        "AY01      AGENCT NAME, INC.                           8885550170            8885550270                                              ",
                                        "AG01000000    UNKNOWN                  FNAME LNAMED                                                                                 ",
                                        "PI011 00000200000                                                                                    NNN        NN                  ",
                                        "PC01EXAM1     QQQQQQQ RR VVVVVVVVVVV                  Y                                                                             ",
                                        "SR01    000       TT Collection day 1                                                                                               ",
                                        "SR02    000       YY Collection day 2                                                                                               ",
                                        "SE0120110307200000                                                                                                                  "

                                    };

                testRegContainer.MyMapping(o => o.Agency, lns => testRegistry.ResolveToEntity(lns.FirstOrDefault(ln => ln.Substring(0,2) == "AY")));

                var testOrderRslt = testRegContainer.ResolveToEntity(testOrderData);
                Console.WriteLine(testOrderRslt.Agency == null);
                Console.WriteLine(testOrderRslt.Agency.Name);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
    public class SetOps
    {
        #region dependent on GetHashCode
        public static List<object> Intersect(List<object> leftList,
            List<object> rightList)
        {
            Func<object, int> identity = x => x.GetHashCode();
            return Intersect(leftList, rightList, identity);
        }

        public static List<object> Union(List<object> leftList,
            List<object> rightList)
        {
            Func<object, int> identity = x => x.GetHashCode();
            return Union(leftList, rightList, identity);
        }

        public static List<object> LeftSetDiff(List<object> leftList,
            List<object> rightList)
        {
            Func<object, int> identity = x => x.GetHashCode();
            return LeftSetDiff(leftList, rightList, identity);
        }

        public static List<object> RightSetDiff(List<object> leftList,
            List<object> rightList)
        {
            Func<object, int> identity = x => x.GetHashCode();
            return RightSetDiff(leftList, rightList, identity);
        }

        public static List<object> SymetricDiff(List<object> leftList, List<object> rightList)
        {
            Func<object, int> identity = x => x.GetHashCode();
            return SymetricDiff(leftList, rightList, identity);
        }

        #endregion

        #region calling assembly specify identity resolution
        public static List<T> Intersect<T>(List<T> leftList,
            List<T> rightList, Func<T, int> identity)
        {
            if (leftList == null || rightList == null || identity == null)
                return new List<T>();

            var setOperation = leftList.Select(identity).Intersect(rightList.Select(identity));
            return ConsolidateFromHashCode(setOperation, leftList, rightList, identity);
        }

        public static List<T> Union<T>(List<T> leftList,
            List<T> rightList, Func<T, int> identity)
        {
            if (identity == null)
                return new List<T>();
            if (leftList == null && rightList == null)
                return new List<T>();
            if (rightList == null)
                return leftList;
            if (leftList == null)
                return rightList;

            var setOperation = leftList.Select(identity).Union(rightList.Select(identity));
            return ConsolidateFromHashCode(setOperation, leftList, rightList, identity);
        }

        public static List<T> LeftSetDiff<T>(List<T> leftList,
            List<T> rightList, Func<T, int> identity)
        {
            if (identity == null)
                return new List<T>();
            if (leftList == null && rightList == null)
                return new List<T>();
            if (rightList == null)
                return leftList;
            if (leftList == null)
                return new List<T>();

            var setOperation = leftList.Select(identity).Except(rightList.Select(identity));
            return ConsolidateFromHashCode(setOperation, leftList, rightList, identity);
        }

        public static List<T> RightSetDiff<T>(List<T> leftList,
            List<T> rightList, Func<T, int> identity)
        {
            if (identity == null)
                return new List<T>();
            if (leftList == null && rightList == null)
                return new List<T>();
            if (rightList == null)
                return new List<T>();
            if (leftList == null || leftList.Count <= 0)
                return rightList;

            var setOperation = rightList.Select(identity).Except(leftList.Select(identity));
            return ConsolidateFromHashCode(setOperation, leftList, rightList, identity);
        }

        public static List<T> SymetricDiff<T>(List<T> leftList, List<T> rightList, Func<T, int> identity)
        {
            if (identity == null)
                return new List<T>();
            if (leftList == null && rightList == null)
                return new List<T>();
            if (rightList == null)
                return leftList;
            if (leftList == null)
                return rightList;

            var setOperation =
                leftList.Select(identity)
                    .Except(rightList.Select(identity))
                    .Union(rightList.Select(identity).Except(leftList.Select(identity)));
            return ConsolidateFromHashCode(setOperation, leftList, rightList, identity);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static List<T> ConsolidateFromHashCode<T>(IEnumerable<int> setOperation,
            List<T> leftList, List<T> rightList, Func<T, int> identity )
        {
            if (setOperation == null)
                return new List<T>();
            if (leftList == null && rightList == null)
                return new List<T>();
            if (rightList == null)
                return leftList;
            if (leftList == null)
                return rightList;

            var listOut = new List<T>();
            foreach (var hc in setOperation)
            {
                var l = leftList.FirstOrDefault(x => identity(x) == hc);
                if(Equals(l, null))
                    l = rightList.First(x => identity(x) == hc);

                listOut.Add(l);
            }
            return listOut;
        }
        #endregion
    }	
}