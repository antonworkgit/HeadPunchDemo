using UnityEngine;

namespace Scripts.Interactive.Painting
{
    public sealed class CollisionPainter : MonoBehaviour
    {
        [SerializeField]
        private PaintParams _paintParams = PaintParams.Default;

        // Hardcode
        private const int PaintableLayer = 7;

        public void InitializePaint(PaintParams paintParams)
        {
            _paintParams = paintParams;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == PaintableLayer && collision.collider.TryGetComponent<IPaintable>(out IPaintable paintable))
            {
                paintable.Paint(collision.GetContact(0).point, _paintParams);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == PaintableLayer && collision.collider.TryGetComponent<IPaintable>(out IPaintable paintable))
            {
                paintable.Paint(collision.GetContact(0).point, _paintParams);
            }
        }
    }
}