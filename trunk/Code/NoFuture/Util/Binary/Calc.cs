using System;

namespace NoFuture.Util.Binary
{
    public class Calc
    {
        /// <summary>
        /// Utility function to calculate the length of a jagged array needed to contain the given input length.
        /// </summary>
        /// <param name="arrayLength">The length of the array to split.</param>
        /// <param name="blockSize"></param>
        /// <returns>Length of a jagged array needed to contain the given input length.</returns>
        public static long NumberOfInclusiveBlocks(long arrayLength,long blockSize)
        {
            return NumberOfInclusiveBlocks(arrayLength, 0,blockSize);
        }
        //recursive call
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static long NumberOfInclusiveBlocks(long length, long count,long blockSize)
        {
            return count * blockSize < length ? NumberOfInclusiveBlocks(length, ++count,blockSize) : count;
        }

        /// <summary>
        /// Utility function to get the sum of length of the multi-dim array.
        /// </summary>
        /// <param name="tapperedArray"></param>
        /// <returns></returns>
        public static long TotalLength(byte[][] tapperedArray)
        {
            var tapperedLength = 0;
            for (var i = 0; i < tapperedArray.Length; i++)
            {
                tapperedLength += tapperedArray[i].Length;
            }
            return tapperedLength;
        }
        /// <summary>
        /// Calculates the length, in number of characters, that will be needed to convert the given byte array length to a base64 char array.
        /// </summary>
        /// <param name="byteArrayLength">The length of the byte array that will be passed to a Base64 Convertor.</param>
        /// <returns>The needed length of the base64 string.</returns>
        public static long Base64Length(long byteArrayLength)
        {
            return (byteArrayLength + 2 - ((byteArrayLength + 2) % 3)) / 3 * 4;
        }
        /// <summary>
        /// Calculates the length, in the number of bytes, that will be needed to decode the given base64 length back to a byte array.
        /// </summary>
        /// <param name="base64StringLength">The length, in the number of chars, of the given base64 string.</param>
        /// <returns>Upon receiving the value '0' the value is incalculable and the string is probably not base64.</returns>
        public static long ByteArrayLength(long base64StringLength)
        {
            return ByteArrayLength(base64StringLength, base64StringLength);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static long ByteArrayLength(long dim, long returnupon)
        {
            if (dim == 0) return 0;
            if (Base64Length(dim) == returnupon) return --dim;
            return ByteArrayLength(--dim, returnupon);
        }


    }
}
