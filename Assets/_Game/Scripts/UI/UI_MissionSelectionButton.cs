using _Game.Scripts.Gameplay.Mission;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI
{
    public class UI_MissionSelectionButton : UI_Button
    {
        [SerializeField]private UI_MissionSelection missionUI;
        [SerializeField] private Mission myMission;
        private TextMeshProUGUI myText;
        private bool isSelected = false;
        private void OnValidate()
        {
            gameObject.name = "Button - Select Mission: " + myMission.missionName;
        }

        public override void Start()
        {
            base.Start();
            buttonText.text = myMission.missionName;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            missionUI.UpdateMissionDesicription(myMission.missionDescription);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (isSelected == false)
                missionUI.UpdateMissionDesicription("Choose a mission");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isSelected = true;
            MissionManager.Instance.SetCurrentMission(myMission);
        }
    }
}
