using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    enum NoteBeginAutomatonState
    {
        Initial,
        Take,
        TakeA,
        TakeANote
    }

    class NoteBeginAutomaton
    {
        static readonly Dictionary<NoteBeginAutomatonState, Dictionary<string, NoteBeginAutomatonState>> stateTransitions = new Dictionary<NoteBeginAutomatonState, Dictionary<string, NoteBeginAutomatonState>>
        {
            { NoteBeginAutomatonState.Initial, new Dictionary<string, NoteBeginAutomatonState>{ { "take", NoteBeginAutomatonState.Take } } },
            { NoteBeginAutomatonState.Take, new Dictionary<string, NoteBeginAutomatonState>{ { "a", NoteBeginAutomatonState.TakeA } } },
            { NoteBeginAutomatonState.TakeA, new Dictionary<string, NoteBeginAutomatonState>{ { "note", NoteBeginAutomatonState.TakeANote } } }
        };

        NoteBeginAutomatonState state = NoteBeginAutomatonState.Initial;

        public NoteBeginAutomatonState State
        {
            get
            {
                return this.state;
            }
        }

        public void Consume(string word)
        {
            Dictionary<string, NoteBeginAutomatonState> transitions;
            if (stateTransitions.TryGetValue(this.state, out transitions))
            {
                NoteBeginAutomatonState newState;
                if (transitions.TryGetValue(word, out newState))
                {
                    this.state = newState;
                    return;
                }
            }

            // $NOTE: this is not a perfect implementation
            this.state = NoteBeginAutomatonState.Initial;
            if (stateTransitions.TryGetValue(this.state, out transitions))
            {
                NoteBeginAutomatonState newState;
                if (transitions.TryGetValue(word, out newState))
                {
                    this.state = newState;
                }
            }
        }
    }
}
