using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EnumNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EnumNaming";
        private static readonly LocalizableString Title = "Enum naming convention violation";
        private static readonly LocalizableString MessageFormat = "Enum name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Enums should follow the naming convention of starting with 'E' and using PascalCase.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.EnumDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;
            var enumName = enumDeclaration.Identifier.Text;

            if (!IsEnumNamingConvention(enumName))
            {
                var diagnostic = Diagnostic.Create(Rule, enumDeclaration.Identifier.GetLocation(), enumName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool IsEnumNamingConvention(string name)
        {
            return name.Length > 1 && name.StartsWith("E") && PascalUtility.IsPascalCase(name.Substring(1));
        }
    }
}
