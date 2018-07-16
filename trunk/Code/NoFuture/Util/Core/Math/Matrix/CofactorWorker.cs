using System;

namespace NoFuture.Util.Core.Math.Matrix
{
    public delegate void StartWork();

    public delegate void EndWork(CofactorWorkResult result);

    public class CofactorWorker
    {
        public event EndWork Completed;
        private readonly StartWork _workLoad;
        private readonly double[,] _myMinor;

        public long RowIndex { get; }
        public long ColumnIndex { get; }
        public Guid Id { get; }
        public double Determinant { get; private set; }

        public CofactorWorker(double[,] ic, long i, long j)
        {
            _myMinor = ic;
            RowIndex = i;
            ColumnIndex = j;
            Id = Guid.NewGuid();
            _workLoad = DoMyWork;
            _workLoad.BeginInvoke(CallBack, Id);
        }
        public void DoMyWork()
        {
            Determinant = _myMinor.Determinant();
            if ((RowIndex + ColumnIndex) % 2 == 1)
                Determinant *= -1;
        }

        private void CallBack(IAsyncResult z)
        {
            _workLoad.EndInvoke(z);
            Completed?.Invoke(new CofactorWorkResult(Id, Determinant, RowIndex, ColumnIndex));
        }
    }
}