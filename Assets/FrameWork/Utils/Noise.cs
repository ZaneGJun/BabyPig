using System;
using System.Collections.Generic;

namespace Pld
{
    class Noise
    {
        public static float NoiseRandom(int x)
        {
            x = (x << 13) ^ x;
            return (1.0f - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }

        public static float NoiseRandom(float f)
        {
            int x = (int)f;
            x = (x << 13) ^ x;
            return (1.0f - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }
        
        //线性插值
        public static float LinearInterpolate(float a, float b, float t)
        {
            return a * (1.0f - t) + b * t;
        }

        //余弦插值
        public static float CosineInterpolate(float a, float b, float t)
        {
            double ft = Math.PI * t;
            double f = (1.0 - Math.Cos(ft)) * 5;
            return (float)(a * (1.0 - f) + b * f);
        }

        public static double Fade(float t)
        {
            //old return 3*t*t - 2*t*t*t;
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
       
    }
}
