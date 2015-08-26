using System;
using System.Collections;

namespace NoFuture.Console
{
    public class IncrementPerfCounters
    {
        #region Properties

        public static bool Quit = false;
        public CallCounts Counts { get; set; }
        public System.Collections.Generic.List<MyMarshalled> MarshalledObjects { get; set; }
        public System.Collections.Generic.List<MyPoco> PocoObjects { get; set; }
        public System.Collections.Generic.List<System.Threading.Thread> Threads { get; set; }
        public System.Collections.Generic.List<MyLock> Locks { get; set; }
        public System.Collections.Generic.List<byte[]> LargeObjects { get; set; }
        public System.Collections.Generic.List<MyPinned> PinnedObjects { get; set; }

        #endregion

        public static void Main(string[] args)
        {
            try
            {

                var myProgram = new IncrementPerfCounters();
                myProgram.MainLoop();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.StackTrace);
                System.Console.ReadKey();
            }
        }

        #region Constructors

        public IncrementPerfCounters()
        {
            Counts = new CallCounts();
            MarshalledObjects = new System.Collections.Generic.List<MyMarshalled>();
            PocoObjects = new System.Collections.Generic.List<MyPoco>();
            Threads = new System.Collections.Generic.List<System.Threading.Thread>();
            Locks = new System.Collections.Generic.List<MyLock>();
            LargeObjects = new System.Collections.Generic.List<byte[]>();
            PinnedObjects = new System.Collections.Generic.List<MyPinned>();
        }

        #endregion

        public void MainLoop()
        {

            System.Console.WriteLine("enter 'quit' to exit...");
            for (; ; )
            {
                var input = String.Format("{0}", System.Console.ReadLine());

                if (input.ToLower().Trim() == "quit")
                {
                    Quit = true;
                    if (MyLock.ItsAlreadyLocked)
                    {
                        System.Console.WriteLine("removing object locks... please wait.");
                    }

                    foreach (var pin in PinnedObjects)
                    {
                        pin.Dispose();
                    }
                    for (var i = 0; i < LargeObjects.Count; i++)
                    {
                        LargeObjects[i] = null;
                    }
                    foreach (var marshalledObject in MarshalledObjects)
                    {
                        marshalledObject.Dispose();
                    }
                    foreach (var myLock in Locks)
                    {
                        myLock.Dispose();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return;
                }

                if (input.ToLower().Trim() == CallCounts.PropertyNames.Exception.ToLower())
                {
                    HandleCmdException();
                    Counts.Exception += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Lock.ToLower())
                {
                    HandleCmdLock();
                    Counts.Lock += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Thread.ToLower())
                {
                    HandleCmdThread();
                    Counts.Thread += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Pinned.ToLower())
                {
                    HandleCmdPinned();
                    Counts.Pinned += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Marshal.ToLower())
                {
                    HandleCmdMarshal();
                    Counts.Marshal += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Loh.ToLower())
                {
                    HandleCmdLoh();
                    Counts.Loh += 1;
                }
                else if (input.ToLower().Trim() == CallCounts.PropertyNames.Poco.ToLower())
                {
                    HandleCmdPoco();
                    Counts.Poco += 1;
                }
                else if (input.ToLower().Trim() == "print")
                {
                    System.Console.WriteLine(Counts.Print());
                }
                else if (input.ToLower().Trim() == "help")
                {
                    System.Console.WriteLine("Enter a valid command to cause the respective .NET performance");
                    System.Console.WriteLine("counter to be incremented.");
                    System.Console.WriteLine("Enter 'print' to see the running totals of each command.");
                    System.Console.WriteLine("Commands are not case-sensitive.");
                    System.Console.WriteLine("Valid commands are:");
                    System.Console.WriteLine("-------------------");
                    var cmds = Counts.GetType().GetProperties();
                    foreach (var cmd in cmds)
                    {
                        System.Console.WriteLine("\t{0}", cmd.Name);
                    }
                    System.Console.WriteLine("\t{0}", "Print");
                    System.Console.WriteLine("\t{0}", "Quit");
                }
                else
                {
                    //echo
                    System.Console.WriteLine(input);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #region Command Handlers

        internal void HandleCmdException()
        {
            try
            {
                throw new Exception("new exception thrown");
            }
            catch (Exception)
            {

            }
        }
        internal void HandleCmdLock()
        {
            Locks.Add(MyLock.CreateALockProblem());
        }
        internal void HandleCmdThread()
        {
            var newThread = new System.Threading.Thread(MyThreadFunction);
            newThread.Start();
            Threads.Add(newThread);
        }
        internal void HandleCmdPinned()
        {
            PinnedObjects.Add(new MyPinned());
        }
        internal void HandleCmdMarshal()
        {
            MarshalledObjects.Add(new MyMarshalled());
        }
        internal void HandleCmdLoh()
        {
            var lo = new byte[CallCounts.LARGE_OBJECT_BYTE_ARRAY_SIZE];//1 mb
            for (var i = 0; i < lo.Length; i++)
            {
                lo[i] = (byte)(i % 256);
            }
            LargeObjects.Add(lo);
        }
        internal void HandleCmdPoco()
        {
            var myPoco = new MyPoco
            {
                MyInt = Counts.Poco,
                MyDate = DateTime.Now,
                MyString = Util.Etc.LoremIpsumEightParagraphs,
                MyProperty = new MyChildPoco { MyChildString = String.Empty }
            };
            PocoObjects.Add(myPoco);
        }

        #endregion

        private void MyThreadFunction()//this is a deadend
        {
            for (; ; )//ever
            {
                if (Quit)
                {
                    return;
                }
                System.Threading.Thread.Sleep(CallCounts.THREAD_SLEEP_MILLI_SECONDS);
            }
        }

    }
}
