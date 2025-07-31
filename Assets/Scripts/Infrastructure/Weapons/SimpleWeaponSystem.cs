using Scripts.Weapons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Infrastructure.Weapons
{
    public sealed class SimpleWeaponSystem : MonoBehaviour
    {
        [SerializeField]
        private List<BaseSimpleWeapon> _weaponSlots;

        private PlayerInputActions _playerInput;

        private int _activeSlot = -1;

        private ISemiAutoWeapon _semiAutoWeapon;
        private IFullAutoWeapon _fullAutoWeapon;

        private void Awake()
        {
            _playerInput = new PlayerInputActions();
            ChangeWeapon(0);
        }

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.Fire.started += HandleFireInput;
            _playerInput.Player.Fire.performed += HandleFireInput;
            _playerInput.Player.Fire.canceled += HandleFireInput;

            _playerInput.Player.Slot1.performed += OnSlot1Performed;
            _playerInput.Player.Slot2.performed += OnSlot2Performed;
            _playerInput.Player.Slot3.performed += OnSlot3Performed;
        }
        private void OnDisable()
        {
            _playerInput.Disable();

            _playerInput.Player.Fire.started -= HandleFireInput;
            _playerInput.Player.Fire.performed -= HandleFireInput;
            _playerInput.Player.Fire.canceled -= HandleFireInput;

            _playerInput.Player.Slot1.performed -= OnSlot1Performed;
            _playerInput.Player.Slot2.performed -= OnSlot2Performed;
            _playerInput.Player.Slot3.performed -= OnSlot3Performed;
        }

        public void ChangeWeapon(int slotId)
        {
            if (slotId == _activeSlot)
                return;

            _weaponSlots.ForEach(x => x.gameObject.SetActive(false));
            _weaponSlots[slotId].gameObject.SetActive(true);
            var weapGO = _weaponSlots[slotId];

            //_semiAutoWeapon = weapGO as ISemiAutoWeapon;
            //_fullAutoWeapon = weapGO as IFullAutoWeapon;

            // visually better NULLs
            _semiAutoWeapon = weapGO is ISemiAutoWeapon semi ? semi : null;
            _fullAutoWeapon = weapGO is IFullAutoWeapon fullAuto ? fullAuto : null;

            _activeSlot = slotId;
        }

        public void HandleFireInput(InputAction.CallbackContext context)
        {
            bool pressed = context.ReadValueAsButton();

            if (pressed)
            {
                _semiAutoWeapon?.TryPunch();
                _fullAutoWeapon?.TryStartFiring();
            }
            else
            {
                _fullAutoWeapon?.TryStopFiring();
            }
        }

        public void OnSlot1Performed(InputAction.CallbackContext _) => ChangeWeapon(0);
        public void OnSlot2Performed(InputAction.CallbackContext _) => ChangeWeapon(1);
        public void OnSlot3Performed(InputAction.CallbackContext _) => ChangeWeapon(2);
    }
}