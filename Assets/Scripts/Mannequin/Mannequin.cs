using Scripts.Interactive.Punching;
using Scripts.Cinematic;
using System;
using UnityEngine;
using Scripts.Interactive.Painting;
using Scripts.Interactive.Debrisifying;

namespace Scripts.Mannequin
{
    public sealed class Mannequin : MonoBehaviour
    {
        [SerializeField]
        private float MaxHealth;

        [Header("Body Parts")]
        [SerializeField]
        private ManneqHead _head;

        [SerializeField]
        private ManneqBody _body;

        [SerializeField]
        private Paintable _headVisual;

        [SerializeField]
        private Paintable _bodyVisual;

        private float _health;

        public float HealthNormalized => Mathf.Clamp01(_health / MaxHealth);
        public float HeadEnduranceNormalized => _head.EnduranceNormalized;
        public float BodyEnduranceNormalized => _body.EnduranceNormalized;

        public event Action OnDamageReceived;

        private void Awake()
        {
            _health = MaxHealth;
        }

        private void OnEnable()
        {
            _head.OnDamaged += HandleDamage;
            _body.OnDamaged += HandleDamage;
        }

        private void OnDisable()
        {
            _head.OnDamaged -= HandleDamage;
            _body.OnDamaged -= HandleDamage;
        }

        private void HandleDamage(PunchData punchData)
        {
            _health -= punchData.Damage;

            OnDamageReceived?.Invoke();

            if (_health <= 0)
            {
                Die(punchData);
            }
        }

        public void Die(PunchData punchData)
        {
            if (_headVisual.TryGetComponent<Debrisable>(out Debrisable headDeb))
            {
                // If the final punch was at head punch it and make it bleed
                bool headCrit = punchData.HitObject == _head.gameObject;

                if (headCrit)
                {
                    PaintParams trailsParams = new PaintParams()
                    {
                        inkColor = _headVisual.GetMostColor(),
                        radius = 0.15f,
                        hardness = 0.45f,
                        strength = 0.45f,
                    };

                    headDeb.BecomeDebrisWithKnockbackAndCollisionPainting(punchData, trailsParams);
                    KillCamSampleService.PlayKillCam(_headVisual.gameObject);
                }
                // Otherwise just fall
                else
                {
                    headDeb.BecomeDebris();
                }
            }

            // The body always falls and becomes debris
            if (_bodyVisual.TryGetComponent<Debrisable>(out Debrisable bodyDeb))
                bodyDeb.BecomeDebris();

            Destroy(gameObject);
        }
    }
}