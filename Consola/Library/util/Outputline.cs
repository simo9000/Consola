using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.Library.util
{
    public class Outputline
    {
        private StringBuilder buffer;

        private void initialize()
        {
            buffer = new StringBuilder();
        }

        /// <summary>
        /// Creates an empty output line
        /// </summary>
        public Outputline()
        {
            initialize();
        }

        /// <summary>
        /// Create an output line with unformated text
        /// </summary>
        /// <param name="text">Text to append</param>
        public Outputline(string text)
        {
            initialize();
            buffer.Append(text);
        }

        internal string generateString()
        {
            return buffer.ToString();
        }

        #region append

        /// <summary>
        /// Appends unformated text to output line
        /// </summary>
        /// <param name="text">Text to append</param>
        /// <returns>Output line (for chaining)</returns>
        public Outputline Append(string text)
        {
            text = text.Replace(Environment.NewLine, "</br>");
            buffer.Append(text);
            return this;
        }

        /// <summary>
        /// Append a single character to output line
        /// </summary>
        /// <param name="chr">char to append</param>
        /// <returns>Output line (for chaining)</returns>
        public Outputline Append(char chr)
        {
            buffer.Append(chr);
            return this;
        }

        /// <summary>
        /// Used to combine two output lines
        /// </summary>
        /// <param name="second">line to append</param>
        /// <returns>Output line (for chaining)</returns>
        public Outputline Append(Outputline second)
        {
            return Append(second.generateString());
        }

        private const string colorTemplate = "<span style=\"color:{0}\">{1}</span>";
        /// <summary>
        /// Append colored text to output line
        /// </summary>
        /// <param name="text">Text to append</param>
        /// <param name="color">Text color (name, hex code, or RGB)</param>
        /// <returns></returns>
        public Outputline AppendColor(string text, string color)
        {
            return Append(String.Format(colorTemplate, color, text));
        }

        private const string linkTemplate = "<a href=\"{0}\" target=\"_blank\">{1}</a>";
        /// <summary>
        /// Append link to output line
        /// </summary>
        /// <param name="url">link target</param>
        /// <param name="text">link text (defaults to target if omited)</param>
        /// <returns></returns>
        public Outputline AppendLink(string url, string text = null)
        {
            if (text == null) text = url;
            return Append(String.Format(linkTemplate, url, text));
        }
        #endregion

    }
}
