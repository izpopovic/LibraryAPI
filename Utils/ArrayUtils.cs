using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Utils
{
    public static class ArrayUtils
    {
        static List<int> multiplierList = new List<int>() { 7, 3, 1 };

        public static int[] CreateMultipliedArray(int numberOfRepetitions)
        {
            var list = new List<int>();
            for (int i = 0; i < numberOfRepetitions; i++)
            {
                list.AddRange(multiplierList);
            }
            return list.ToArray();
        }
    }
}
