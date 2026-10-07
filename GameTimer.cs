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

        // คอนสตรัคเตอร์
        public GameTimer(float defaultLimitSeconds = 30.0f)
        {
            this.timeLimit = defaultLimitSeconds;
            this.timeRemaining = defaultLimitSeconds;
            this.isRunning = false;
            this.isTimeOut = false;
        }

        /// <summary>
        /// เริ่มจับเวลาด่านใหม่
        /// </summary>
        public void Start(float? customLimit = null)
        {
            if (customLimit.HasValue)
                this.timeLimit = customLimit.Value;

            this.timeRemaining = this.timeLimit;
            this.isRunning = true;
            this.isTimeOut = false;
        }

        /// <summary>
        /// หยุดเวลาชั่วคราว
        /// </summary>
        public void Pause()
        {
            this.isRunning = false;
        }

        /// <summary>
        /// เล่นต่อ
        /// </summary>
        public void Resume()
        {
            this.isRunning = true;
        }

        /// <summary>
        /// รีเซ็ตเวลากลับไปค่าเริ่มต้นของด่าน
        /// </summary>
        public void Reset()
        {
            this.timeRemaining = this.timeLimit;
            this.isTimeOut = false;
            this.isRunning = true;
        }

        /// <summary>
        /// อัปเดตการลดลงของเวลา
        /// </summary>
        /// <param name="deltaTime">เวลาที่ผ่านไปในเฟรมนี้ (วินาที)</param>
        /// <param name="frog">กบผู้เล่นที่จะถูกลดหัวใจเมื่อหมดเวลา</param>
        /// <returns>true หากหมดเวลาและกบตาย</returns>
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
                    // รีเซ็ตเวลาสำหรับรอบต่อไป
                    Reset();
                }

                return true; // หมดเวลา
            }

            return false;
        }
    }
}
