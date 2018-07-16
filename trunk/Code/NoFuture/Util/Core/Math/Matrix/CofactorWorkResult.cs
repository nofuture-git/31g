using System;

namespace NoFuture.Util.Core.Math.Matrix
{
    public class CofactorWorkResult
    {
        public CofactorWorkResult(Guid workerId, double det, long rowIndex, long columnIndex)
        {
            Determinant = det;
            Id = workerId;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public double Determinant { get; }
        public Guid Id { get; }
        public long RowIndex { get; }
        public long ColumnIndex { get; }
    }
}