using Scripts.Interactive.Punching;
using Scripts.Animations;
using Scripts.Animations.Bat;
using UnityEngine;
using Scripts.Interactive.Painting;

namespace Scripts.Weapons.Concrete
{
    public sealed class BatWeapon : BaseSimpleWeapon, ISemiAutoWeapon, IAnimationHitListener
    {
        private enum AttackDirection
        {
            None = 0,
            Left,
            Right,
        }

        [SerializeField]
        private BatAnimator _animator;

        [SerializeField]
        private Transform _leftAttackOrigin;

        [SerializeField]
        private Transform _rightAttackOrigin;

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

        protected sealed override void HandlePunch(Ray screenRay)
        {
            //1) Raycast from L/R
            //2) If raycast lr -> punch from them
            //3) If no LR -> raycast front;

            Vector3 attackOrigin = _lastAttackDirection == AttackDirection.Left ? _leftAttackOrigin.position : _rightAttackOrigin.position;

            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponData.AttackRange, Punchable) && hit.rigidbody != null && hit.collider.TryGetComponent<IPunchable>(out IPunchable punchable))
            {
                Vector3 hitDirection = screenRay.direction;
                Vector3 hitOrigin = hit.point;
                float forcePower = Random.Range(weaponData.KnockbackRange.x, weaponData.KnockbackRange.y);
                float damage = weaponData.Damage;

                Vector3 sideDirection = (hit.rigidbody.position - attackOrigin).normalized;
                Ray sideRay = new Ray(attackOrigin, sideDirection);

                if (Physics.Raycast(sideRay, out RaycastHit sideHit, float.MaxValue, Punchable) && sideHit.rigidbody != null && sideHit.rigidbody == hit.rigidbody)
                {
                    hitDirection = sideRay.direction;
                    hitOrigin = sideHit.point;
                }

                Vector3 force = hitDirection * forcePower;
                punchable.ReceivePunch(new PunchData() { Damage = weaponData.Damage, KnockbackForce = force, HitPoint = hitOrigin, HitObject = hit.rigidbody.gameObject });
            }
        }

        protected override void HandlePaint(Ray screenRay)
        {
            Vector3 attackOrigin = _lastAttackDirection == AttackDirection.Left ? _leftAttackOrigin.position : _rightAttackOrigin.position;

            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponData.AttackRange, Paintable) && hit.rigidbody != null && hit.collider.TryGetComponent<IPaintable>(out IPaintable paintable))
            {
                Vector3 sideDirection = (hit.rigidbody.position - attackOrigin).normalized;
                Ray sideRay = new Ray(attackOrigin, sideDirection);

                if (Physics.Raycast(sideRay, out RaycastHit sideHit, float.MaxValue, Paintable) && sideHit.rigidbody != null && sideHit.rigidbody == hit.rigidbody)
                {
                    paintable.Paint(sideHit.point, weaponData.PaintParams);
                }
                else
                {
                    paintable.Paint(hit.point, weaponData.PaintParams);
                }
            }
        }
    }
}