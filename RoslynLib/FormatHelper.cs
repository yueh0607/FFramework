using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RoslynLib
{
    public static class FormatHelper
    {
        public static string RemoveLeadingWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // 使用正则表达式去除每行开头的空格和TAB
            string pattern = @"^[ \t]+";
            string replacement = string.Empty;
            Regex regex = new Regex(pattern, RegexOptions.Multiline);

            return regex.Replace(input, replacement);
        }
    }
}
