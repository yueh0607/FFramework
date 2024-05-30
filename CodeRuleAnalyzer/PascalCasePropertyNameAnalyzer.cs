using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PascalCasePropertyNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PascalCasePropertyName";
        private static readonly LocalizableString Title = "Property name should be in PascalCase";
        private static readonly LocalizableString MessageFormat = "Property name '{0}' should be in PascalCase. Property names should start with an uppercase letter and follow the PascalCase convention.";
        private static readonly LocalizableString Description = "Property names should follow the PascalCase naming convention, where the first letter of each word is uppercase.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var propertyDeclaration = (PropertyDeclarationSyntax)context.Node;
            var propertyName = propertyDeclaration.Identifier.Text;

            if (!PascalUtility.IsPascalCase(propertyName))
            {
                var diagnostic = Diagnostic.Create(Rule, propertyDeclaration.Identifier.GetLocation(), propertyName);
                context.ReportDiagnostic(diagnostic);
            }
        }

    }

}
