using UnityEngine;

namespace Scripts.Interactive.Painting
{
    public interface IPaintable
    {
        void Paint(Vector3 hitPosition, PaintParams paintParams);
    }
}