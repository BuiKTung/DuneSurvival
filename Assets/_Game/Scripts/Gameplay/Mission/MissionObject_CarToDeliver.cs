using System;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    public class MissionObject_CarToDeliver : MonoBehaviour
    {
        public static event Action OnCarDelivery;

        public void InvokeOnCarDelivery() => OnCarDelivery?.Invoke();
    }
}
