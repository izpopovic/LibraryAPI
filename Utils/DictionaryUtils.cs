using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Utils
{
    public static class DictionaryUtils
    {
        public static Dictionary<char, int> FillDictionary()
        {
            var dictionary = new Dictionary<char, int>();

            char[] keys = { '<', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            int[] values = { 0, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var value = values[i];
                dictionary.Add(key, value);
            }
            return dictionary;
        }
    }
}
