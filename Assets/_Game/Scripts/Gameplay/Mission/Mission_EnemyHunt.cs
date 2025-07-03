using System.Collections.Generic;
using _Game.Scripts.Gameplay.Enemy;
using _Game.Scripts.Gameplay.LevelGeneration;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    [CreateAssetMenu(fileName = "Hunt - Mission", menuName = "Missions/Hunt - Mission")]

    public class Mission_EnemyHunt : Mission
    {
        public int amountToKill = 12;
        public EnemyType enemyType;

        private int killsToGo;

        public override void StartMission()
        {
            killsToGo = amountToKill;
            UpdateMissionUI();
            MissionObject_HuntTarget.OnTargetKilled += EliminateTarget;
            
            List<Enemy.Enemy> validEnemies = new List<Enemy.Enemy>();

            if (enemyType == EnemyType.Melee)
                validEnemies = LevelGenerator.Instance.GetEnemyList();
            else
            {
                foreach(Enemy.Enemy enemy in LevelGenerator.Instance.GetEnemyList())
                {
                    if (enemy.enemyType == enemyType)
                    {
                        validEnemies.Add(enemy);
                    }
                }
            }

            for (int i = 0; i > amountToKill; i++)
            {
                if(validEnemies.Count <= 0 )
                    return;
                int randomIndex = Random.Range(0, validEnemies.Count);
                validEnemies[randomIndex].AddComponent<MissionObject_HuntTarget>();
                validEnemies.RemoveAt(randomIndex);
            }
        }

        public override bool MissionCompleted()
        {
            return killsToGo <= 0;
        }

        private void EliminateTarget()
        {
            killsToGo--;
            UpdateMissionUI();
            
            if (killsToGo <= 0)
            {
                UI.UI.Instance.inGameUI.UpdateMissionInfo("Get to the evacuation point.");
                MissionObject_HuntTarget.OnTargetKilled -= EliminateTarget;
            }
        }
        private void UpdateMissionUI()
        {
            string missionText = "Eliminate " + amountToKill + " enemies with signal disruptor.";
            string missionDetaiils = "Targets left: " + killsToGo;

            UI.UI.Instance.inGameUI.UpdateMissionInfo(missionText, missionDetaiils);
        }

    }
}
