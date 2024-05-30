using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RoslynLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Generator]
public class ModuleClassGenerator : ISourceGenerator
{
    private const string InterfaceName = "IModule"; // 指定的接口名称
    private const string AttName = "FFramework.ModuleStaticAttribute";
    public void Initialize(GeneratorInitializationContext context)
    {

    }

    public void Execute(GeneratorExecutionContext context)
    {
        Compilation compilation = context.Compilation;

        // 遍历 Compilation 下的所有语法树
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var root = syntaxTree.GetRoot();

            // 在语法树中查找所有的 ClassDeclarationSyntax 节点
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            // 遍历所有类
            foreach (var classDeclaration in classDeclarations)
            {
                var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                if (IsModuleClass(classDeclaration, semanticModel, out string genName))
                {
                    var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
                    var accessibility = SymbolAccessibilityToString(classSymbol.DeclaredAccessibility);
                    List<ISymbol> members = new List<ISymbol>(20);
                    // 遍历类的成员
                    foreach (var member in classSymbol.GetMembers())
                    {
                        // 如果成员是字段、属性或方法，并且是公共或内部的，则输出其信息
                        if ((member is IFieldSymbol || member is IPropertySymbol || member is IMethodSymbol) &&
                            (member.DeclaredAccessibility == Accessibility.Public || member.DeclaredAccessibility == Accessibility.Internal))
                        {
                            members.Add(member);
                        }
                    }
                    RemoveConstructorsAndDestructors(members);
                    string code = GenerateCode(members, accessibility, genName, classDeclaration.Identifier.ValueText);
                    SourceText sourceText = SourceText.From(code, Encoding.UTF8);
                    context.AddSource(genName + ".cs", sourceText);
                }
            }
        }
    }




    private bool IsModuleClass(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, out string generateName)
    {
        var interfaceTypeName = InterfaceName; // 接口名称
        var attributeName = AttName; // 特性名称



        // 判断类是否实现了指定接口
        var implementsInterface = TypeHelper.ImplementsInterface(classDeclaration, interfaceTypeName);

        // 在语法树中查找特性
        var attribute = TryGetAttributeValue(classDeclaration, semanticModel, attributeName, out generateName);


        return implementsInterface && attribute;
    }

    private bool TryGetAttributeValue(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, string attributeName, out string attributeValue)
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


    private string GenerateCode(List<ISymbol> members, string accessibility, string genName, string moduleName)
    {
        string modelCode =
    $@"namespace FFramework
{{
    {accessibility} static partial class {genName}
    {{
#GENCODE#
    }}
}}
";

        StringBuilder codeBuilder = new StringBuilder();

        foreach (var symbol in members)
        {
            switch (symbol.Kind)
            {
                case SymbolKind.Field:
                    var fieldSymbol = symbol as IFieldSymbol;
                    codeBuilder.AppendLine($"        {SymbolAccessibilityToString(fieldSymbol.DeclaredAccessibility)} static {fieldSymbol.Type} {fieldSymbol.Name} => Envirment.Current.GetModule<{moduleName}>().{fieldSymbol.Name};");
                    break;
                case SymbolKind.Property:

                    var propertySymbol = symbol as IPropertySymbol;
                    var accessModifier = SymbolAccessibilityToString(propertySymbol.DeclaredAccessibility);

                    // 检查 get 访问器的修饰符是否为 public 或 internal
                    var getAccessor = propertySymbol.GetMethod != null &&
                                      (propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public ||
                                       propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Internal)
                        ? $"get => FFramework.Envirment.Current.GetModule<{moduleName}>().{propertySymbol.Name};"
                        : null;

                    // 检查 set 访问器的修饰符是否为 public 或 internal
                    var setAccessor = propertySymbol.SetMethod != null &&
                                      (propertySymbol.SetMethod.DeclaredAccessibility == Accessibility.Public ||
                                       propertySymbol.SetMethod.DeclaredAccessibility == Accessibility.Internal)
                        ? $"set => FFramework.Envirment.Current.GetModule<{moduleName}>().{propertySymbol.Name} = value;"
                        : null;

                    // 如果 get 或 set 访问器的修饰符符合条件，则生成属性代码
                    if (getAccessor != null || setAccessor != null)
                    {
                        codeBuilder.AppendLine($"        {accessModifier} static {propertySymbol.Type} {propertySymbol.Name} {{ {getAccessor} {setAccessor} }}");
                    }
                    break;
                case SymbolKind.Method:
                    var methodSymbol = symbol as IMethodSymbol;
                    var returnType = methodSymbol.ReturnType.ToString();
                    var methodName = methodSymbol.Name;

                    // 构建方法的泛型参数部分
                    var typeParameters = "";
                    if (methodSymbol.TypeParameters.Any())
                    {
                        typeParameters = "<";
                        foreach (var tp in methodSymbol.TypeParameters)
                        {
                            typeParameters += $"{tp.Name}, ";
                        }
                        typeParameters = typeParameters.TrimEnd(',', ' ') + ">";
                    }

                    // 构建方法的参数列表
                    var parameters = "";
                    foreach (var p in methodSymbol.Parameters)
                    {
                        parameters += $"{p.Type} {p.Name}, ";
                    }
                    parameters = parameters.TrimEnd(',', ' ');

                    // 构建方法的调用部分
                    var invocation = $"().{methodName}{typeParameters}(";
                    foreach (var p in methodSymbol.Parameters)
                    {
                        invocation += $"{p.Name}, ";
                    }
                    invocation = invocation.TrimEnd(',', ' ') + ")";

                    // 构建泛型约束部分
                    var constraints = WhereHelper.MethodWhere(methodSymbol);

                    // 生成代码
                    codeBuilder.AppendLine($"        {SymbolAccessibilityToString(methodSymbol.DeclaredAccessibility)} static {returnType} {methodName}{typeParameters}({parameters}){constraints} => Envirment.Current.GetModule<{moduleName}>{invocation};");
                    break;
            }
        }
        return modelCode.Replace("#GENCODE#", codeBuilder.ToString());
    }


    private void RemoveConstructorsAndDestructors(List<ISymbol> members)
    {
        // 创建一个新的列表，用于存储排除构造函数和析构函数后的成员
        var filteredMembers = new List<ISymbol>();

        // 遍历原始列表中的每个成员
        foreach (var member in members)
        {
            // 检查成员的类型是否为方法
            if (member is IMethodSymbol methodSymbol)
            {
                // 排除一切编译器生成的方法
                if (!methodSymbol.MethodKind.Equals(MethodKind.Constructor) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.StaticConstructor) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.Destructor) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.PropertyGet) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.PropertySet) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.EventAdd) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.EventRemove) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.BuiltinOperator) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.UserDefinedOperator) &&
                    !methodSymbol.MethodKind.Equals(MethodKind.Conversion)

                    )
                {
                    // 将成员添加到新的列表中
                    filteredMembers.Add(member);
                }
            }
            else
            {
                // 对于非方法成员，直接添加到新的列表中
                filteredMembers.Add(member);
            }
        }

        // 更新原始列表，替换为排除构造函数和析构函数后的成员列表
        members.Clear();
        members.AddRange(filteredMembers);
    }

    private string SymbolAccessibilityToString(Accessibility accessibility)
    {
        switch (accessibility)
        {
            case Accessibility.Public:
                return "public";
            case Accessibility.Internal:
                return "internal";
            case Accessibility.Private:
                return "private";
            default:
                return "private";
        }
    }

}



