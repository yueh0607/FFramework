using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FFramework
{
    public enum CodeElementType
    {
        Class,
        Method,
        Variable,
        Interface,
        Struct
    }

    public static class NameValidUtils
    {
        // C# 保留字列表
        static readonly HashSet<string> CSharpKeywords = new HashSet<string> {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char",
            "checked", "class", "const", "continue", "decimal", "default", "delegate",
            "do", "double", "else", "enum", "event", "explicit", "extern", "false",
            "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit",
            "in", "int", "interface", "internal", "is", "lock", "long", "namespace",
            "new", "null", "object", "operator", "out", "override", "params", "private",
            "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
        };

        public static bool IsValid(CodeElementType type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            string pattern;
            switch (type)
            {
                case CodeElementType.Class:
                case CodeElementType.Struct:
                case CodeElementType.Variable:
                    // Class and Struct names
                    pattern = @"^(?!\d)[a-zA-Z_\u4e00-\u9fa5][a-zA-Z0-9_\u4e00-\u9fa5]*$";
                    break;
                case CodeElementType.Method:
                    // Method names
                    pattern = @"^[a-zA-Z_][a-zA-Z0-9_]*$";
                    break;
                case CodeElementType.Interface:
                    // Interface names (must start with I)
                    pattern = @"^I[a-zA-Z_\u4e00-\u9fa5][a-zA-Z0-9_\u4e00-\u9fa5]*$";
                    break;
                default:
                    throw new ArgumentException("Unsupported CodeElementType", nameof(type));
            }

            // Check if the name matches the pattern
            if (!Regex.IsMatch(name, pattern))
            {
                return false;
            }

            // Check if the name is a C# keyword
            return !CSharpKeywords.Contains(name);
        }
    }
}
