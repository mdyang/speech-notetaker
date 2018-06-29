using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    interface ISpeechListener
    {
        void Notify(string sentence);
    }
}
