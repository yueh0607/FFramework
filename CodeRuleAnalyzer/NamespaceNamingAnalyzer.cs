using System.Collections.Immutable;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamespaceNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NamespaceNaming";
        private static readonly LocalizableString Title = "Namespace naming convention violation";
        private static readonly LocalizableString MessageFormat = "Namespace name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Namespaces should follow the PascalCase naming convention with dot notation.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var namespaceDeclaration = (NamespaceDeclarationSyntax)context.Node;
            var namespaceName = namespaceDeclaration.Name.ToString();

            if (!IsPascalCaseWithDotNotation(namespaceName))
            {
                var diagnostic = Diagnostic.Create(Rule, namespaceDeclaration.Name.GetLocation(), namespaceName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool IsPascalCaseWithDotNotation(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            bool hasLowerCase = false;
            bool foundDot = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '.')
                {
                    if (!hasLowerCase || !char.IsUpper(name[i + 1]))
                    {
                        return false;
                    }
                    foundDot = true;
                    hasLowerCase = false;
                }
                else if (char.IsLower(name[i]))
                {
                    hasLowerCase = true;
                }
                else if (char.IsUpper(name[i]) && !hasLowerCase)
                {
                    return false;
                }
            }
            return foundDot;
        }
    }

}
