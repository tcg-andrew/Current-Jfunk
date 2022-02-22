using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseExplorer
{
    public static class Output
    {
        static FileStream stream;
        static StreamWriter writer;

        public static void Init()
        {
            stream = new FileStream("C:\\DatabaseExplorer.txt", FileMode.Create);
            stream.Position = stream.Length;
            writer = new StreamWriter(stream);
        }

        public static void Write(string o)
        {
            lock (writer)
            {
                writer.WriteLine(o);
                writer.Flush();
            }
        }
    }
}
