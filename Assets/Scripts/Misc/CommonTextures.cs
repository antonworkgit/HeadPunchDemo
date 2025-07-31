using UnityEngine;

namespace Scripts.Helpers.Rendering
{
    public static class CommonTextures
    {
        private static Texture2D _whitePixel;

        public static Texture2D WhitePixel
        {
            get
            {
                if (_whitePixel == null)
                {
                    Warmup();
                }
                return _whitePixel;
            }
        }

        private static void Warmup()
        {
            _whitePixel = new Texture2D(1, 1);
            _whitePixel.SetPixel(0, 0, Color.white);
            _whitePixel.Apply();
        }

        // public static void Dispose();
    }
}
