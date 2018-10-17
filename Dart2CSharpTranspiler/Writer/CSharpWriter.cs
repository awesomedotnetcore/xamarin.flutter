﻿using System.Collections.Generic;
using Dart2CSharpTranspiler.Dart;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dart2CSharpTranspiler.Writer
{
    public class CSharpWriter
    {  
        private readonly DartModel _model;

        /// <summary>
        /// Dictionary of all mixins representing the original name and generated interface name.
        /// </summary>
        public Dictionary<string, string> Mixins { get; }
         
        public static IList<string> Enums = new List<string>();

        private string _namespaceRoot;

        public CSharpWriter(DartModel model, string namespaceRoot)
        {
            _model = model;
            _namespaceRoot = namespaceRoot;
            Mixins = MixinAnalyzer.FindMixins(model);
        }

        /// <summary>
        /// Generates the namespace syntax that represents a dart file.
        /// </summary>
        public NamespaceDeclarationSyntax GenerateFileSyntaxTree(DartFile file)
        {
            var namespaceName = NameGenerator.GenerateNamespaceName(file.Folder, _namespaceRoot);
            var namespaceDeclartion = SyntaxFactory.NamespaceDeclaration(namespaceName)
                      .NormalizeWhitespace();
            namespaceDeclartion = ReferenceGenerator.AddUsings(file, namespaceDeclartion);
            namespaceDeclartion = ClassGenerator.AddClasses(file, namespaceDeclartion, Mixins); 
            namespaceDeclartion = ClassGenerator.AddMixinInterfaces(file, namespaceDeclartion, Mixins);

            return namespaceDeclartion;
        }
    } 
}