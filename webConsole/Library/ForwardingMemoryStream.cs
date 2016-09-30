using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace webConsole.Library
{
    public class ForwardingMemoryStream : MemoryStream
    {
        public Action<string> writeEvent;

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            string result = Encoding.Default.GetString(buffer.Take(count).ToArray());
            writeEvent(result);
        }

    }
}