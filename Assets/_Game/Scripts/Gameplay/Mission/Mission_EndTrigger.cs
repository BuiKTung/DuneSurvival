using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    public class Mission_EndTrigger : MonoBehaviour
    {
        private GameObject player;

        private void Start()
        {
            player = GameObject.Find("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != player)
                return;

            if(MissionManager.Instance.MissionCompleted())
                Debug.Log("Level completed!");
        }
    }
}
