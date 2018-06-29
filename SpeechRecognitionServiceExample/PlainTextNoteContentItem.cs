using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SpeechToTextWPFSample
{
    class PlainTextNoteContentItem : INoteContentItem
    {
        private StringBuilder sb;

        public PlainTextNoteContentItem(string initialText)
        {
            this.sb = new StringBuilder(initialText);
        }

        public void Append(string text)
        {
            this.sb.Append($" {text}");
        }

        public virtual string ToHtml()
        {
            return $"{HttpUtility.HtmlEncode(sb.ToString())}";
        }

        public string ToPlainText()
        {
            return sb.ToString();
        }
    }

    class ParagraphTextNoteContentItem : PlainTextNoteContentItem
    {
        public ParagraphTextNoteContentItem(string initialText) : base(initialText) { }

        public override string ToHtml()
        {
            return $"<p>{base.ToHtml()}</p>";
        }
    }
}
