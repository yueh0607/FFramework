using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoslynLib
{
    public static class WhereHelper
    {

        public static string InterfaceWhere(InterfaceDeclarationSyntax syntax, SyntaxTree tree, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(tree);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(syntax) as INamedTypeSymbol;
            return InterfaceWhere(interfaceSymbol);
        }

        public static string InterfaceWhere(INamedTypeSymbol interfaceSymbol)
        {
            // 检查接口是否是泛型的
            if (!interfaceSymbol.IsGenericType)
            {
                return string.Empty;
            }

            // 获取泛型参数的约束
            var constraints = interfaceSymbol.TypeParameters
                .Select(tp =>
                {
                    var constraintParts = new List<string>();

                    // 添加其他约束
                    var typeConstraints = tp.ConstraintTypes.Select(ct => ct.ToString());
                    constraintParts.AddRange(typeConstraints);

                    // 添加引用类型约束
                    if (tp.HasReferenceTypeConstraint)
                    {
                        constraintParts.Add("class");
                    }

                    // 添加值类型约束
                    if (tp.HasValueTypeConstraint)
                    {
                        constraintParts.Add("struct");
                    }

                    // 添加构造函数约束
                    if (tp.HasConstructorConstraint)
                    {
                        constraintParts.Add("new()");
                    }
                    if (constraintParts.Count == 0) return null;
                    return $"{tp.Name} : {string.Join(", ", constraintParts)}";
                });

            var list = constraints.ToList();
            list.RemoveAll(c => c == null);

            //constraints = constraints.Where(c => c != null);

            // 构建 where 子句
            var whereClauses = list.Select(constraint => $"where {constraint}");

            return string.Join(" ", whereClauses);
        }


        public static string MethodWhere(MethodDeclarationSyntax syntax, SyntaxTree tree, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(tree);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(syntax) as IMethodSymbol;
            return MethodWhere(interfaceSymbol);
        }

        public static string MethodWhere(IMethodSymbol methodSymbol)
        {
            // 检查方法是否是泛型的
            if (!methodSymbol.IsGenericMethod)
            {
                // 如果不是泛型方法，返回 empty
                return string.Empty;
            }

            // 获取泛型参数的约束
            var constraints = methodSymbol.TypeParameters
                .Select(tp =>
                {
                    var constraintParts = new List<string>();

                    // 添加其他约束
                    var typeConstraints = tp.ConstraintTypes.Select(ct => ct.ToString());
                    constraintParts.AddRange(typeConstraints);

                    //class
                    if (tp.HasReferenceTypeConstraint)
                    {
                        constraintParts.Add("class");
                    }

                    //struct
                    if (tp.HasValueTypeConstraint)
                    {
                        constraintParts.Add("struct");
                    }

                    // 添加构造函数约束
                    if (tp.HasConstructorConstraint)
                    {
                        constraintParts.Add("new()");
                    }

                    if (constraintParts.Count == 0) return null;
                    return $"{tp.Name} : {string.Join(", ", constraintParts)}";
                });
            constraints = constraints.Where(c => c != null);

            // 构建 where 子句
            var whereClauses = constraints.Select(constraint => $"where {constraint}");

            return string.Join(" ", whereClauses);
        }
    }
}
