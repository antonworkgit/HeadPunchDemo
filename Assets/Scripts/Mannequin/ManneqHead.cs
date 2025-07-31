using Scripts.Ragdoll;
using UnityEngine;

namespace Scripts.Mannequin
{
    public sealed class ManneqHead : ManneqBodyPart
    {
        [SerializeField]
        private HeadJointController _jointController;

        // Make head less stable
        protected override void OnEnduranceChanged(float enduranceNormalized)
        {
            _jointController.SetupJoint(EnduranceNormalized);
        }
    }
}