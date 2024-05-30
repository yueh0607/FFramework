using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace CodeRuleAnalyzer
{
  
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DelegateNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DelegateNaming";
        private static readonly LocalizableString Title = "Delegate naming convention violation";
        private static readonly LocalizableString MessageFormat = "Delegate name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Delegates should follow the PascalCase naming convention.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.DelegateDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var delegateDeclaration = (DelegateDeclarationSyntax)context.Node;
            var delegateName = delegateDeclaration.Identifier.Text;

            if (!IsPascalCase(delegateName))
            {
                var diagnostic = Diagnostic.Create(Rule, delegateDeclaration.Identifier.GetLocation(), delegateName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool IsPascalCase(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
            {
                return false;
            }

            bool hasLowerCase = false;
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsLower(name[i]))
                {
                    hasLowerCase = true;
                }
                else if (char.IsUpper(name[i]) && !hasLowerCase)
                {
                    return false;
                }
            }
            return hasLowerCase;
        }
    }

}
