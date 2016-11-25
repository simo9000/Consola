using System;
using System.IO;

namespace Consola.Library.util
{
    /// <summary>
    /// Container for download metadata and stream
    /// </summary>
    public struct Download
    {
        internal string MimeType, FileName;
        internal Stream Content;
        internal Guid Key;
        internal Action Clear;

        /// <summary>
        /// Creates a new download object
        /// </summary>
        /// <param name="FileName">file name for download</param>
        /// <param name="Content">Data Stream (Open for reading)</param>
        /// <param name="MimeType">the mime type to use in the download response header</param>
        /// <param name="DownloadCompletedCallback">Delegate that will be called when download is completed</param>
        public Download(string FileName, Stream Content, string MimeType, Action DownloadCompletedCallback = null)
        {
            Clear = DownloadCompletedCallback ?? (() => { return; });
            Key = Guid.NewGuid();
            this.Content = Content;
            this.Content.Seek(0, SeekOrigin.Begin);
            this.FileName = FileName;
            this.MimeType = MimeType;
        }
    }
}
