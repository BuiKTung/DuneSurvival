using _Game.Scripts.Core;
using _Game.Scripts.Gameplay.MainCharacter;

namespace _Game.Scripts.Systems
{
    public class ControlsManager : Singleton<ControlsManager>
    {
        public PlayerControls controls { get; private set; }
        private Player player;
        
        private void Start()
        {
            controls = GameManager.Instance.player.controls;
            player = GameManager.Instance.player;

            //SwitchToCharacterControls();
        }
        private void OnEnable()
        {
            GameManager.OnGameStateChanged += HandleGameState;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= HandleGameState;
        }
        private void SwitchToCharacterControls()
        {
            controls.UI.Disable();
            controls.Character.Enable();
            player.SetControlsEnabledTo(true);
        }

        private void SwitchToUIControls()
        {
            controls.UI.Enable();
            controls.Character.Disable();
            player.SetControlsEnabledTo(false);
        }

        private void HandleGameState(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    SwitchToUIControls();
                    break;
                case GameState.Playing:
                    SwitchToCharacterControls();
                    break;
                case GameState.Pause:
                    SwitchToUIControls();
                    break;
                case GameState.GameOver:
                    SwitchToUIControls();
                    break;
                case GameState.Victory:
                    SwitchToUIControls();
                    break;
            }
        }
    }
}
