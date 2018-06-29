using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    class SpeechBus
    {
        public static readonly ConcurrentQueue<string> QUEUE = new ConcurrentQueue<string>();
        public static readonly Semaphore QUEUE_EMPTY_SEMAPHORE = new Semaphore(0, int.MaxValue);

        private static readonly List<ISpeechListener> listeners = new List<ISpeechListener>();

        public static void RegisterListener(ISpeechListener listener)
        {
            SpeechBus.listeners.Add(listener);
        }

        public static void Run()
        {
            while (true)
            {
                SpeechBus.QUEUE_EMPTY_SEMAPHORE.WaitOne();
                string sentence;
                SpeechBus.QUEUE.TryDequeue(out sentence);

                foreach (ISpeechListener listener in SpeechBus.listeners)
                {
                    listener.Notify(sentence);
                }
            }
        }
    }
}
