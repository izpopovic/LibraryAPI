using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Utils
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this String text)
        {
            return text == null || text.Trim().Length == 0;
        }
    }
}
