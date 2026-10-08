using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
namespace JumfrogbyMark
{
    public static class Soundplayer // static class
    {
        private static WindowsMediaPlayer sound = new WindowsMediaPlayer();
        private static WindowsMediaPlayer sfxJump = new WindowsMediaPlayer();

        public static void PlayStartMusic() // sound startgame
        {
            sound.URL = @"assets\sfx\sound-startgame.mp3";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 30;
            sound.controls.play();

        }
        public static void PlayGameplayMusic() // sound startgame
        {
            sound.URL = @"assets\sfx\sound-gameplay.mp3";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 30;
            sound.controls.play();

        }

        public static void PlayJumpSound() // Sound frog moves
        {
            sfxJump.URL = @"assets\sfx\sound-jump.mp3";
            sfxJump.settings.volume = 20;
            sfxJump.controls.play();
        }
        public static void PlayDmgSound() // Sound got damge
        {
            sfxJump.URL = @"assets\sfx\sound-dmg.mp3";
            sfxJump.settings.volume = 100;
            sfxJump.controls.play();
        }

        public static void StopMusic()
        {
            sound.controls.stop();
        }
        public static bool ToggleMute()
        {
            sound.settings.mute = !sound.settings.mute;
            sfxJump.settings.mute = sound.settings.mute;
            return sound.settings.mute;
        }
    }
}
