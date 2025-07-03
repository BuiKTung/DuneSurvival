using System.Collections;
using _Game.Scripts.Core;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Systems;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class UI : Singleton<UI>
    {
        public UI_InGame inGameUI { get; private set; }
        public UI_WeaponSelection weaponSelectionUI { get; private set; }
        [SerializeField] private UI_MainMenu mainMenuUI;
        [SerializeField] private UI_Upgrades upgradeUI;
        [SerializeField] private UI_Setting settingUI;
        [SerializeField] private UI_Credit creditUI;
        [SerializeField] private UI_Mission uiMission;
        [SerializeField] private UI_Pause pauseUI;
        [SerializeField] private UI_GameOver gameOverUI;
        [SerializeField] private UI_ComicPanel comicPanelUI;
        public UI_ComicPanel victoryScreenUI;
        #region Button

        [Header("Button on Main Menu")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button creditButton;
        [SerializeField] private Button quitButton;
        
        [Header("Button on Mission Selection")]
        [SerializeField] private Button backMissionButton;
        [SerializeField] private Button nextMissionButton;

        [Header("Button on Weapon Selection")] 
        [SerializeField] private Button backWeaponSelectionButton;
        [SerializeField] private Button nextWeaponSelectionButton;
        
        [Header("Button on Upgrades")]
        [SerializeField] private Button backUpgradeButton;
        
        [Header("Button on Settings")]
        [SerializeField] private Button backSettingsButton;
        
        [Header("Button on Credits")]
        [SerializeField] private Button backCreditsButton;
        
        [Header("Button on Pause")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button backToMainMenuButton;
        
        [Header("Button on Comic Panel")]
        [SerializeField] private Button playOnComicPanelButton;
        #endregion
        
        [SerializeField] private GameObject[] UIElements;
        [Header("Fade Image")]
        [SerializeField] private Image fadeImage;
        protected override void Awake()
        {
            base.Awake();
            inGameUI = GetComponentInChildren<UI_InGame>(true);
            weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
        }

        private void Start()
        {
            AssignEventForButtons();
            AssignInputsUI();
            StartCoroutine(ChangeImageAlpha(0, 1.5f, null));
        }
        private void AssignEventForButtons()
        {
            //MainMenu
            playButton.onClick.AddListener(() => SwitchTo(uiMission.gameObject));
            upgradeButton.onClick.AddListener(() => SwitchTo(upgradeUI.gameObject));
            settingButton.onClick.AddListener(() =>
            {
                SwitchTo(settingUI.gameObject);
                settingUI.LoadSettings();
            });
            creditButton.onClick.AddListener(() => SwitchTo(creditUI.gameObject));
            quitButton.onClick.AddListener(QuitTheGame);
            
            //Mission
            backMissionButton.onClick.AddListener(() => SwitchTo(mainMenuUI.gameObject));
            nextMissionButton.onClick.AddListener(() => SwitchTo(weaponSelectionUI.gameObject));
            
            //Weapon Selection
            backWeaponSelectionButton.onClick.AddListener(() =>SwitchTo(uiMission.gameObject));
            nextWeaponSelectionButton.onClick.AddListener(() =>
            {
                if (weaponSelectionUI.ConfirmWeaponSelection())
                {
                    comicPanelUI.ResetComicPanels();
                    SwitchTo(comicPanelUI.gameObject);
                }
            });
            
            //Comic
            playOnComicPanelButton.onClick.AddListener(StartTheGame);
            
            //Setting
            backSettingsButton.onClick.AddListener(() => SwitchTo(mainMenuUI.gameObject));
            
            //Upgrade
            backUpgradeButton.onClick.AddListener(() => SwitchTo(mainMenuUI.gameObject));
            
            //Credit
            backCreditsButton.onClick.AddListener(() => SwitchTo(mainMenuUI.gameObject));
            
            //Pause
            resumeButton.onClick.AddListener(PauseSwitch);
            backToMainMenuButton.onClick.AddListener(() =>
            {
                SwitchTo(mainMenuUI.gameObject);
                GameManager.Instance.ChangeState(GameState.MainMenu);
            });
        }

        private void StartTheGame()
        {
            StartCoroutine(StartGameSequence());
        }

        private void SwitchTo(GameObject uiToSwitchOn)
        {
            foreach (GameObject go in UIElements)
            {
                go.SetActive(false);
            }
         
            uiToSwitchOn.SetActive(true);
        }

        private void QuitTheGame() => Application.Quit();
        public void RestartTheGame() => StartCoroutine(ChangeImageAlpha(1, 1f, GameManager.Instance.RestartScene));
        public void ShowGameOverUI(string message = "GAME OVER!")
        {
            SwitchTo(gameOverUI.gameObject);
            gameOverUI.ShowGameOverMessage(message);
        }
        public void ShowVictoryScreenUI()
        {
            StartCoroutine(ChangeImageAlpha(1, 1.5f, SwitchToVictoryScreenUI));
        }

        private void SwitchToVictoryScreenUI()
        {
            victoryScreenUI.ResetComicPanels();
            SwitchTo(victoryScreenUI.gameObject);
            
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;
        }
        public void PauseSwitch()
        {
            if(GameManager.Instance.IsState(GameState.MainMenu))
                return;
            
            bool gamePause = pauseUI.gameObject.activeSelf;

            if (gamePause)
            {
                SwitchTo(inGameUI.gameObject);
                GameManager.Instance.ChangeState(GameState.Playing);
                TimeManager.Instance.ResumeTime();
            }
            else 
            {
                SwitchTo(pauseUI.gameObject);
                GameManager.Instance.ChangeState(GameState.Pause);
                TimeManager.Instance.PauseTime();
            }
        }
        private IEnumerator StartGameSequence()
        {

            //THIS SHOULD BE UNCOMMENTED BEFORE MAKING A BUILD
            //StartCoroutine(ChangeImageAlpha(1, 1, null));
            //yield return new WaitForSeconds(1);

            yield return null;
            SwitchTo(inGameUI.gameObject);
            GameManager.Instance.GameStart();
            StartCoroutine(ChangeImageAlpha(0,.1f, null));
        }

        private IEnumerator ChangeImageAlpha(float targetAlpha, float duration,System.Action onComplete)
        {
            float time = 0;
            Color currentColor = fadeImage.color;
            float startAlpha = currentColor.a;

            while(time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

                fadeImage.color = new Color(currentColor.r,currentColor.g, currentColor.b,alpha);
                yield return null;
            }

            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);


            // Call the cimpletion method if it exists
            onComplete?.Invoke();
        }
        private void AssignInputsUI()
        {
            PlayerControls controls = GameManager.Instance.player.controls;

            controls.UI.UIPause.performed += ctx => PauseSwitch();
        }
        private void OnEnable()
        {
            GameManager.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState newState)
        {
            if (newState == GameState.Victory)
            {
                ShowVictoryScreenUI();
            }
        }
        
        //In Scene
        [ContextMenu("Assign Audio To Buttons")]
        public void AssignAudioListenesrsToButtons()
        {
            UI_Button[] buttons = FindObjectsOfType<UI_Button>(true);

            foreach (var button in buttons)
            {
                button.AssignAudioSource();
            }
        }
    }
}
