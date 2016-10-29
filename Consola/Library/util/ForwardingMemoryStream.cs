using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Consola.Library.util
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    internal class ForwardingMemoryStream : MemoryStream
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