using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI
{
    public class UI_GameOver : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameOverText;
    
        public void ShowGameOverMessage(string message)
        {
            gameOverText.text = message;
        }
    }
}
