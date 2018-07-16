using System;
using System.Threading;

namespace NoFuture.Util.Core.Math.Matrix
{
    public class CofactorSupervisor
    {
        private readonly double[,] _cofactor;
        private readonly double[,] _input;
        private readonly object _msgLock = new object();
        private long _i;
        private long _j;
        private readonly long _rows;
        private readonly long _columns;
        private readonly long _maxWait;
        private long _returnCounter = 0L;

        public CofactorSupervisor(double[,] a)
        {
            _i = 0L;
            _j = 0L;
            _input = a;
            _rows = _input.CountOfRows();
            _columns = _input.CountOfColumns();
            _cofactor = new double[_input.CountOfRows(),_input.CountOfColumns()];

            _maxWait = 3300L * _rows;
        }

        public int WaitInterval { get; set; } = 100;

        public double[,] CalcCofactor()
        {
            var waiting = 0L;
            var processors = Convert.ToInt32(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS") ?? "4");
            for (var i = 0; i < processors; i++)
            {
                StartSingleWorker();
            }

            while (!IsComplete)
            {
                Thread.Sleep(WaitInterval);
                waiting += WaitInterval;
                if(waiting > _maxWait)
                    throw new TimeoutException($"The {nameof(CofactorSupervisor)} is not responding.");
            }

            lock (_msgLock)
            {
                return _cofactor;
            }
        }

        protected internal void ReceiveWorkComplete(CofactorWorkResult result)
        {
            var i = result.RowIndex;
            var j = result.ColumnIndex;
            lock (_msgLock)
            {
                _cofactor[i,j] = result.Determinant;
                _returnCounter += 1;
            }

            StartSingleWorker();
        }

        protected internal void StartSingleWorker()
        {
            if (IsComplete)
                return;
            var next = NextIj();
            var i = next.Item1;
            var j = next.Item2;
            if (i >= _rows || j >= _columns)
                return;
            var ic = _input.SelectMinor(i, j);
            var worker = new CofactorWorker(ic, i, j);
            worker.Completed += ReceiveWorkComplete;
        }

        protected internal bool IsComplete
        {
            get
            {
                lock (_msgLock)
                {
                    return _returnCounter >= _columns * _rows;
                }
            }
        }

        protected internal Tuple<long, long> NextIj()
        {
            var t = new Tuple<long,long>(_i, _j);
            if (_j + 1 >= _columns)
            {
                _j = 0;
                _i += 1;
            }
            else
            {
                _j += 1;
            }
            
            return t;
        }
    }
}