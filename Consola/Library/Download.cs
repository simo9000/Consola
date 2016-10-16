using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.Library
{
    public struct Download
    {
        internal string MimeType, FileName;
        internal Stream Content;
        internal Guid Key;
        internal Action Clear;

        public Download(string FileName, Stream Content, string MimeType, Action GarbageCollectionCallBack)
        {
            Clear = GarbageCollectionCallBack;
            Key = Guid.NewGuid();
            this.Content = Content;
            this.FileName = FileName;
            this.MimeType = MimeType;
        }
    }
}
