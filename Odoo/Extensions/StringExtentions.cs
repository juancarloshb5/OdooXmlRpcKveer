using System;
using System.Collections.Generic;
using System.Text;

namespace Odoo.Extensions
{
    public static class StringExtentions
    {
        public static string[] ToLowerAndSplitWithUnderscore(this IEnumerable<string> array)
        {
            var resultStringArray = new List<string>();
            foreach (var str in array)
            {
                var newStr = str.ToLowerAndSplitWithUnderscore();
                resultStringArray.Add(newStr);

            }
            return resultStringArray.ToArray();
        }

        public static string ToLowerAndSplitWithUnderscore(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (Char.IsUpper(c) && str.IndexOf(c) != 0)
                {
                    sb.Append('_');
                }
                sb.Append(Char.ToLower(c));

            }

            return sb.ToString();
        }
    }
}
