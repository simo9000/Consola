using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Consola.Library
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}