using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InterfaceNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "InterfaceNaming";
        private static readonly LocalizableString Title = "Interface naming convention violation";
        private static readonly LocalizableString MessageFormat = "Interface name '{0}' does not start with 'I' or is not in PascalCase";
        private static readonly LocalizableString Description = "Interface names should start with 'I' and follow the PascalCase naming convention.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InterfaceDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var interfaceDeclaration = (InterfaceDeclarationSyntax)context.Node;
            var interfaceName = interfaceDeclaration.Identifier.Text;

            if (!PascalUtility.IsPascalCase(interfaceName) || !interfaceName.StartsWith("I"))
            {
                var diagnostic = Diagnostic.Create(Rule, interfaceDeclaration.Identifier.GetLocation(), interfaceName);
                context.ReportDiagnostic(diagnostic);
            }
        }

       
    }

}
