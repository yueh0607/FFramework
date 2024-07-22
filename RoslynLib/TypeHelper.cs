using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynLib
{
    public static class TypeHelper
    {

        /// <summary>
        /// 类型是否实现了接口
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <param name="interfaceName">接口的名称，不需要完全限定</param>
        /// <returns></returns>
        public static bool ImplementsInterface(ClassDeclarationSyntax classDeclaration, string interfaceName)
        {
            var baseList = classDeclaration?.BaseList;

            if (baseList != null)
            {
                foreach (var baseType in baseList.Types)
                {
                    var typeName = baseType.Type.ToString();
                    if (typeName == interfaceName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsNestedClass(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        {
            // Check if the class declaration is inside another class or struct
            var parent = classDeclaration.Parent;

            // If the parent is not null and is a class or struct, the class is nested
            if (parent is ClassDeclarationSyntax || parent is StructDeclarationSyntax)
            {
                return true;
            }

            // Otherwise, the class is a top-level class
            return false;
        }
    }
}
