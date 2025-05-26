using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Core;
using UnityEngine;

namespace _Game.Scripts.Utilities
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int poolSize = 10;
        
        private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        public GameObject GetObject(GameObject prefab)
        {
            if(poolDictionary.ContainsKey(prefab) == false || poolDictionary[prefab].Count == 0)
                InitializeNewPool(prefab);
            
            GameObject objectToGet = poolDictionary[prefab].Dequeue();
            objectToGet.SetActive(true);
            objectToGet.transform.parent = null;
            
            return objectToGet;
        }

        public void ReturnToPoolWaitASecond(GameObject objectToReturn, float delay)
        {
            StartCoroutine(DelayReturn(delay, objectToReturn));
        }

        private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
        {
            yield return new WaitForSeconds(delay);
            
            ReturnToPool(objectToReturn);
        }
        public void ReturnToPool(GameObject ObjectToReturn)
        {
            ObjectToReturn.SetActive(false);
            GameObject originalPrefab = ObjectToReturn.GetComponent<PooledObject>().originalPrefab;
            poolDictionary[originalPrefab].Enqueue(ObjectToReturn);
            ObjectToReturn.transform.SetParent(transform);
        }
        private void InitializeNewPool(GameObject prefab)
        {
            poolDictionary[prefab] = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject(prefab);
            }
        }

        private void CreateNewObject(GameObject prefab)
        {
            GameObject newObject = Instantiate(prefab, transform);
            newObject.AddComponent<PooledObject>().originalPrefab = prefab;
            newObject.SetActive(false); 
            
            poolDictionary[prefab].Enqueue(newObject);
        }
    }
}
