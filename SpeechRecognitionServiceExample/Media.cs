using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    class Media
    {
        private static readonly Semaphore _sem = new Semaphore(0, int.MaxValue);

        private static readonly System.Media.SoundPlayer player = new System.Media.SoundPlayer(@".\LYNC_untag.wav");

        static Media()
        {
            Task.Factory.StartNew(Run);
        }

        public static void Sound()
        {
            _sem.Release();
        }

        private static void Run()
        {
            while(true)
            {
                _sem.WaitOne();
                player.Play();
            }
        }
    }
}
