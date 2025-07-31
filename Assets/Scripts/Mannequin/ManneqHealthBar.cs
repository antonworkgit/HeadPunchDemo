using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Mannequin
{
    public sealed class ManneqHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image _headBarFill;

        [SerializeField]
        private Image _manneqBarFill;

        [SerializeField]
        private Mannequin _mannequin;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            SetHeadBarValue(1f);
            SetManneqBarValue(1f);
        }

        private void OnEnable()
        {
            UpdateBars();
            _mannequin.OnDamageReceived += UpdateBars;
        }

        private void OnDisable()
        {
            _mannequin.OnDamageReceived -= UpdateBars;
        }

        private void LateUpdate()
        {
            Vector3 dir = _camera.transform.position - transform.position;
            dir.y = 0f;
            transform.forward = -dir.normalized;
        }

        private void UpdateBars()
        {
            SetHeadBarValue(_mannequin.HeadEnduranceNormalized);
            SetManneqBarValue(_mannequin.HealthNormalized);
        }

        private void SetValue(Image barFill, float value)
        {
            if (value < 0f || value > 1f)
                Debug.LogWarning($"Invalid bar value: {value}");

            barFill.fillAmount = Mathf.Clamp01(value);
        }

        private void SetHeadBarValue(float value) => SetValue(_headBarFill, value);
        private void SetManneqBarValue(float value) => SetValue(_manneqBarFill, value);
    }
}