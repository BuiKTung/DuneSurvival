using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
   public abstract class Mission : ScriptableObject
   {
      public string missionName;
      [TextArea]
      public string missionDescription;
   
      public abstract void StartMission();
      public abstract bool MissionCompleted();
      public virtual void UpdateMission(){}
   }
}
