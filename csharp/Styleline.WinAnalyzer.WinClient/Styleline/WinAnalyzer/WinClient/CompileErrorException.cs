namespace Styleline.WinAnalyzer.WinClient
{
    using System;
    using System.CodeDom.Compiler;

    public class CompileErrorException : Exception
    {
        public CompileErrorException(CompilerError error) : base(error.ToString())
        {
        }
    }
}

