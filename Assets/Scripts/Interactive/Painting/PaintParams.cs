using System;
using UnityEngine;

namespace Scripts.Interactive.Painting
{
    [Serializable]
    public struct PaintParams
    {
        public Color inkColor;
        public float strength;
        public float hardness;
        public float radius;

        public static readonly PaintParams Default = new()
        {
            inkColor = Color.magenta,
            radius = 0.15f,
            hardness = 0.5f,
            strength = 0.5f
        };

        public static PaintParams FromColor(Color color) => new()
        {
            inkColor = color,
            radius = 0.15f,
            hardness = 0.5f,
            strength = 0.5f
        };
    }
}