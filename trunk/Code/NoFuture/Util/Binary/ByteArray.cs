using System;
using System.Globalization;

namespace NoFuture.Util.Binary
{
    public static class BitOrderMask
    {
        public const ulong QWORD_MSB = (ulong)0xFF00000000000000;
        public const ulong QWORD_7TH = (ulong)0xFF000000000000;
        public const ulong QWORD_6TH = (ulong)0xFF0000000000;
        public const ulong QWORD_5TH = (ulong)0xFF00000000;
        public const uint DWORD_MSB = (uint)0xFF000000;
        public const uint DWORD_3RD = (uint)0xFF0000;
        public const ushort WORD_MSB = (ushort)0xFF00;
        public const byte LSB = (byte)0xFF;

        public const ulong DWORD_MASK = (ulong)0x00000000FFFFFFFF;
        public const uint WORD_MASK = (uint) 0x0000FFFF;
    }
    public class BityOrderMarker
    {
        public static byte[] UTF8 = new byte[] { 0xef, 0xbb, 0xbf };
        public static byte[] UTF16_BE = new byte[] { 0xfe, 0xff };
        public static byte[] UTF16_LE = new byte[] { 0xff, 0xfe };
        public static byte[] UTF32_BE = new byte[] { 0x0, 0x0, 0xfe, 0xff };
        public static byte[] UTF32_LE = new byte[] { 0xff, 0xfe, 0x0, 0x0 };
    }
    public static class ByteArray
    {
        private static byte[][] _base64_chars = { new byte[] { 0x30, 0x39 }, new byte[] { 0x41, 0x5A }, new byte[] { 0x61, 0x7A }, new byte[] { 0x2F, 0x2B, 0x3D } };

        /// <summary>
        /// Binary utility method to turn a long (64 bit) into a
        /// big-endian byte array (index 0 is LSB)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>An eight byte array.</returns>
        public static byte[] FromQWord(ulong value)
        {
            byte[] qWord = new byte[8];
            qWord[0] = (byte)((value & BitOrderMask.QWORD_MSB) >> 56);
            qWord[1] = (byte)((value & BitOrderMask.QWORD_7TH) >> 48);
            qWord[2] = (byte)((value & BitOrderMask.QWORD_6TH) >> 40);
            qWord[3] = (byte)((value & BitOrderMask.QWORD_5TH) >> 32);

            byte[] dWord = FromDWord((uint)(value & BitOrderMask.DWORD_MASK));

            Array.Copy(dWord,0,qWord,4,4);

            return qWord;
        }

        /// <summary>
        /// Binary utility method to turn an int (32 bit) into 
        /// big-endian byte array (index 0 is LSB).
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A four byte array.</returns>
        public static byte[] FromDWord(uint value)
        {
            var dWord = new byte[4];

            dWord[0] = (byte)((value & BitOrderMask.DWORD_MSB) >> 24);
            dWord[1] = (byte)((value & BitOrderMask.DWORD_3RD) >> 16);

            var word = FromWord((ushort)(value & BitOrderMask.WORD_MASK));

            Array.Copy(word,0,dWord,2,2);

            return dWord;
        }

        /// <summary>
        /// Binary utility to turn a bytes into a 32 bit int.
        /// </summary>
        /// <param name="pointer">Most significant byte</param>
        /// <param name="middle"></param>
        /// <param name="ring"></param>
        /// <param name="pinkie">Least significant byte.</param>
        /// <returns></returns>
        public static int ToDWord(byte pointer, byte middle, byte ring, byte pinkie)
        {
            var dWord = (pointer << 24);
            dWord = (middle << 16) | dWord;
            dWord = (ring << 8) | dWord;
            dWord = pinkie | dWord;
            return dWord;
        }

        /// <summary>
        /// Binary utility to turn bytes into a 64 bit long
        /// </summary>
        /// <param name="msb"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <param name="lsb"></param>
        /// <returns></returns>
        public static long ToQWord(byte msb, byte b, byte c, byte d, byte e, byte f, byte g, byte lsb)
        {
            var qword = msb << 56;
            qword = (b << 48) | qword;
            qword = (c << 40) | qword;
            qword = (d << 32) | qword;
            qword = (e << 24) | qword;
            qword = (f << 16) | qword;
            qword = (g << 8) | qword;
            qword = lsb | qword;

            return qword;
        }

