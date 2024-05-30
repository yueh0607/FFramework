using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRuleAnalyzer
{

    public class ConstraintDefinition
    {
        public static List<string> AnalyzerExcludePath = new List<string>() {
            "/PackageCache/",
            "/ThirdParty/",
            "/Plugins/"
        };

        public static bool ExcludeAnalize(string path)
        {
            foreach (var file in AnalyzerExcludePath)
            {
                if (path.Contains(file))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
