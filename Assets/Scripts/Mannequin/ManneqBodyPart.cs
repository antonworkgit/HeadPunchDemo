using Scripts.Interactive.Punching;
using System;
using UnityEngine;

namespace Scripts.Mannequin
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ManneqBodyPart : MonoBehaviour, IPunchable
    {
        [SerializeField]
        private float _maxEndurance;

        public float MaxEndurance => _maxEndurance;
        public float Endurance { get; private set; }
        public float EnduranceNormalized => Mathf.Clamp01(Endurance / MaxEndurance);
        public bool Functioning { get; private set; } = true;

        public event Action<PunchData> OnDamaged;

        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Endurance = _maxEndurance;

            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void ReceivePunch(PunchData punchData)
        {
            if (Functioning)
            {
                Endurance -= punchData.Damage;

                OnEnduranceChanged(EnduranceNormalized);

                if (Endurance <= 0)
                    Functioning = false;
            }

            HandleKnockback(punchData.KnockbackForce, ForceMode.Impulse);
            OnDamaged?.Invoke(punchData);
        }

        protected virtual void HandleKnockback(Vector3 knockbackForce, ForceMode forceMode = ForceMode.Impulse)
        {
            Rigidbody.AddForce(knockbackForce, forceMode);
        }

        protected virtual void OnEnduranceChanged(float enduranceNormalized) { }
    }
}