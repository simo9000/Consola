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

        public Outputline()
        {
            initialize();
        }

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

        public Outputline Append(string text)
        {
            text = text.Replace(Environment.NewLine, "</br>");
            buffer.Append(text);
            return this;
        }

        public Outputline Append(char chr)
        {
            buffer.Append(chr);
            return this;
        }

        public Outputline Append(Outputline second)
        {
            return Append(second.generateString());
        }

        private const string colorTemplate = "<span style=\"color:{0}\">{1}</span>";
        public Outputline AppendColor(string text, string color)
        {
            return Append(String.Format(colorTemplate, color, text));
        }

        private const string linkTemplate = "<a href=\"{0}\" target=\"_blank\">{1}</a>";
        public Outputline AppendLink(string url, string text = null)
        {
            if (text == null) text = url;
            return Append(String.Format(linkTemplate, url, text));
        }
        #endregion

    }
}