        /// <summary>
        /// Binary utility to turn bytes into 16 bit short
        /// </summary>
        /// <param name="msb"></param>
        /// <param name="lsb"></param>
        /// <returns></returns>
        public static short ToWord(byte msb, byte lsb)
        {
            var word = msb << 8;
            return Convert.ToInt16(word | lsb);
        }

        /// <summary>
        /// Binary utility method to turn a short (16 bit) into
        /// a big-endian byte array (index 0 is LSB)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A two byte array.</returns>
        public static byte[] FromWord(ushort value)
        {
            byte[] word = new byte[2];

            word[0] = (byte)((value & BitOrderMask.WORD_MSB) >> 8);
            word[1] = (byte)((value & BitOrderMask.LSB));

            return word;
        }

        /// <summary>
        /// Given some string in the form of serial characters of two digit hex values, 
        /// a byte array is returned accordingly.
        /// </summary>
        /// <param name="hexString">
        /// The string to be converted to a byte array.
        /// Any inclusion of character sequence '0x' will be removed
        /// and any string which does not evenly divide will be padded 
        /// with an additional '0' char to the front of the string.
        /// </param>
        /// <returns>A byte array according to each two-digit sequence which is parsed 
        /// to a hex value. </returns>
        public static byte[] ToHexByteArray(string hexString)
        {
            hexString = hexString.Replace("0x", "");
            //pad a zero to odd-number length strings
            if(hexString.Length % 2 == 1)
            {
                hexString = String.Format("0{0}", hexString);
            }

            var data = new System.Collections.Generic.List<byte>();
            for(var i = 0; i < hexString.Length / 2; i++)
            {
                var p = hexString.Substring(i * 2, 2);
                byte q = 0;
                if(Byte.TryParse(p,NumberStyles.HexNumber,null as IFormatProvider ,out q))
                {
                    data.Add(q);
                }
            }
            return data.ToArray();
        }

        /// <summary>
        /// Debugging utility to get hex values of a byte array.  Prints to Console.
        /// </summary>
        /// <param name="value"></param>
        public static string PrintByteArray(byte[] value)
        {
            var print = new System.Text.StringBuilder();
            for (var i = 0; i < value.Length; i++)
                print.Append(value[i].ToString("X2"));

            return print.ToString();
        }
        /// <summary>
        /// Debugging utility to get hex values of a byte array in the form of '(byte)0x00,' - prints to Console.
        /// </summary>
        /// <param name="value"></param>
        public static string PrettyPrintByteArray(byte[] value)
        {
            var pretty = new System.Text.StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                pretty.Append("(byte)0x" + value[i].ToString("X2") + ",");
                if (i > 0 && i % 4 == 0) 
                    pretty.AppendLine();
            }

