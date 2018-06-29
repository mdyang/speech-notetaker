using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    enum NoteEndAutomatonState
    {
        Initial,
        That,
        ThatIs,
        ThatIsThe,
        ThatIsTheNote
    }

    class NoteEndAutomaton
    {
        static readonly Dictionary<NoteEndAutomatonState, Dictionary<string, NoteEndAutomatonState>> stateTransitions = new Dictionary<NoteEndAutomatonState, Dictionary<string, NoteEndAutomatonState>>
        {
            { NoteEndAutomatonState.Initial, new Dictionary<string, NoteEndAutomatonState>{ { "that", NoteEndAutomatonState.That } } },
            { NoteEndAutomatonState.That, new Dictionary<string, NoteEndAutomatonState>{ { "is", NoteEndAutomatonState.ThatIs } } },
            { NoteEndAutomatonState.ThatIs, new Dictionary<string, NoteEndAutomatonState>{ { "the", NoteEndAutomatonState.ThatIsThe } } },
            { NoteEndAutomatonState.ThatIsThe, new Dictionary<string, NoteEndAutomatonState>{ { "note", NoteEndAutomatonState.ThatIsTheNote } } }
        };

        NoteEndAutomatonState state = NoteEndAutomatonState.Initial;

        public NoteEndAutomatonState State
        {
            get
            {
                return this.state;
            }
        }

        public void Consume(string word)
        {
            Dictionary<string, NoteEndAutomatonState> transitions;
            if (stateTransitions.TryGetValue(this.state, out transitions))
            {
                NoteEndAutomatonState newState;
                if (transitions.TryGetValue(word, out newState))
                {
                    this.state = newState;
                    return;
                }
            }

            this.state = NoteEndAutomatonState.Initial;
            if (stateTransitions.TryGetValue(this.state, out transitions))
            {
                NoteEndAutomatonState newState;
                if (transitions.TryGetValue(word, out newState))
                {
                    this.state = newState;
                }
            }
        }
    }
}
