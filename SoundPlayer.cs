using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
namespace JumfrogbyMark
{
    internal class Soundplayer // static class
    {
        private static WindowsMediaPlayer sound = new WindowsMediaPlayer();
        public static void PlayStartMusic() // sound startgame
        {
            sound.URL = @"assets\sound\sound-startgame.mp3";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 50;
            sound.controls.play();
            
        }

        public static void PlayGameMusic() // Sound Gameplay
        {
            sound.URL = @"";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 50;
            sound.controls.play();
        }

        public static void StopMusic()
        {
            sound.controls.stop();
        }
        public static bool ToggleMute()
        {
            sound.settings.mute = !sound.settings.mute;
            return sound.settings.mute;
        }
    }
}
