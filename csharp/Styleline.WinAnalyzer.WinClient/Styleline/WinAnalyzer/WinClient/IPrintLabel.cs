namespace Styleline.WinAnalyzer.WinClient
{
    using System;
    using System.IO;

    public interface IPrintLabel
    {
        void Print(UlLabelTO label, TextWriter tw);
    }
}

