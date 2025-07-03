using _Game.Scripts.Gameplay.LevelGeneration;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    [CreateAssetMenu(fileName = "Find Key - Mission", menuName = "Missions/Key - Mission")]
    public class Mission_KeyFind : Mission
    {
        [SerializeField] private GameObject key;
        private bool keyFound;
        
        public override void StartMission()
        {
            MissionObject_Key.OnKeyPickedUp += PickUpKey;

            UI.UI.Instance.inGameUI.UpdateMissionInfo("Find a key-holder. Retrive the key.");
            
            Enemy.Enemy enemy = LevelGenerator.Instance.GetRandomEnemy();
            enemy?.GetComponent<Enemy_DropController>()?.GiveKey(key);
            enemy?.MakeEnemyVip();
        }

        public override bool MissionCompleted()
        {
            return keyFound;
        }

        private void PickUpKey()
        {
            keyFound = true;
            MissionObject_Key.OnKeyPickedUp -= PickUpKey;
            UI.UI.Instance.inGameUI.UpdateMissionInfo("You've got the key! \n Get to the evacuation point.");
        }
    }
}
