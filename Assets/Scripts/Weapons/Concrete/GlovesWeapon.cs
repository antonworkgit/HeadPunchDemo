using Scripts.Animations;
using Scripts.Animations.Gloves;
using UnityEngine;

namespace Scripts.Weapons.Concrete
{
    public sealed class GlovesWeapon : BaseSimpleWeapon, ISemiAutoWeapon, IAnimationHitListener
    {
        private enum AttackDirection
        {
            None = 0,
            Left,
            Right,
        }

        [SerializeField]
        private GlovesAnimator _animator;

        private float _lastShootTime;
        private AttackDirection _lastAttackDirection;

        private void Start()
        {
            _lastAttackDirection = Random.value > 0.5F ? AttackDirection.Left : AttackDirection.Right;
        }

        private void OnEnable()
        {
            _animator.OnAnimationHitEvent += HandleAnimationHit;
        }

        private void OnDisable()
        {
            _animator.OnAnimationHitEvent -= HandleAnimationHit;
        }

        public void TryPunch()
        {
            if (Time.time <= _lastShootTime + weaponData.Cooldown)
                return;

            if (_animator.IsAttacking)
                return;

            _lastShootTime = Time.time;

            if (_lastAttackDirection == AttackDirection.Left)
            {
                _animator.PlayAttackR();
                _lastAttackDirection = AttackDirection.Right;
            }
            else
            {
                _animator.PlayAttackL();
                _lastAttackDirection = AttackDirection.Left;
            }
        }
    }
}