using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RoslynLib
{
    internal class AttributeHelper
    {
        public static bool TryGetAttributeValue(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, string attributeName, out string attributeValue)
        {
            attributeValue = null;

            // 获取所有的特性
            var attributes = semanticModel.GetDeclaredSymbol(classDeclaration)?.GetAttributes();

            // 遍历特性，查找目标特性
            foreach (var attribute in attributes)
            {
                // 获取特性的完整名称
                var attributeFullName = attribute.AttributeClass?.ToDisplayString();

                // 如果特性的完整名称与目标特性名称匹配
                if (attributeFullName == attributeName)
                {
                    // 获取特性的参数值
                    var parameterValue = attribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    attributeValue = parameterValue;
                    return true;
                }
            }

            // 如果未找到目标特性，则返回 false
            return false;
        }


    }
}
