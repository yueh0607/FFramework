using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FieldNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "FieldNaming";
        private static readonly LocalizableString Title = "Field naming convention violation";
        private static readonly LocalizableString MessageFormat = "Field name '{0}' does not follow the naming convention";
        private static readonly LocalizableString Description = "Fields should follow the naming convention.";
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

            foreach (var variable in fieldDeclaration.Declaration.Variables)
            {
                var fieldName = variable.Identifier.Text;
                var modifiers = fieldDeclaration.Modifiers;

                if (IsPublicOrInternal(modifiers))
                {
                    if (!IsM_PascalCase(fieldName))
                    {
                        var diagnostic = Diagnostic.Create(Rule, variable.Identifier.GetLocation(), fieldName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                else if (IsPrivateOrProtected(modifiers))
                {
                    if (!IsM_PascalCase(fieldName) && !IsCamelCase(fieldName))
                    {
                        var diagnostic = Diagnostic.Create(Rule, variable.Identifier.GetLocation(), fieldName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private bool IsPublicOrInternal(SyntaxTokenList modifiers)
        {
            return modifiers.Any(SyntaxKind.PublicKeyword) || modifiers.Any(SyntaxKind.InternalKeyword);
        }

        private bool IsPrivateOrProtected(SyntaxTokenList modifiers)
        {
            return modifiers.Any(SyntaxKind.PrivateKeyword) || modifiers.Any(SyntaxKind.ProtectedKeyword);
        }

        private bool IsCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsLower(name[0]))
            {
                return false;
            }

            for (int i = 1; i < name.Length; i++)
            {
                if (!char.IsLetterOrDigit(name[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsM_PascalCase(string name)
        {
            return name.Length > 2 && name.StartsWith("m_") && PascalUtility.IsPascalCase(name.Substring(2));
        }

     
    }
}

