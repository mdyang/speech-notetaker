using Microsoft.CognitiveServices.SpeechRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeechToTextWPFSample
{
    class NoteTakingSpeechListener : ISpeechListener
    {
        enum NoteStartEndState
        {
            Initial,
            Started
        }

        enum NoteContentState
        {
            Paragraph,
            List
        }

        private bool priorIsNumber = false;

        private List<string> noteContent = new List<string>();

        private NoteStartEndState noteStartEndState = NoteStartEndState.Initial;

        private NoteContentState noteContentState = NoteContentState.Paragraph;

        private MainWindow mainWnd;

        private WordSequenceTrigger noteBeginTrigger;

        private WordSequenceTrigger noteEndTrigger;

        private WordSequenceTrigger endListTrigger;

        private Note note;

        private HashSet<string> dirtyWordList;

        public NoteTakingSpeechListener(MainWindow mainWnd, string startNoteCmd, string stopNoteCmd, string endListCmd)
        {
            this.mainWnd = mainWnd;
            this.InitializeCommand(startNoteCmd, stopNoteCmd, endListCmd);
            this.Initialize();
        }

        public void InitializeCommand(string startNoteCmd, string stopNoteCmd, string endListCmd)
        {
            this.noteBeginTrigger = new WordSequenceTrigger(startNoteCmd.GetNormalizedTokens().ToArray());
            this.noteEndTrigger = new WordSequenceTrigger(stopNoteCmd.GetNormalizedTokens().ToArray());
            this.endListTrigger = new WordSequenceTrigger(endListCmd.GetNormalizedTokens().ToArray());
        }

        public void Notify(string sentence)
        {
            IEnumerable<string> sentenceTokens = sentence.GetOriginalTokens();
            foreach (string rawToken in sentenceTokens)
            {
                string token = rawToken;
                string normalizedToken = token.GetNormalized();
                if (this.dirtyWordList.Contains(normalizedToken))
                {
                    token = normalizedToken = "[beep]";
                }

                if (noteStartEndState == NoteStartEndState.Started)
                {
                    if ("number".Equals(normalizedToken))
                    {
                        this.priorIsNumber = true;
                        continue;
                    }

                    int neverUsed;
                    if (this.priorIsNumber && (int.TryParse(normalizedToken, out neverUsed) || "one".Equals(normalizedToken)))
                    {
                        this.AppendNewListItemToNoteEnd();
                        continue;
                    }

                    this.AppendTokenToNoteEnd(token);
                    this.endListTrigger.Consume(normalizedToken);
                    if (this.endListTrigger.Matched)
                    {
                        this.endListTrigger.Reset();
                        this.AppendNewParagraphToNoteEnd();
                    }

                    this.noteEndTrigger.Consume(normalizedToken);
                    if (this.noteEndTrigger.Matched)
                    {
                        this.noteEndTrigger.Reset();
                        this.noteStartEndState = NoteStartEndState.Initial;
                        this.mainWnd.NotifyNoteEnd(this.note);
                    }

                    this.mainWnd.FlushNote(this.note);
                }
                else if (noteStartEndState == NoteStartEndState.Initial)
                {
                    this.noteBeginTrigger.Consume(normalizedToken);
                    if (this.noteBeginTrigger.Matched)
                    {
                        this.mainWnd.NotifyNoteStart();
                        this.noteBeginTrigger.Reset();
                        this.noteStartEndState = NoteStartEndState.Started;
                    }
                }
            }
        }

        private void Initialize()
        {
            this.note = new Note();
            this.dirtyWordList = new HashSet<string>();
            string rawList;
            using (StreamReader sr = new StreamReader(new FileStream(@".\en.txt", FileMode.Open)))
            {
                rawList = sr.ReadToEnd();
            }

            foreach (string token in rawList.Split('\n'))
            {
                this.dirtyWordList.Add(token.GetNormalized());
            }
        }

        private void AppendNewListItemToNoteEnd()
        {
            // if note is empty or last item is not list, append a new list
            ListNoteContentItem list = this.note.IsEmpty ? null : this.note.LastItem as ListNoteContentItem;
            if (this.note.IsEmpty || list == null)
            {
                ListNoteContentItem listContent = new ListNoteContentItem();
                listContent.Append(new PlainTextNoteContentItem(string.Empty));
                this.note.Append(listContent);
            }
            else
            {
                // otherwise append a new item to existing list
                list.Append(new PlainTextNoteContentItem(string.Empty));
            }
        }

        private void AppendNewParagraphToNoteEnd()
        {
            this.note.Append(new ParagraphTextNoteContentItem(string.Empty));
        }

        private void AppendTokenToNoteEnd(string token)
        {
            if (this.note.IsEmpty)
            {
                if (this.noteContentState == NoteContentState.Paragraph)
                {
                    this.note.Append(new ParagraphTextNoteContentItem(token));
                }
                else if (this.noteContentState == NoteContentState.List)
                {
                    ListNoteContentItem listContent = new ListNoteContentItem();
                    listContent.Append(new PlainTextNoteContentItem(token));
                    this.note.Append(listContent);
                }

                return;
            }

            INoteContentItem lastItem = this.note.LastItem;
            PlainTextNoteContentItem lastPlainTextContent = lastItem as PlainTextNoteContentItem;
            if (lastPlainTextContent == null)
            {
                lastPlainTextContent = (lastItem as ListNoteContentItem).LastItem;
            }

            lastPlainTextContent.Append(token);
        }
    }
}
