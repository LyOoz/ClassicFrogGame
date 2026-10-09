using System;
using System.Drawing;

namespace JumfrogbyMark
{

    public class GameTimer
    {
        // field
        private float timeLimit;       
        private float timeRemaining;    
        private bool isRunning;
        private bool isTimeOut;

        // Properties
        public float TimeLimit { get => timeLimit; set => timeLimit = value; }
        public float TimeRemaining { get => timeRemaining; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }
        public bool IsTimeOut { get => isTimeOut; }
        public int SecondsRemaining => (int)Math.Ceiling(Math.Max(0, timeRemaining));

        // Constructor
        public GameTimer(float defaultLimitSeconds = 30.0f)
        {
            this.timeLimit = defaultLimitSeconds;
            this.timeRemaining = defaultLimitSeconds;
            this.isRunning = false;
            this.isTimeOut = false;
        }
        public void Start(float? customLimit = null)
        {
            if (customLimit.HasValue)
                this.timeLimit = customLimit.Value;

            this.timeRemaining = this.timeLimit;
            this.isRunning = true;
            this.isTimeOut = false;
        }
        public void Pause()
        {
            this.isRunning = false;
        }
        public void Resume()
        {
            this.isRunning = true;
        }
        public void Reset()
        {
            this.timeRemaining = this.timeLimit;
            this.isTimeOut = false;
            this.isRunning = true;
        }
        public void AddTime(float seconds)
        {
            if (seconds <= 0) return;

            timeRemaining += seconds;
            if (timeRemaining > timeLimit)
                timeRemaining = timeLimit;

            if (timeRemaining > 0)
                isTimeOut = false;
        }
        public bool Update(float deltaTime, PlayerFrog frog = null)
        {
            if (!isRunning || isTimeOut)
                return false;

            timeRemaining -= deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isTimeOut = true;

                // หากเลยเวลากบจะตาย และหัวใจลดลง 1 ดวง
                if (frog != null)
                {
                    frog.TakeDamage();
                    Reset();
                }

                return true; // หมดเวลา
            }

            return false;
        }
    }
}
