using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PascalCaseMethodNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PascalCaseMethodName";
        private static readonly LocalizableString Title = "Method name should be in PascalCase";
        private static readonly LocalizableString MessageFormat = "Method name '{0}' should be in PascalCase. Method names should start with an uppercase letter and follow the PascalCase convention.";
        private static readonly LocalizableString Description = "Method names should follow the PascalCase naming convention, where the first letter of each word is uppercase.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            var methodName = methodDeclaration.Identifier.Text;

            if (!PascalUtility.IsPascalCase(methodName))
            {
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodName);
                context.ReportDiagnostic(diagnostic);
            }
        }

     
    }

}
