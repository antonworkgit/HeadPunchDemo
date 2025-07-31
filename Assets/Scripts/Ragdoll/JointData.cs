using System;
using UnityEngine;

namespace Scripts.Ragdoll
{
    [Serializable]
    public class JointData
    {
        public Vector2 rotationLimitRange = new Vector2(45f, 25f);
        public Vector2 springRange = new Vector2(0.0f, 100f);
        public Vector2 damperRange = new Vector2(0.0f, 10f);
        public Vector2 maximumForce = new Vector2(0.025f, 5f);

        public void GetInterpolated(float t, float mass, out float rotationLimit, out float spring, out float damper, out float maxForce)
        {
            rotationLimit = Mathf.Lerp(rotationLimitRange.x, rotationLimitRange.y, t);

            // forces multiplied by mass
            spring = Mathf.Lerp(springRange.x, springRange.y, t) * mass;
            damper = Mathf.Lerp(damperRange.x, damperRange.y, t) * mass;
            maxForce = Mathf.Lerp(maximumForce.x, maximumForce.y, t) * mass;
        }

        //public void GetMax(out float rotationLimit, out float spring, out float damper, out float maxForce) 
        //    => GetInterpolated(1.0f, out rotationLimit, out spring, out damper, out maxForce);
    }
}