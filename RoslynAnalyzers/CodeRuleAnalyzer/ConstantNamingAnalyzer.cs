using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConstantNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ConstantNaming";
        private static readonly LocalizableString Title = "Constant naming convention violation";
        private static readonly LocalizableString MessageFormat = "Constant name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Constants should follow the UPPER_CASE naming convention with underscores.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (FieldDeclarationSyntax)context.Node;
            if (fieldDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                foreach (var variable in fieldDeclaration.Declaration.Variables)
                {
                    var constantName = variable.Identifier.Text;
                    if (!IsUpperCaseWithUnderscores(constantName))
                    {
                        var diagnostic = Diagnostic.Create(Rule, variable.Identifier.GetLocation(), constantName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private bool IsUpperCaseWithUnderscores(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            bool isUpperCase = true;
            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsUpper(name[i]) && name[i] != '_')
                {
                    isUpperCase = false;
                    break;
                }
            }
            return isUpperCase;
        }
    }

}
