using System;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    public class MissionObject_HuntTarget : MonoBehaviour
    {
        public static event Action OnTargetKilled; 
        
        public void InvokeOnTargetKilled() => OnTargetKilled?.Invoke();
    }
}
