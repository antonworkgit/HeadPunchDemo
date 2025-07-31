using UnityEngine;

namespace Scripts.Interactive.Punching
{
    public struct PunchData
    {
        public float Damage;
        public Vector3 KnockbackForce;
        public Vector3 HitPoint;
        public GameObject HitObject;
    }
}
