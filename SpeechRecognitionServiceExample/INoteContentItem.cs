using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    public interface INoteContentItem
    {
        string ToHtml();

        string ToPlainText();
    }
}
