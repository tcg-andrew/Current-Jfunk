using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PowerAnalyzer
{
    public static class InvokeExtension
    {
        public static void InvokeSafe<T>(this T c, Action<T> action) where T: Control
        {
            Action method = () => action(c);
            if (!c.IsDisposed)
            {
                if (c.InvokeRequired)
                {
                    c.Invoke(method);
                }
                else
                {
                    action(c);
                }
            }
        }
    }
}

