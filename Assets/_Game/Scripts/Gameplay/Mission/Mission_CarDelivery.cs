using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    [CreateAssetMenu(fileName = "Car delivery - Mission", menuName = "Missions/Car delivery - Mission")]

    public class Mission_CarDelivery : Mission
    {
        private bool carWasDelivered;
        public override void StartMission()
        {
            string missionText = "Find a functional vehicle.";
            string missionDetails = "Deliver it to the evacuation point.";

            UI.UI.Instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
            FindObjectOfType<MissionObject_CarDeliveryZone>(true).gameObject.SetActive(true);

            carWasDelivered = false;
            MissionObject_CarToDeliver.OnCarDelivery += CarDeliveryCompleted;

            Car[] cars = FindObjectsOfType<Car>();

            foreach (Car car in cars)
            {
                car.AddComponent<MissionObject_CarToDeliver>();
            }

        }

        public override bool MissionCompleted()
        {
            return carWasDelivered;
        }

        private void CarDeliveryCompleted()
        {
            carWasDelivered = true;
            MissionObject_CarToDeliver.OnCarDelivery -= CarDeliveryCompleted;
            
            UI.UI.Instance.inGameUI.UpdateMissionInfo("Get to the evacuation point.");
        }
  
    }
}
