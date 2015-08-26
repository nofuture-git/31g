using System;

namespace TestAnythingHere
{
    public class LogicalBooleans
    {

        public static bool[,] GetBooleanOrganizedLikeBits()
        {
            var returnArray = new bool[16,4];
            var len = returnArray.GetLength(0);
            for (var i = 0; i < 15;i++ )
            {
                returnArray[i, 0] = Convert.ToBoolean(i & 1);
                returnArray[i, 1] = Convert.ToBoolean(i & 2);
                returnArray[i, 2] = Convert.ToBoolean(i & 4);
                returnArray[i, 3] = Convert.ToBoolean(i & 8);
            }
            return returnArray;
        }

        public static bool[,] GetAllTestValueCombinations(int numberOfProperties)
        {
            var rArray = new bool[(int)(Math.Pow(2, numberOfProperties)),numberOfProperties];

            for(var i = 0; i < rArray.GetLength(0);i++)
            {
                for(var j = 0;j<numberOfProperties;j++)
                {
                    rArray[i, j] = Convert.ToBoolean(i & (int)(Math.Pow(2, j)));
                }
            }
            return rArray;
        }
    }
}
