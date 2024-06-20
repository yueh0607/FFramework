using System;
using System.Collections.Immutable;
using CodeRuleAnalyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FFramework.CodeRuleAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PascalCaseClassNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PascalCaseClassName";
        private static readonly LocalizableString Title = "Class name should be in PascalCase";
        private static readonly LocalizableString MessageFormat = "Class name '{0}' should be in PascalCase. Class names should start with an uppercase letter and follow the PascalCase convention.";
        private static readonly LocalizableString Description = "Class names should follow the PascalCase naming convention, where the first letter of each word is uppercase.";
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var className = classDeclaration.Identifier.Text;

            if (!PascalUtility.IsPascalCase(className))
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), className);
                context.ReportDiagnostic(diagnostic);
            }
        }

       
    }
}