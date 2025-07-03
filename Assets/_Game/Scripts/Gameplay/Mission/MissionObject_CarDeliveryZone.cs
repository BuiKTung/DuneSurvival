using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    public class MissionObject_CarDeliveryZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Car car = other.GetComponent<Car>();

            if(car != null)
                car.GetComponent<MissionObject_CarToDeliver>().InvokeOnCarDelivery();
        }
    }
}
