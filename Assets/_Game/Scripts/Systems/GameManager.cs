using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Core;
using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Mission;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Systems
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        Pause,
        Victory,
    }
    public class GameManager : Singleton<GameManager>
    {
        public Player player;
       
        public static event Action<GameState> OnGameStateChanged;

        public GameState currentState { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Player>();
        }

        private void Start()
        {
            ChangeState(GameState.MainMenu);
        }
        public void GameStart()
        {
            SetDefaultWeaponsForPlayer();
            MissionManager.Instance.StartMission();
            ChangeState(GameState.Playing);
            //LevelGenerator.instance.InitializeGeneration();
            // We start selected mission in a LevelGenerator script ,after we done with level creation.
        }
        public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        private void SetDefaultWeaponsForPlayer()
        {
            List<Weapon_Data> newList = UI.UI.Instance.weaponSelectionUI.SelectedWeaponData();
            player.weapon.SetDefaultWeapon(newList);
        }
        public void ChangeState(GameState newState)
        {
            if (newState == currentState) return;

            currentState = newState;
            
            OnGameStateChanged?.Invoke(newState);
        }
        public bool IsState(GameState gameState)
        {
            return currentState == gameState;
        }

        public void GameOver()
        {
            TimeManager.Instance.SlowMotionFor(1.5f);
            UI.UI.Instance.ShowGameOverUI();
            ChangeState(GameState.GameOver);
            CameraManager.Instance.ChangeCameraDistance(5);
        }
        public void GameCompleted()
        {
            UI.UI.Instance.ShowVictoryScreenUI();
            player.health.currentHealth += 99999; 
            // So player won't die in last second.
        }
    }
}
