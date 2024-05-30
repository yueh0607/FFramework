using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Generator]
public class CallEventExtensionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (SyntaxTree syntaxTree in syntaxTrees)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            IEnumerable<InterfaceDeclarationSyntax> interfaces = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>();
            foreach (InterfaceDeclarationSyntax @interface in interfaces)
            {
                if (IsCallEventInterface(@interface, out string methodName,out string returnType, out string[] parameterTypes, out string[] parameterNames, out string namespaceName))
                {
                    string extensionMethodClass = GenerateExtensionMethodClass(@interface.Identifier.ToString(), namespaceName, methodName,returnType, parameterTypes, parameterNames);
                    stringBuilder.AppendLine(extensionMethodClass);

                    SourceText sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);

                    context.AddSource($"CallEvent_{@interface.Identifier.Text}_Extensions.cs", sourceText);

                    stringBuilder.Clear();
                }
            }
        }

    }
    private bool IsCallEventInterface(InterfaceDeclarationSyntax @interface, out string methodName, out string returnType, out string[] parameterType, out string[] parameterNames, out string namespaceName)
    {
        methodName = null;
        returnType = null;
        parameterType = null;
        parameterNames = null;
        namespaceName = null;

        var namespaceDeclaration = @interface.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        if (namespaceDeclaration != null)
        {
            namespaceName = namespaceDeclaration.Name.ToString();
        }

        if (@interface.BaseList != null)
        {
            foreach (var baseType in @interface.BaseList.Types)
            {
                var baseInterface = baseType.Type as GenericNameSyntax;
                if (baseInterface != null && baseInterface.Identifier.Text == "ICallEvent")
                {
                    var genericTypeArguments = baseInterface.TypeArgumentList.Arguments;
                    string tempMethodName = null;
                    string tempReturnType = null;
                    List<string> tempParameterType = new List<string>();
                    List<string> tempParameterNames = new List<string>();

                    var matchingMethod = @interface.Members.FirstOrDefault(member =>
                    {
                        if (member is MethodDeclarationSyntax methodDeclaration)
                        {
                            var parameters = methodDeclaration.ParameterList.Parameters;
                            if (parameters.Count == genericTypeArguments.Count - 1) // Subtract 1 to exclude the return type
                            {
                                bool allParametersMatch = true;
                                for (int i = 0; i < parameters.Count; i++)
                                {
                                    var parameterTypeName = parameters[i].Type.ToString();
                                    if (parameterTypeName != genericTypeArguments[i].ToString())
                                    {
                                        allParametersMatch = false;
                                        break;
                                    }
                                    tempParameterNames.Add(parameters[i].Identifier.Text);
                                    tempParameterType.Add(parameterTypeName);
                                }

                                // Extract the return type from the last generic argument
                                tempReturnType = genericTypeArguments[genericTypeArguments.Count - 1].ToString();

                                if (allParametersMatch)
                                {
                                    tempMethodName = methodDeclaration.Identifier.Text;
                                    return true;
                                }
                            }
                        }
                        return false;
                    });

                    methodName = tempMethodName;
                    returnType = tempReturnType;
                    parameterType = tempParameterType.ToArray();
                    parameterNames = tempParameterNames.ToArray();

                    if (matchingMethod != null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private string GenerateExtensionMethodClass(string interfaceName, string namespaceName, string methodName ,string returnType, string[] parameterTypes, string[] parameterNames)
    {
        string signature = string.Join(", ", parameterTypes.Zip(parameterNames, (type, name) => $"{type} {name}"));
        string inputParameters = string.Join(", ", parameterNames);

        string listName = $"q1588_57_list_13845_812841";
        string objName = $"_655__obj_333695__254";
        string resultName = $"_23result_99875265_1284622821";
        string publisherName = $"publisher_3_3_8184";
        string posName = $"___688_pos_5887_4531_2";
        string convertObjName = $"convertObj_3_3_8184";

        // Generate the extension method static class
        string extensionMethodClass = $@"
using global::System;
using global::System.Buffers;
using {namespaceName};

namespace FFramework
{{
    public static class IMyCallPublishExtensions
    {{
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static {returnType} Call(this object {objName}, {signature})
        {{
            if({objName} is {interfaceName} {convertObjName})
                return {convertObjName}.{methodName}({inputParameters});
            throw new global::System.InvalidCastException(""No such call event"");
        }}

        public static {returnType} Call<T>(this IEventPublisher {publisherName}, {signature}) where T : {interfaceName}
        {{
            DynamicQueue<IEventListener> {listName} = {publisherName}.GetPublishableEvents<T>(typeof(T));
            {returnType} {resultName} = default;
            if ({listName} != null && {listName}.Count > 0)
            {{
                {listName}.StartEnum();
                while ({listName}.MoveNext(out IEventListener {objName}))
                {{
                    {resultName} = ((T){objName}).Call({inputParameters});
                    {listName}.Return({objName});
                }}
                {listName}.EndEnum();
                return {resultName};
            }}
            return default;
        }}

        public static ValueTuple<int,{returnType}[]> CallAll<T>(this IEventPublisher {publisherName}, {signature}) where T : {interfaceName}
        {{
            DynamicQueue<IEventListener> {listName} = {publisherName}.GetPublishableEvents<T>(typeof(T));
            if ({listName} != null && {listName}.Count > 0)
            {{
                {returnType}[] {resultName} = ArrayPool<{returnType}>.Shared.Rent({listName}.Count);
                global::System.Int32 {posName} = 0;
                {listName}.StartEnum();
                while ({listName}.MoveNext(out IEventListener {objName}))
                {{
                    {resultName}[{posName}++] = ((T){objName}).{methodName}({inputParameters});
                    {listName}.Return({objName});
                }}
                {listName}.EndEnum();
                return ({listName}.Count,{resultName});    
            }}
            return (0,null);
        }}
    }}
}}";

        return extensionMethodClass;
    }

    public void Initialize(GeneratorInitializationContext context)
    {

    }
}
