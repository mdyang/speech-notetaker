using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    class WordSequenceTrigger
    {
        private string[] sequence;

        private int index = 0;

        public WordSequenceTrigger(string[] sequence)
        {
            this.sequence = sequence;
        }

        public bool Matched { get; private set; }

        public void Reset()
        {
            this.Matched = false;
            this.index = 0;
        }

        public void Consume(string word)
        {
            if (word.Equals(sequence[index]))
            {
                this.index++;
                if (this.index == this.sequence.Length)
                {
                    this.Matched = true;
                }

                return;
            }

            this.index = 0;
            if (word.Equals(sequence[index]))
            {
                this.index++;
            }
        }
    }
}
