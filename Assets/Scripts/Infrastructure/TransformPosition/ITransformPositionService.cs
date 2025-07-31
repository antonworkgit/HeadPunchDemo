using UnityEngine;

namespace Scripts.Infrastructure.TransformPosition
{
    public interface ITransformPositionService
    {
        public Vector3 TransformPosition { get; }
    }
}