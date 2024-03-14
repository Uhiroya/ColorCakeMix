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
            return Distance(RGBToLAB(color1), RGBToLAB(color2));
        }
        static float Func(float x){
            if(x>0.008856f){
                return Mathf.Pow(x,(1f/3f));
            }else {
                return (1f/3f)*(Mathf.Pow((0.008856f),2f))*x+4f/29f;
            }
        }

        public static Vector3 RGBToLAB(Color color){
            float rl = Mathf.Pow(color.r,2.2f);
            float gl = Mathf.Pow(color.g,2.2f);
            float bl = Mathf.Pow(color.b,2.2f);

            float M11=0.4124f,M12=0.3576f,M13=0.1805f;
            float M21=0.2126f,M22=0.7152f,M23=0.0722f;
            float M31=0.0193f,M32=0.1192f,M33=0.9505f;

            float i = M11+M12+M13;
            float j = M21+M22+M23;
            float k = M31+M32+M33;

            float x = M11*rl+M12*gl+M13*bl;
            float y = M21*rl+M22*gl+M23*bl;
            float z = M31*rl+M32*gl+M33*bl;

            return new Vector3(116f*Func(y/j)-16f, 500f*(Func(x/i)-Func(y/j)), 200f*(Func(y/j)-Func(z/k)));
        }

        static float Distance(Vector3 c1,Vector3 c2)
        {
            return Mathf.Sqrt(Mathf.Pow(c2.x - c1.x,2f) + 
                             Mathf.Pow(c2.y - c1.y,2f) + 
                             Mathf.Pow(c2.z - c1.z,2f));
        }
    }
}
