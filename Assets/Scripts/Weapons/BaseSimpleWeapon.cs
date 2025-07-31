using Scripts.Interactive.Punching;
using Scripts.Animations;
using Scripts.Weapons.Data;
using UnityEngine;
using Scripts.Interactive.Painting;

namespace Scripts.Weapons
{
    [DisallowMultipleComponent]
    public abstract class BaseSimpleWeapon : MonoBehaviour, IAnimationHitListener
    {
        protected static LayerMask Paintable {  get; private set; }
        protected static LayerMask Punchable { get; private set; }
        protected static LayerMask Damageable { get; private set; }

        protected static Ray GetScreenRay() => Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        [SerializeField]
        protected WeaponData weaponData;

        [SerializeField]
        private Transform _playerTF;

        protected Vector3 PlayerPosition => _playerTF.position;

        private void Awake()
        {
            // Hardcode
            Paintable = 1 << LayerMask.NameToLayer("Paintable");
            Punchable = 1 << LayerMask.NameToLayer("Punchable");
            Damageable = 1 << LayerMask.NameToLayer("Damageable");
        }

        public void OnAnimationHit()
        {
            // This should be done in individual systems to observe SRP
            // However, since this is a demo, the logic is kept in a god-class for simplicity and clarity
            // Ray should go from Player or Weapon not from the Screen

            Ray screenRay = GetScreenRay();

            HandlePunch(screenRay);
            HandlePaint(screenRay);
        }

        protected virtual void HandlePunch(Ray screenRay)
        {
            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponData.AttackRange, Punchable) && hit.rigidbody != null && hit.collider.TryGetComponent<IPunchable>(out IPunchable punchable))
            {
                Vector3 force = screenRay.direction * Random.Range(weaponData.KnockbackRange.x, weaponData.KnockbackRange.y);
                punchable.ReceivePunch(new PunchData() { Damage = weaponData.Damage, KnockbackForce = force, HitPoint = hit.point, HitObject = hit.rigidbody.gameObject });
            }
        }

        protected virtual void HandlePaint(Ray screenRay)
        {
            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponData.AttackRange, Paintable) && hit.rigidbody != null && hit.collider.TryGetComponent<IPaintable>(out IPaintable paintable))
            {
                paintable.Paint(hit.point, weaponData.PaintParams);
            }
        }
    }
}