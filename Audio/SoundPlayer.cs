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
        private static bool isMuted = false;

        public static bool IsMuted => isMuted;

        public static void PlayStartMusic() // sound startgame
        {
            sound.URL = @"assets\sfx\sound-startgame.mp3";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 30;
            ApplyMute();
            sound.controls.play();

        }
        public static void PlayGameplayMusic() // sound startgame
        {
            sound.URL = @"assets\sfx\sound-gameplay.mp3";
            sound.settings.setMode("loop", true);
            sound.settings.volume = 30;
            ApplyMute();
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
        public static void PlayTakeLotus() 
        {
            sfxJump.URL = @"assets\sfx\sound_score.mp3";
            sfxJump.settings.volume = 100;
            sfxJump.controls.play();
        }
        public static void PlayTakeItem()
        {
            sfxJump.URL = @"assets\sfx\sound_items.mp3";
            sfxJump.settings.volume = 100;
            sfxJump.controls.play();
        }
        public static void PlayWinner()
        {
            sfxJump.settings.setMode("loop", false);
            sfxJump.URL = @"assets\sfx\sound-winner.mp3";
            sfxJump.settings.volume = 100;
            sfxJump.controls.play();
        }
        public static void PlayGameOver()
        {
            sfxJump.settings.setMode("loop", false);
            sfxJump.URL = @"assets\sfx\sound-loser.mp3";
            sfxJump.settings.volume = 100;
            sfxJump.controls.play();
        }


        public static void StopMusic()
        {
            sound.controls.stop();
        }
        public static bool ToggleMute()
        {
            isMuted = !isMuted;
            ApplyMute();
            return isMuted;
        }

        private static void ApplyMute()
        {
            sound.settings.mute = isMuted;
            sfxJump.settings.mute = isMuted;
        }
    }
}
