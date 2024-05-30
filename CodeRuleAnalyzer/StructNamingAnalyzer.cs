using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StructNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "StructNaming";
        private static readonly LocalizableString Title = "Struct naming convention violation";
        private static readonly LocalizableString MessageFormat = "Struct name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Struct names should follow the PascalCase naming convention.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.StructDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var structDeclaration = (StructDeclarationSyntax)context.Node;
            var structName = structDeclaration.Identifier.Text;

            if (!PascalUtility.IsPascalCase(structName))
            {
                var diagnostic = Diagnostic.Create(Rule, structDeclaration.Identifier.GetLocation(), structName);
                context.ReportDiagnostic(diagnostic);
            }
        }

       
    }

}
