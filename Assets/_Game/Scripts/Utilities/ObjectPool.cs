using System.Collections.Generic;
using _Game.Scripts.Core;
using UnityEngine;

namespace _Game.Scripts.Utilities
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int poolSize = 10;
        private Queue<GameObject> bulletPool;
        private void Start()
        {
            CreateInitialPool();
        }
        public GameObject GetBullet()
        {
            if(bulletPool.Count == 0)
                CreateNewBullet();
            GameObject bulletToGet = bulletPool.Dequeue();
            bulletToGet.SetActive(true);
            bulletToGet.transform.parent = null;
            return bulletToGet;
        }

        public void ReturnToPool(GameObject bulletToReturn)
        {
            bulletToReturn.SetActive(false);
            bulletPool.Enqueue(bulletToReturn); 
            bulletToReturn.transform.SetParent(transform);
        }
        private void CreateInitialPool()
        {
            bulletPool = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            newBullet.SetActive(false); 
            bulletPool.Enqueue(newBullet);
        }
    }
}
