using UnityEngine;

namespace Takechi
{
    public static class ColorUtility
    {
        /// <summary>
        /// 色差を測って0～100で返すメソッド
        /// </summary>
        public static float ColorDistance(Color color1, Color color2)
        {
            var subtractColor = XYZtoLAB(RGBtoXYZ(color1)) - XYZtoLAB(RGBtoXYZ(color2));
            return Mathf.Lerp(0, 100,
                (3f - (subtractColor.x / 95.047f + subtractColor.y / 100f + subtractColor.z / 108.883f)) / 3f);
        }
        static Vector3 RGBtoXYZ(Color color)
        {
            float r = color.r;
            float g = color.g;
            float b = color.b;
    
            if (r > 0.04045f) r = Mathf.Pow((r + 0.055f) / 1.055f, 2.4f);
            else r /= 12.92f;
            if (g > 0.04045f) g = Mathf.Pow((g + 0.055f) / 1.055f, 2.4f);
            else g /= 12.92f;
            if (b > 0.04045f) b = Mathf.Pow((b + 0.055f) / 1.055f, 2.4f);
            else b /= 12.92f;
    
            r *= 100.0f;
            g *= 100.0f;
            b *= 100.0f;
    
            return new Vector3(r * 0.4124f + g * 0.3576f + b * 0.1805f, 
                r * 0.2126f + g * 0.7152f + b * 0.0722f, 
                r * 0.0193f + g * 0.1192f + b * 0.9505f);
        }
    
        static Vector3 XYZtoLAB(Vector3 color)
        {
            color.x /= 95.047f;
            color.y /= 100.000f;
            color.z /= 108.883f;
    
            if (color.x > 0.008856f) color.x = Mathf.Pow(color.x, 1.0f / 3.0f);
            else color.x = (7.787f * color.x) + (16.0f / 116.0f);
            if (color.y > 0.008856f) color.y = Mathf.Pow(color.y, 1.0f / 3.0f);
            else color.y = (7.787f * color.y) + (16.0f / 116.0f);
            if (color.z > 0.008856f) color.z = Mathf.Pow(color.z, 1.0f / 3.0f);
            else color.z = (7.787f * color.z) + (16.0f / 116.0f);
    
            return new Vector3((116.0f * color.y) - 16.0f, 500.0f * (color.x - color.y), 200.0f * (color.y - color.z));
        }
    }
}
