using _Game.Scripts.Systems;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    [CreateAssetMenu(fileName = "New Timer Mission", menuName = "Missions/Timer - Mission")]
    public class Mission_Timer : Mission
    {
        public float time;
        private float currentTime;
        public override void StartMission()
        {
           currentTime = time;
        }

        public override bool MissionCompleted()
        {
            return currentTime > 0;
        }

        public override void UpdateMission()
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                GameManager.Instance.GameOver();
            }
            
            UpdateMissionUI();
        }

        private void UpdateMissionUI()
        {
            string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");
            string missionText = "Get to evacuation point before plane take off.";
            string missionDetails = "Time left: " + timeText;

            UI.UI.Instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
        }
    }
}
