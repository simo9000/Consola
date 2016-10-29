using System;
using System.IO;
using System.Text;

using Consola.Library;
using Consola.Library.util;

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
