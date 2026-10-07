using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumfrogbyMark
{
    internal class SoundPlayer
    {
        private System.Media.SoundPlayer player;
        private bool isMuted = false;
        public SoundPlayer()
        {
            player = new System.Media.SoundPlayer();
        }
        public void Play(string filePath)
        {
            if (isMuted)
                return;

            player.Stop();
            player.SoundLocation = filePath;
            player.Load();
            player.PlayLooping();
        }
        public void Stop()
        {
            player.Stop();
        }
        public void Mute()
        {
            isMuted = true;
            player.Stop();
        }
        public void Unmute()
        {
            isMuted = false;
        }
        public bool IsMuted()
        {
            return isMuted;
        }
    }

}
