using System.Collections;
using _Game.Scripts.Core;
using UnityEngine;

namespace _Game.Scripts.Systems
{
    public class TimeManager : Singleton<TimeManager>
    {
        [SerializeField] private float resumeRate = 3;
        [SerializeField] private float pauseRate = 7;

        private float timeAdjustRate;
        private float targetTimeScale = 1;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SlowMotionFor(1f);
            
            if (Mathf.Abs(Time.timeScale - targetTimeScale) > .05f)
            {
                float adjustRate = Time.unscaledDeltaTime * timeAdjustRate;
                Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, adjustRate);
            }
            else
                Time.timeScale = targetTimeScale;
        }

        public void PauseTime()
        {
            timeAdjustRate = pauseRate;
            targetTimeScale = 0;
        }

        public void ResumeTime()
        {
            timeAdjustRate = resumeRate;
            targetTimeScale = 1;
        }

        public void SlowMotionFor(float seconds) => StartCoroutine(SlowTimeCo(seconds));

        private IEnumerator SlowTimeCo(float seconds)
        {
            targetTimeScale = .5f;
            Time.timeScale = targetTimeScale;
            yield return new WaitForSecondsRealtime(seconds);
            ResumeTime();
        }
    }
}
