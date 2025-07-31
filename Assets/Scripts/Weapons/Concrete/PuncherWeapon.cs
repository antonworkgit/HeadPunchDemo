using Scripts.Animations.Puncher;
using UnityEngine;

namespace Scripts.Weapons.Concrete
{
    public sealed class PuncherWeapon : BaseSimpleWeapon, IFullAutoWeapon
    {
        [SerializeField]
        private PuncherAnimator _animator;

        private float _lastShootTime;
        private bool _fireToggled;

        private void OnEnable()
        {
            _animator.OnAnimationHitEvent += OnAnimationHit;
        }

        private void OnDisable()
        {
            _animator.OnAnimationHitEvent -= OnAnimationHit;
            _fireToggled = false;
        }

        private void Update()
        {
            if (_fireToggled)
                TryPunch();
        }

        public void TryStartFiring() => _fireToggled = true;
        public void TryStopFiring() => _fireToggled = false;

        private void TryPunch()
        {
            if (Time.time <= _lastShootTime + weaponData.Cooldown)
                return;

            if (_animator.IsFiring)
                return;

            _lastShootTime = Time.time;
            _animator.PlayFire();
        }
    }
}