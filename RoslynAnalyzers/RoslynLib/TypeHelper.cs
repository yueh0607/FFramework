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
    }
}
