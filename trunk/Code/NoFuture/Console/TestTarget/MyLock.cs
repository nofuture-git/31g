namespace NoFuture.Console
{
    public class MyLock : System.IDisposable
    {
        public static object LockObject = new object();
        public static bool ItsAlreadyLocked = false;
        public static bool BreakOut = false;
        public System.Threading.Thread GetsTheLock { get; set; }
        public System.Threading.Thread WaitsForever { get; set; }

        public static MyLock CreateALockProblem()
        {
            var myLock = new MyLock();
            myLock.SetLockInMotion();
            return myLock;
        }

        public void SetLockInMotion()
        {
            if (!ItsAlreadyLocked)
            {
                GetsTheLock = new System.Threading.Thread(new System.Threading.ThreadStart(BothWantInHere));
                GetsTheLock.Start();
            }
            WaitsForever = new System.Threading.Thread(new System.Threading.ThreadStart(BothWantInHere));
            WaitsForever.Start();
        }

        public static void BothWantInHere()
        {
            lock (LockObject)//this is global scoped so everyone else has to wait
            {
                ItsAlreadyLocked = true;
                for (; ; )//ever
                {
                    if (IncrementPerfCounters.Quit || BreakOut)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(CallCounts.THREAD_SLEEP_MILLI_SECONDS);
                }
            }
        }
        public void Dispose()
        {
            BreakOut = true;
            if (GetsTheLock != null)
            {
                GetsTheLock.Abort();
            }
            if (WaitsForever != null)
            {
                WaitsForever.Abort();
            }
        }
    }
}
