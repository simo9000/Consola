using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consola.Library;
using System.IO;

namespace Tests
{
    public class BasicFunctions : Scriptable
    {
        [Description("Basic Name field")]
        public string Name { get; set; }

        public BasicFunctions(ScriptSession session)
        {
            session.startupMessage = String.Empty;
        }

        [Description("Prints Hello World")]
        public void printMessage()
        {
            Session.WriteLine("Hello World");
        }

        [Description("Downloads file")]
        public void download(string content)
        {
            MemoryStream stream = new MemoryStream();
            byte[] data = Encoding.Default.GetBytes(content);
            stream.Write(data, 0, data.Length);
            Session.pushDownload(new Download("test.txt", stream, "text/plain"));
        }
    }
}
