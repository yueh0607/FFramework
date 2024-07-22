using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RoslynLib;

[Generator]
public class InjectSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver that will be created for each generation pass
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Retrieve the populated receiver
        if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            return;

        var compilation = context.Compilation;
        var attributeSymbol = compilation.GetTypeByMetadataName("FFramework.InjectAttribute");

        if (attributeSymbol == null)
        {
            return; // Ensure InjectAttribute is available
        }

        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (classSymbol == null || TypeHelper.IsNestedClass(classDeclaration,semanticModel))
            {
                continue;
            }

            var fieldsAndProperties = classSymbol.GetMembers()
                .Where(m => m.GetAttributes().Any(a => a.AttributeClass.Equals(attributeSymbol)));

            if (!fieldsAndProperties.Any())
            {
                continue;
            }

            var namespaceName = classSymbol.ContainingNamespace.Name;
            var className = classSymbol.Name;

            var sourceCode = GenerateSourceCode(namespaceName, className, fieldsAndProperties);
            sourceCode = FormatHelper.RemoveLeadingWhitespace(sourceCode);

            context.AddSource($"{className}_Inject.cs", SourceText.From(sourceCode, Encoding.UTF8));
        }
    }

    private string GenerateSourceCode(string namespaceName, string className, IEnumerable<ISymbol> fieldsAndProperties)
    {
        var sb = new StringBuilder();

        if (namespaceName != string.Empty)
        {
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
        }

        sb.AppendLine($"    public partial class {className}");
        sb.AppendLine("    {");
        sb.AppendLine($"        public void Inject(FFramework.Scope scope)");
        sb.AppendLine("        {");

        foreach (var member in fieldsAndProperties)
        {
            var memberName = member.Name;
            var memberType = member is IFieldSymbol field ? field.Type.ToDisplayString() : ((IPropertySymbol)member).Type.ToDisplayString();

            sb.AppendLine($"            this.{memberName} = scope.Resolve<{memberType}>();");
        }

        sb.AppendLine("        }");
        sb.AppendLine($"        public void Inject()");
        sb.AppendLine("        {");
        sb.AppendLine($"            this.Inject(FFramework.Scope.Global);");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
        if (namespaceName != string.Empty)
        {
            sb.AppendLine("}");
        }
        
        return sb.ToString();
    }

    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Any class with at least one field or property with the [Inject] attribute is a candidate
            if (syntaxNode is ClassDeclarationSyntax classDeclaration  &&
                (classDeclaration.Members.OfType<BaseFieldDeclarationSyntax>().Any(HasInjectAttribute) ||
                classDeclaration.Members.OfType<PropertyDeclarationSyntax>().Any(HasInjectAttribute)))
            {
                CandidateClasses.Add(classDeclaration);
            }
        }

        private static bool HasInjectAttribute(BaseFieldDeclarationSyntax fieldDeclaration)
        {
            return fieldDeclaration.AttributeLists
                .SelectMany(attrList => attrList.Attributes)
                .Any(attr => attr.Name.ToString().EndsWith("Inject"));
        }

        private static bool HasInjectAttribute(PropertyDeclarationSyntax propertyDeclaration)
        {
            return propertyDeclaration.AttributeLists
                .SelectMany(attrList => attrList.Attributes)
                .Any(attr => attr.Name.ToString().EndsWith("Inject"));
        }
    }
}
