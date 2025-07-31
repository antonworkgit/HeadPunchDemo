using UnityEngine;

namespace Scripts.Ragdoll
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class HeadJointController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _torsoRB;

        [Header("Anchor Settings")]
        [SerializeField]
        private Vector3 _anchor = new Vector3(0f, -0.2f, 0f);
        [SerializeField]
        private Vector3 _connectedAnchor = new Vector3(0f, 0.2f, 0f);
        
        [Header("Joint Limits")]
        [SerializeField]
        private JointData _jointData;

        private Rigidbody _headRB;
        private ConfigurableJoint _joint;

        private SoftJointLimit _hightJointLimit;
        private SoftJointLimit _lowJointLimit;
        private JointDrive _jointDrive;

        private void Start()
        {
            _headRB = GetComponent<Rigidbody>();
            CreateJoint();
            SetupJointMax();
        }

        private void CreateJoint()
        {
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;

            _joint = gameObject.AddComponent<ConfigurableJoint>();
            _joint.connectedBody = _torsoRB;

            // Locked motion
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;
            _joint.zMotion = ConfigurableJointMotion.Locked;

            // Limited rotation
            _joint.angularXMotion = ConfigurableJointMotion.Limited;
            _joint.angularYMotion = ConfigurableJointMotion.Limited;
            _joint.angularZMotion = ConfigurableJointMotion.Limited;

            // Anchors
            _joint.autoConfigureConnectedAnchor = false;
            _joint.anchor = _anchor;
            _joint.connectedAnchor = _connectedAnchor;
        }

        public void SetupJoint(float power)
        {
            Debug.Log($"Joint setup: {power}", this.gameObject);

            _jointData.GetInterpolated(power, _headRB.mass, out float rotationLimit, out float spring, out float damper, out float maxForce);

            // Angular limits
            _hightJointLimit = new SoftJointLimit { limit = rotationLimit };
            _lowJointLimit = new SoftJointLimit { limit = -rotationLimit };
            _joint.lowAngularXLimit = _lowJointLimit;
            _joint.highAngularXLimit = _hightJointLimit;
            _joint.angularYLimit = _hightJointLimit;
            _joint.angularZLimit = _hightJointLimit;

            // Return to origin pos
            _jointDrive = new JointDrive
            {
                positionSpring = spring,
                positionDamper = damper,
                maximumForce = maxForce,
            };

            _joint.angularXDrive = _jointDrive;
            _joint.angularYZDrive = _jointDrive;
        }

        private void SetupJointMax() => SetupJoint(1.0F);
    }
}