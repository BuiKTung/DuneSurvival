
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] protected Image buttonImage;
        [SerializeField] protected TextMeshProUGUI buttonText;
        
        [Header("Mouse hover settings")] 
        public float scaleSpeed = 3f;
        public float scaleRate = 1.2f;
        
        private Vector3 defaultScale;
        private Vector3 targetScale;
        
        [Header("Audio")]
        [SerializeField] private AudioSource pointerEnterSFX;
        [SerializeField] private AudioSource pointerDownSFX;
        public virtual void Start()
        {
            defaultScale = transform.localScale;
            targetScale = defaultScale;
        }

        public virtual void Update()
        {
            if (Mathf.Abs(transform.lossyScale.x - targetScale.x) > .01f)
            {
                float scaleValue = 
                    Mathf.Lerp(transform.localScale.x, targetScale.x, Time.deltaTime * scaleSpeed);

                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
        }

        private void ReturnToDefault()
        {
            targetScale = defaultScale;
            
            if(buttonImage != null)
                buttonImage.color = Color.white;
            if(buttonText != null)
                buttonText.color = Color.white;
        }
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            targetScale = defaultScale * scaleRate;
            
            if(buttonImage != null)
                buttonImage.color = Color.yellow;
            if(buttonText != null)
                buttonText.color = Color.yellow;
            
            if (pointerEnterSFX != null)
                pointerEnterSFX.Play();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            ReturnToDefault();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            ReturnToDefault();
            
            if(pointerDownSFX != null)
                pointerDownSFX.Play();
        }
        public void AssignAudioSource()
        {
            pointerEnterSFX = GameObject.Find("UI_PointerEnter").GetComponent<AudioSource>();
            pointerDownSFX = GameObject.Find("UI_PointerDown").GetComponent<AudioSource>();

        }
    }
}