            return pretty.ToString();

        }
        /// <summary>
        /// Adds a zero-byte (0x00) to the front of a byte array.
        /// </summary>
        /// <param name="inputarray"></param>
        /// <returns>The same array as the input but with a zero-byte in front.</returns>
        public static byte[] PadWithZeroInFront(byte[] inputarray)
        {
            byte[] toadd = inputarray;
            byte[] zeropad = new byte[toadd.Length + 1];
            zeropad[0] = (byte)0;
            Array.Copy(toadd, 0, zeropad, 1, toadd.Length);
            return zeropad;
        }
        /// <summary>
        /// Adds a zero-byte (0x00) to the front of each byte array in the multi-dimensional array.
        /// </summary>
        /// <param name="inputarray">
        /// No return value - pass-by-value-semantics ensures operation on underlying object.
        /// </param>
        public static void PadAllContainedWithZeroInFront(byte[][] inputarray)
        {
            for (int i = 0; i < inputarray.Length; i++)
            {
                inputarray[i] = PadWithZeroInFront(inputarray[i]);
            }
        }
        /// <summary>
        /// Utility function to instantiate a jagged array needed to contain the given parameters
        /// </summary>
        /// <param name="numberofblocks">The length of the jagged array object.</param>
        /// <param name="blocksize">The length of each contained array, save the last.</param>
        /// <param name="remainder">The length of the last array.</param>
        /// <returns></returns>
        public static byte[][] InitializeTapperedArray(long numberofblocks, long blocksize, long remainder)
        {
            byte[][] toreturn = new byte[numberofblocks][];
            for (var i = 0; i < numberofblocks - 1; i++)
            {
                toreturn[i] = new byte[blocksize];
            }
            toreturn[numberofblocks - 1] = new byte[remainder];
            return toreturn;
        }
        /// <summary>
        /// Utility function to break a byte array into its jagged-array equivilent.  
        /// Block size is set using the 'BlockSize' property, defaults to 256.
        /// </summary>
        /// <param name="sourceByteArray">The byte array to be broken apart.</param>
        /// <param name="blocksize"></param>
        /// <returns>
        /// Fully populated jagged or square (should it fit without a remainder) byte 
        /// array containing the bytes of the input array.
        /// </returns>
        public static byte[][] BreakByteArray(byte[] sourceByteArray, long blocksize)
        {
            if (blocksize == 0)
                blocksize = Shared.Constants.DEFAULT_BLOCK_SIZE;
            byte[][] returnArray;
            var numberofblocks = Calc.NumberOfInclusiveBlocks(sourceByteArray.Length, blocksize);
            //get the crap at the tail of the array first so the loop can skip it
            var remainder = sourceByteArray.Length % blocksize;
            //System.out.println("the calculated remainder is " + remainder);
            if (remainder > 0)
            {
                returnArray = InitializeTapperedArray(numberofblocks, blocksize, remainder);
                returnArray[numberofblocks - 1] = GetArrayTail(sourceByteArray, blocksize);

                //get one less than the total number of blocks
                for (var i = 0; i < numberofblocks - 1; i++)
                {
                    Array.Copy(sourceByteArray, (i * blocksize), returnArray[i], 0, blocksize);
                }
            }
            else//it fits as a square array
            {
                returnArray = new byte[numberofblocks][];
                for (var i = 0; i < numberofblocks; i++)
                {
                    returnArray[i] = new byte[blocksize];
                    Array.Copy(sourceByteArray, (i * blocksize), returnArray[i], 0, blocksize);
                }
            }
            return returnArray;
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static byte[] GetArrayTail(byte[] sourceByteArray, long numberofblocks)
        {
            var remainder = sourceByteArray.Length % numberofblocks;
            if (remainder == 0) return new byte[0];
            var returnArray = new byte[remainder];
            for (var i = 0; i < remainder; i++)
            {
                var index = sourceByteArray.Length - remainder + i;
                returnArray[i] = sourceByteArray[index];
            }
            return returnArray;
        }
        /// <summary>
        /// Utility function to put a jagged or square array back into a single 
        /// byte array object.
        /// </summary>
        /// <param name="tapperedArray">The array to be put back together</param>
        /// <param name="blocksize"></param>
        /// <returns>
        /// Fully populated byte array containing all the bytes of the input 
        /// array as a single byte array object.
        /// </returns>
        public static byte[] PutArrayBackTogether(byte[][] tapperedArray, long blocksize)
        {
            var concatArray = new byte[Calc.TotalLength(tapperedArray)];
            for (var i = 0; i < tapperedArray.Length; i++)
            {
                for (var j = 0; j < tapperedArray[i].Length; j++)
                {
                    concatArray[(i * blocksize) + j] = tapperedArray[i][j];
                }
            }
            return concatArray;
        }

        /// <summary>
        /// Intended to move content off the end of <see cref="source"/> array and add it 
        /// to the front of <see cref="receiving"/> array up to the first occurance of 
        /// <see cref="marker"/>.
        /// </summary>
        /// <param name="receiving"></param>
        /// <param name="source"></param>
        /// <param name="marker"></param>
        public static byte[] ShiftArrayContent(byte[] receiving, ref byte[] source, string marker)
        {
            //validate the inputs
            if (string.IsNullOrWhiteSpace(marker) || 
                 receiving == null || 
                 receiving.Length <= 0 || 
                 source == null || 
                 source.Length <=0)
                return receiving;

            //perform the first test 
            var markerBuffer = marker.ToCharArray();
            var endOfSource = new byte[markerBuffer.Length];
            Array.Copy(source, (source.Length - endOfSource.Length), endOfSource, 0, endOfSource.Length);
            Func<byte[], string, bool> markerEq = (b, s) => System.Text.Encoding.UTF8.GetString(b) == s;

            //loop while test continues to fail
            while (!markerEq(endOfSource, marker))
            {
                //take the tail just scoped from the source and put it at the front of an new receiving
                var newReceiving = new byte[receiving.Length + endOfSource.Length];
                Array.Copy(endOfSource, 0, newReceiving, 0, endOfSource.Length);

                //now append the rest of receiving to the end of the new receiving
                Array.Copy(receiving, 0, newReceiving, endOfSource.Length, receiving.Length);

                //now cut the end of the source off and push the remainder to a new source
                var newSource = new byte[source.Length - markerBuffer.Length];
                Array.Copy(source, 0, newSource, 0, newSource.Length);

                //set the passed in source to the new source
                source = newSource;

                //do likewise for receiving
                receiving = newReceiving;

                //test if we have trimmined everything off the source
                if (source.Length < endOfSource.Length)
                    break;

                //get the next tail of source - loop condition performs the test
                Array.Copy(source, (source.Length - endOfSource.Length), endOfSource, 0, endOfSource.Length);
            }

            //condition must have passed to reach this line
            return receiving;
        }
        /// <summary>
        /// Utility function that replaces 'Url-safe' base64 replacements with 
        /// the original base64 values.
        /// </summary>
        /// <param name="valuearray">
        /// A byte array of an encoded url-safe base64 string.  
        /// The input should be a byte array representation of the base64 characters 
        /// not a decoded byte array thereof.
        /// </param>
        /// <returns>go through the byte array looking to replace '_' with '/' and '-' with '+'</returns>
        public static byte[] ReplaceBase64UrlSafeCharsWithOriginals(byte[] valuearray)
        {
            //go through the byte array looking to replace '_' with '/' and '-' with '+'
            for (var i = 0; i < valuearray.Length; i++)
            {
                switch (valuearray[i])
                {
                    case 0x5F:
                        valuearray[i] = 0x2F;
                        break;
                    case 0x2D:
                        valuearray[i] = 0x2B;
                        break;
                    default:
                        continue;
                }
            }
            return valuearray;
        }
        /// <summary>
        /// Utility function which validates that each byte-value of an 
        /// encoded base64 byte array is within the limits of base64 characters.
        /// </summary>
        /// <param name="inputvalue">
        /// A byte array of an encoded url-safe base64 string.  
        /// The input should be a byte array representation of the base64 
        /// characters not a decoded byte array thereof.
        /// </param>
        /// <returns>Truth-value of each character being within the base64 charcters</returns>
        public static bool ValidateBase64Chars(byte[] inputvalue)
        {

            for (var i = 0; i < inputvalue.Length; i++)
            {
                if (inputvalue[i] == 0x00) continue;//utf-16 and utf-32 will have alot of these
                if (inputvalue[i] >= _base64_chars[0][0] && inputvalue[i] <= _base64_chars[0][1]) continue;
                if (inputvalue[i] >= _base64_chars[1][0] && inputvalue[i] <= _base64_chars[1][1]) continue;
                if (inputvalue[i] >= _base64_chars[2][0] && inputvalue[i] <= _base64_chars[2][1]) continue;
                if (inputvalue[i] == _base64_chars[3][0] || inputvalue[i] == _base64_chars[3][1] 
                    || (i == inputvalue.Length - 1 && inputvalue[i] == _base64_chars[3][2]))
                    continue;
                //this line of code is only reachable if the character is not Base64
                return false;
            }
            return true;//every character is valid will return here
        }
        /// <summary>
        /// Utility function that checks the input byte array for the 
        /// Unicode byte order characters ([0xFF 0xFE] or [0xFE0xFF]).
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns>
        /// Returns true if either the Big-Endian or Little-Endian 
        /// byte-order marker is present at the front of the array.
        /// </returns>
        public static bool CheckForBitOrderMarker(byte[] inputvalue)
        {
            //check that the length to avoid out-of-bounds exception
            if (inputvalue.Length < 2) return false;
            //check if the byte array begins with the big-endian or little-endian unicode marker
            if ((inputvalue[0] == 0xFE && inputvalue[1] == 0xFF) 
                 || (inputvalue[0] == 0xFF && inputvalue[1] == 0xFE))
                return true;
            //this line it reached upon first two bytes not being endian marker
            return false;
        }
    }
}
