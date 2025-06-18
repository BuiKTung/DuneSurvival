using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Utilities
{
    public static class CacheComponent<T> where T : Component
    {
        private static readonly Dictionary<GameObject, T> Caches = new Dictionary<GameObject, T>();

        public static T CacheGetComponent(GameObject gameObject)
        {
            if (!Caches.TryGetValue(gameObject, out var component))
            {
                component = gameObject.GetComponent<T>();
                Caches[gameObject] = component;
            }
            return component;
        }
        public static T CacheGetComponentInParent(GameObject gameObject)
        {
            if (!Caches.TryGetValue(gameObject, out var component))
            {
                component = gameObject.GetComponentInParent<T>();
                Caches[gameObject] = component;
            }
            return component;
        }

        public static void ClearCaches(GameObject gameObject)
        {
            Caches.Remove(gameObject);
        }

        public static void ClearAllCaches()
        {
            Caches.Clear();
        }
    }
}
