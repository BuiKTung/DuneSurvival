using _Game.Scripts.Gameplay.Health;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class Player : MonoBehaviour
    {
        public PlayerControls controls { get; private set; }
        public PlayerAim aim { get; private set; } 
        public PlayerMovement movement { get; private set; }
        public PlayerWeaponController weapon { get; private set; }
        public PlayerWeaponVisuals weaponVisuals {get; private set;}
        public PlayerInteraction interaction { get; private set; }
        public PlayerHealth health { get; private set; }
        public Ragdoll ragdoll { get; private set; }
        public bool controlsEnabled { get; private set; }
        private void Awake()
        {
            controls = new PlayerControls();
            aim = GetComponent<PlayerAim>();
            movement = GetComponent<PlayerMovement>();
            weapon = GetComponent<PlayerWeaponController>();
            ragdoll = GetComponent<Ragdoll>();
            weaponVisuals = GetComponent<PlayerWeaponVisuals>();
            interaction = GetComponent<PlayerInteraction>();
            health = GetComponent<PlayerHealth>();
        }
        private void OnEnable()
        {
            controls.Enable();
            controls.Character.UIMissionToolTipSwitch.performed += ctx => UI.UI.Instance.inGameUI.SwitchMissionTooltip();
            controls.Character.UIPause.performed += ctx => UI.UI.Instance.PauseSwitch();
        }
        private void OnDisable()
        {
            controls.Disable();
        }
        public void SetControlsEnabledTo(bool enabled) => controlsEnabled = enabled;
    }
}
