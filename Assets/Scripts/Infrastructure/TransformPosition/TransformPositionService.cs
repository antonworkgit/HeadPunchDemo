using UnityEngine;

namespace Scripts.Infrastructure.TransformPosition
{
    public sealed class TransformPositionService : ITransformPositionService
    {
        private readonly Transform _tf;
        public Vector3 TransformPosition => _tf.position;
        public TransformPositionService(Transform tf) => _tf = tf;
    }
}