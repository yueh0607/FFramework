using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeRuleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PublicMemberDocumentationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PublicMemberDocumentation";
        private static readonly LocalizableString Title = "Public member documentation violation";
        private static readonly LocalizableString MessageFormat = "Public {0} '{1}' is missing documentation";
        private static readonly LocalizableString Description = "Public methods, properties, and fields should have XML documentation.";
        private const string Category = "Documentation";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration, SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            switch (context.Node)
            {
                case MethodDeclarationSyntax methodDeclaration:
                    AnalyzeMethod(context, methodDeclaration);
                    break;
                case PropertyDeclarationSyntax propertyDeclaration:
                    AnalyzeProperty(context, propertyDeclaration);
                    break;
                case FieldDeclarationSyntax fieldDeclaration:
                    AnalyzeField(context, fieldDeclaration);
                    break;
            }
        }

        private void AnalyzeMethod(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword) && !HasDocumentation(methodDeclaration))
            {
                var methodName = methodDeclaration.Identifier.Text;
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), "method", methodName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeProperty(SyntaxNodeAnalysisContext context, PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword) && !HasDocumentation(propertyDeclaration))
            {
                var propertyName = propertyDeclaration.Identifier.Text;
                var diagnostic = Diagnostic.Create(Rule, propertyDeclaration.GetLocation(), "property", propertyName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeField(SyntaxNodeAnalysisContext context, FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword) && !HasDocumentation(fieldDeclaration))
            {
                var fieldName = fieldDeclaration.Declaration.Variables.First().Identifier.Text;
                var diagnostic = Diagnostic.Create(Rule, fieldDeclaration.GetLocation(), "field", fieldName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool HasDocumentation(SyntaxNode node)
        {
            var leadingTrivia = node.GetLeadingTrivia();
            foreach (var trivia in leadingTrivia)
            {
                if (trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
