using System;

namespace NoFuture.Hbm.Sid
{
    [Serializable]
    public class BinaryId
    {
        private readonly byte[] _data;
        public BinaryId(byte[] binarySid)
        {
            _data = binarySid;
        }

        public byte[] Data { get { return _data; } }

        public override bool Equals(object obj)
        {
            if ((obj as BinaryId) == null)
            {
                return false;
            }

            var compareTo = obj as BinaryId;

            if (compareTo.Data == null ^ this.Data == null)
            {
                return false;
            }

            if (this.Data != null)
            {
                if (this.Data.Length != compareTo.Data.Length)
                {
                    return false;
                }

                for (var i = 0; i < Data.Length; i++)
                {
                    if (compareTo.Data[i] != this.Data[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 1;
            if (Data != null)
            {
                hashCode += Data.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (Data == null)
            {
                return base.ToString();
            }
            var hexData = new System.Text.StringBuilder();
            for (var i = 0; i < Data.Length; i++)
            {
                hexData.Append(Data[i].ToString("X2"));
            }
            return hexData.ToString();
        }
    }
}
