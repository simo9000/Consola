using System;
using System.IO;
using System.Text;

using Consola.Library;
using Consola.Library.util;
using System.Collections.Generic;

namespace Tests
{
    public class BasicFunctions : Scriptable
    {
        #region hidden
        // these members should not apper in show()
        [Hidden]
        public int hiddenField; 

        [Hidden]
        public int hiddenProperty { get; set; }

        [Hidden]
        public void hiddenMethod() { }

        #endregion

        [Description("Basic Id field")]
        public int id;

        [Description("Nullable field")]
        public byte? Byte;

        [Description("Generic field")]
        public Dictionary<int, string> Dictionary;

        [Description("Basic Name property")]
        public string Name { get; set; }

        [Description("Nullable property")]
        public DateTime? Time { get; set; }

        [Description("Generic property")]
        public IEnumerable<DateTime> Dates { get; set; }

        public BasicFunctions(ScriptSession session)
        {
            session.startupMessage = String.Empty;
        }

        [Description("Prints Hello World")]
        public void printMessage()
        {
            Session.WriteLine("Hello World");
        }

        [Description("Prints List")]
        public void printList()
        {
            Session.WriteLine("Line 1");
            Session.WriteLine("Line 2");
        }

        [Description("Prints Html")]
        public void printHtml()
        {
            Outputline line = new Outputline();
            line.AppendColor("text", "rgba(255, 0, 0, 1)");
            Session.WriteLine(line);
        }

        [Description("Prints Html List")]
        public void printHtmlList()
        {
            Outputline line1 = new Outputline();
            line1.AppendColor("Line 1", "rgba(0, 128, 0, 1)");
            Session.WriteLine(line1); 
            Outputline line2 = new Outputline();
            line2.AppendLink("http://home.simo9000.com/git", "Line 2");
            Session.WriteLine(line2);
        }

        [Description("Downloads file")]
        public void download(string content)
        {
            MemoryStream stream = new MemoryStream();
            byte[] data = Encoding.Default.GetBytes(content);
            stream.Write(data, 0, data.Length);
            Session.pushDownload(new Download("test.txt", stream, "text/plain"));
        }

        [Description("Method with nullables in signature")]
        public int? getNullableInt(bool? b)
        {
            return null;
        }

        [Description("Method accepting DateTime")]
        public void printMonth(DateTime date)
        {
            Session.WriteLine(date.Month.ToString());
        }
    }
}
