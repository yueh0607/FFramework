using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RoslynLib;


[Generator]
public class SerializationGenerator : ISourceGenerator
{
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
                //是指定类型
                var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                if (AttributeHelper.VerfiyAttribute(classDeclaration, semanticModel, "FFramework.FPackableAttribute")
                   &&classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    string code = GenerateCode(classDeclaration, semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol);
                    FormatHelper.RemoveLeadingWhitespace(code);
                    SourceText sourceText = SourceText.From(code, Encoding.UTF8);
                    context.AddSource(classDeclaration.Identifier.ValueText + ".Serialization.cs", sourceText);
                }
            }
        }
    }

    public string GenerateCode(ClassDeclarationSyntax syntax, INamedTypeSymbol classSymbol)
    {

        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        var className = classSymbol.Name;

        var serializeMethod = new StringBuilder();
        var deserializeMethod = new StringBuilder();


        serializeMethod.AppendLine($"namespace {namespaceName}");

        serializeMethod.AppendLine("{");
        serializeMethod.AppendLine($"    public partial class {className}");
        serializeMethod.AppendLine("    {");
        serializeMethod.AppendLine("        public void Serialize(ref global::FFramework.DynamicSequence sequence)");
        serializeMethod.AppendLine("        {");

        deserializeMethod.AppendLine($"        public void Deserialize(ref global::FFramework.DynamicSequence sequence)");
        deserializeMethod.AppendLine("        {");

        List<ISymbol> symbols = new List<ISymbol>(20);
        foreach (var member in classSymbol.GetMembers().
            Where(m => !m.GetAttributes().
            Any(a => a.AttributeClass.ToDisplayString() == "FFramework.Serialization.Binary.Binary.PropertyIgnoreAttribute")))
        {
            symbols.Add(member);
        }

        symbols.Sort((x, y) =>
        {
            // 获取x的Order属性值
            var xOrderAttribute = x.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass.ToDisplayString() == "FFramework.Serialization.Binary.Binary.PropertyOrderAttribute");
            int xOrder = xOrderAttribute != null ? (int)xOrderAttribute.ConstructorArguments[0].Value : 0;

            // 获取y的Order属性值
            var yOrderAttribute = y.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass.ToDisplayString() == "FFramework.Serialization.Binary.Binary.PropertyOrderAttribute");
            int yOrder = yOrderAttribute != null ? (int)yOrderAttribute.ConstructorArguments[0].Value : 0;

            // 根据Order属性值进行升序排序
            int result = xOrder.CompareTo(yOrder);

            // 如果Order属性值相同，保持原有顺序
            return result != 0 ? result : 1;
        });

        symbols.ForEach((x) =>
        {
            switch (x)
            {
                case IFieldSymbol field:
                    if (field.IsAbstract || field.IsStatic || field.IsReadOnly || field.IsConst ||
                    field.IsExtern)
                    {
                        break;
                    }

                    HandleField(field, serializeMethod, deserializeMethod);
                    break;

                case IPropertySymbol property:
                    if (property.IsAbstract || property.IsStatic || property.IsReadOnly ||
                    property.SetMethod == null || property.GetMethod == null)
                    {
                        break;
                    }

                    HandleProperty(property, serializeMethod, deserializeMethod);
                    break;
            }
        });


        serializeMethod.AppendLine("        }");  //method
        deserializeMethod.AppendLine("        }");

        string ser = serializeMethod.ToString();
        serializeMethod.Append(deserializeMethod.ToString());

        serializeMethod.AppendLine("    }");   //class
        serializeMethod.AppendLine("}"); 


        return serializeMethod.ToString();
    }

    void HandleField(IFieldSymbol symbol, StringBuilder serializedMethod, StringBuilder deserializedMethod)
    {
        switch (symbol.Type.SpecialType)
        {
            case SpecialType.System_Byte:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadByte(ref sequence);");
                break;
            case SpecialType.System_SByte:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadSByte(ref sequence);");
                break;
            case SpecialType.System_UInt16:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadUInt16(ref sequence);");
                break;
            case SpecialType.System_Int16:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt16(ref sequence);");
                break;
            case SpecialType.System_UInt32:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadUInt32(ref sequence);");
                break;
            case SpecialType.System_Int32:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt32(ref sequence);");
                break;
            case SpecialType.System_UInt64:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"           this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadUInt64(ref sequence);");
                break;
            case SpecialType.System_Int64:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt64(ref sequence);");
                break;
            case SpecialType.System_Boolean:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Decimal:
            case SpecialType.System_Char:
            case SpecialType.System_Enum:
            case SpecialType.System_DateTime:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.ReadWriteUtil.Write<{symbol.Type}>(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.ReadWriteUtil.Read<{symbol.Type}>(ref sequence);");
                break;
        }
    }

    void HandleProperty(IPropertySymbol symbol, StringBuilder serializedMethod, StringBuilder deserializedMethod)
    {
        switch (symbol.Type.SpecialType)
        {
            case SpecialType.System_Byte:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadByte(ref sequence);");
                break;
            case SpecialType.System_SByte:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadSByte(ref sequence);");
                break;
            case SpecialType.System_UInt16:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper..ReadUInt16(ref sequence);");
                break;
            case SpecialType.System_Int16:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt16(ref sequence);");
                break;
            case SpecialType.System_UInt32:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadUInt32(ref sequence);");
                break;
            case SpecialType.System_Int32:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt32(ref sequence);");
                break;
            case SpecialType.System_UInt64:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadUInt64(ref sequence);");
                break;
            case SpecialType.System_Int64:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.VarIntReadWriteHelper.Write(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.VarIntReadWriteHelper.ReadInt64(ref sequence);");
                break;
            case SpecialType.System_Boolean:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Decimal:
            case SpecialType.System_Char:
            case SpecialType.System_Enum:
            case SpecialType.System_DateTime:
                serializedMethod.AppendLine($"global::FFramework.Serialization.Binary.ReadWriteUtil.Write<{symbol.Type}>(ref sequence,{symbol.Name});");
                deserializedMethod.AppendLine($"this.{symbol.Name} = global::FFramework.Serialization.Binary.ReadWriteUtil.Read<{symbol.Type}>(ref sequence);");
                break;
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {

    }
}
