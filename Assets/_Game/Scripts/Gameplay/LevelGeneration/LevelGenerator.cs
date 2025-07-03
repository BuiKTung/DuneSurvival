using System.Collections.Generic;
using _Game.Scripts.Core;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.LevelGeneration
{
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        private List<Enemy.Enemy> enemyList;
        public List<Enemy.Enemy> GetEnemyList() => enemyList;
        
        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private List<Transform> levelParts;
        [SerializeField] private Transform lastLevelPart ;
        private List<Transform> generatedLevelParts = new List<Transform>();
        private List<Transform> currentLevelParts;
        
        [SerializeField] private SnapPoint nextSnapPoint;
        private SnapPoint defaultSnapPoint;
        
        [Space]
        [SerializeField] private float generationCooldown;
        private float cooldownTimer;
        private bool generationOver;

        private void Start()
        {
            defaultSnapPoint = nextSnapPoint;
            InitializeGeneration();
        }

        private void Update()
        {
            if (generationOver)
                return;
            
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0)
            {
                if (currentLevelParts.Count > 0)
                {
                    cooldownTimer = generationCooldown;
                    GenerateNextLevelPart();
                }
                else if (generationOver == false)
                {
                    FinishGeneration();
                }
            }
        }
        [ContextMenu("Reset Generation")]
        private void InitializeGeneration()
        {
            generationOver = false;
            currentLevelParts = new List<Transform>(levelParts);
            enemyList = new List<Enemy.Enemy>();
            nextSnapPoint = defaultSnapPoint;

            DestroyOldLevelPartsAndEnemies();
        }

        private void DestroyOldLevelPartsAndEnemies()
        {
            foreach (Transform level in generatedLevelParts)
            {
                Destroy(level.gameObject);   
            }

            generatedLevelParts = new List<Transform>();
            enemyList = new List<Enemy.Enemy>();
        }

        private void FinishGeneration()
        {
            generationOver = true;
            GenerateNextLevelPart();
            navMeshSurface.BuildNavMesh();
            foreach (Enemy.Enemy enemy in enemyList)
            {
                enemy.transform.parent = null;
                enemy.gameObject.SetActive(true);
            }
        }

        [ContextMenu("Generate Level")]
        private void GenerateNextLevelPart()
        {
            Transform newPart = null;
            if (generationOver == true)
                newPart = Instantiate(lastLevelPart);
            else
                newPart = Instantiate(ChooseRandomPart()); 
            
            generatedLevelParts.Add(newPart);
            
            LevelPart newLevelPart = newPart.GetComponent<LevelPart>();
            newLevelPart.SnapAndAlignPartTo(nextSnapPoint);

            if (newLevelPart.IntersectionDetected())
            {
                InitializeGeneration();
                return; 
            }
            
            nextSnapPoint = newLevelPart.GetExitPoint();
            enemyList.AddRange(newLevelPart.MyEnemies());
        }
        private Transform ChooseRandomPart()
        {
            int randomIndex = Random.Range(0, currentLevelParts.Count);
            
            Transform choosenPart = currentLevelParts[randomIndex];
            
            currentLevelParts.RemoveAt(randomIndex);
            
            return choosenPart;
        }

        public Enemy.Enemy GetRandomEnemy()
        {
            if(enemyList.Count == 0 )
                return null;
            int randomIndex = Random.Range(0, enemyList.Count);
            return enemyList[randomIndex];
        }
        
    }
}
