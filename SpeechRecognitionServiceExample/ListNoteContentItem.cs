using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    class ListNoteContentItem : INoteContentItem
    {
        private List<PlainTextNoteContentItem> listItems = new List<PlainTextNoteContentItem>();

        public ListNoteContentItem()
        {
        }

        public bool IsEmpty { get { return this.listItems.Count == 0; } }

        public PlainTextNoteContentItem LastItem { get { return this.listItems[this.listItems.Count - 1]; } }

        public void Append(PlainTextNoteContentItem listItem)
        {
            this.listItems.Add(listItem);
        }

        public string ToHtml()
        {
            return $"<ol>{string.Concat(this.listItems.Select(i => $"<li>{i.ToHtml()}</li>"))}</ol>";
        }

        public string ToPlainText()
        {
            return string.Join("\n", this.listItems.Select(i => $"- {i.ToPlainText()}"));
        }
    }
}
