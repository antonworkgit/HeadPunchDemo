using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Helpers.ColorMixing
{
    public static class ColorMixer
    {
        public static Color GetBlendedColor(IEnumerable<Color> colors)
        {
            if (colors == null || !colors.Any())
                return Color.clear;

            float averageR = colors.Average(c => c.r);
            float averageG = colors.Average(c => c.g);
            float averageB = colors.Average(c => c.b);

            return new Color(averageR, averageG, averageB);
        }
    }
}