using UnityEngine;

namespace _Game.Scripts.Gameplay.LevelGeneration
{
    public enum SnapPointType
    {
        Enter,
        Exit,
    }
    public class SnapPoint : MonoBehaviour
    {
        public SnapPointType pointType;

        private void Start()
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }

        private void OnValidate()
        {
            gameObject.name = "SnapPoint - " + pointType.ToString();
        }
    }
}
