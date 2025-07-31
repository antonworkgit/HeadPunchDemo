using Scripts.Interactive.Painting;
using Scripts.Interactive.Punching;
using UnityEngine;

namespace Scripts.Interactive.Debrisifying
{
    public sealed class Debrisable : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private MeshCollider _meshCollider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        public void BecomeDebris(float timeToAutoDestroy = 3f) 
        {
            // Todo: should spawn debris prefab with same tf.PRS and texture
            transform.parent = null;
            gameObject.layer = 0;

            _meshCollider.convex = true;

            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            _rigidbody.freezeRotation = false;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // This component can be added to Paintable to make it follow a physics-only object visually
            if (TryGetComponent<CopyTransformation>(out CopyTransformation copy))
                Destroy(copy);
            
            Destroy(gameObject, timeToAutoDestroy > 0f ? timeToAutoDestroy : 0.01f);
        }

        public void BecomeDebrisWithKnockback(PunchData punchData, float timeToAutoDestroy = 3f)
        {
            BecomeDebris(timeToAutoDestroy: timeToAutoDestroy);

            _rigidbody.AddForce(1.0f * punchData.KnockbackForce, ForceMode.Impulse);
            _rigidbody.AddForce(0.675f * 1.0f * punchData.KnockbackForce.magnitude * transform.up, ForceMode.Impulse);
        }

        public void BecomeDebrisWithKnockbackAndCollisionPainting(PunchData punchData, PaintParams paintParams, float timeToAutoDestroy = 3f)
        {
            BecomeDebrisWithKnockback(punchData, timeToAutoDestroy: timeToAutoDestroy);
            gameObject.AddComponent<CollisionPainter>().InitializePaint(paintParams);
        }
    }
}