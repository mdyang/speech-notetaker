using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    public class Note
    {
        private List<INoteContentItem> noteContent = new List<INoteContentItem>();

        public bool IsEmpty
        {
            get { return this.noteContent.Count == 0; }
        }

        public INoteContentItem LastItem
        {
            get
            {
                return this.noteContent[this.noteContent.Count - 1];
            }
        }

        public void Append(INoteContentItem item)
        {
            this.noteContent.Add(item);
        }

        public string ToHtml()
        {
            return string.Concat(this.noteContent.Select(i => i.ToHtml()));
        }

        public string ToPlainText()
        {
            return string.Join("\n", this.noteContent.Select(i => i.ToPlainText()));
        }
    }
}
