namespace Styleline.WinAnalyzer.WinClient
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Reflection;

    public class Compiler
    {
        private Compiler()
        {
        }

        public static bool CanCompile(string fullSource, params string[] referencedAssemblies)
        {
            return !DoCompile(fullSource, referencedAssemblies).Errors.HasErrors;
        }

        public static Assembly CompileAssembly(string fullSource, params string[] referencedAssemblies)
        {
            CompilerResults results = DoCompile(fullSource, referencedAssemblies);
            if (results.Errors.HasErrors)
            {
                throw new CompileErrorException(results.Errors[0]);
            }
            return results.CompiledAssembly;
        }

        private static CompilerResults DoCompile(string fullSource, params string[] referencedAssemblies)
        {
            ICodeCompiler compiler = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters options = new CompilerParameters {
                GenerateInMemory = true
            };
            foreach (string str in referencedAssemblies)
            {
                options.ReferencedAssemblies.Add(str);
            }
            return compiler.CompileAssemblyFromSource(options, fullSource);
        }
    }
}

