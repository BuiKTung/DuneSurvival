using System;
using _Game.Scripts.Core;

namespace _Game.Scripts.Gameplay.Mission
{
    public class MissionManager : Singleton<MissionManager>
    {
        public Mission currentMission;
        private bool isStartMission = false;
        private void Update()
        {
            if(isStartMission == true) 
                currentMission?.UpdateMission();
        }

        public void SetCurrentMission(Mission mission)
        {
            currentMission = mission;
        }

        public void StartMission()
        {
            isStartMission = true;
            currentMission.StartMission();
        }
        public bool MissionCompleted() => currentMission.MissionCompleted();
    }
}
