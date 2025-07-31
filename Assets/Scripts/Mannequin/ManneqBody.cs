using UnityEngine;

namespace Scripts.Mannequin
{
    public sealed class ManneqBody : ManneqBodyPart
    {
        [SerializeField]
        private Vector2 _bodyAngleRange = new Vector2(10f, 0f);

        // Rotate body
        protected override void OnEnduranceChanged(float enduranceNormalized)
        {
            base.OnEnduranceChanged(enduranceNormalized);

            Transform rbTF = Rigidbody.transform;
            rbTF.localRotation = Quaternion.Euler(
                Mathf.Lerp(_bodyAngleRange.x, _bodyAngleRange.y, enduranceNormalized),
                rbTF.localRotation.eulerAngles.y,
                rbTF.localRotation.eulerAngles.z
            );
        }
    }
}