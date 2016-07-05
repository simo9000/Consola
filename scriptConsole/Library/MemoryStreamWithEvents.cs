using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace scriptConsole.Library
{
    public class MemoryStreamWithEvents : MemoryStream
    {
        public Action<string> writeEvent;

        private void fireWriteEvent()
        {
            StreamReader reader = new StreamReader(this);
            writeEvent(reader.ReadToEnd());
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            string result = Encoding.Default.GetString(buffer.Take(count).ToArray());
            writeEvent(result);
        }

        public override void WriteTo(Stream stream)
        {
            base.WriteTo(stream);
            //fireWriteEvent();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            base.EndWrite(asyncResult);
            //fireWriteEvent();
        }

    }
}