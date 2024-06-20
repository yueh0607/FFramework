using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRuleAnalyzer
{
    internal class PascalUtility
    {
        public static bool IsPascalCase(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
            {
                return false;
            }

            for (int i = 1; i < name.Length; i++)
            {
                if (!char.IsLetterOrDigit(name[i]) || char.IsUpper(name[i]) && !char.IsLetterOrDigit(name[i - 1]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
