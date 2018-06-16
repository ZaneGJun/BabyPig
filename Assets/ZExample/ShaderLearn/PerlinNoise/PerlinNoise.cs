#define TEST 

using System;
using System.Collections;
using UnityEngine;
using Pld;

namespace Pld {
    //[ExecuteInEditMode]
    public class PerlinNoise : MonoBehaviour {

        public static int[] permutation = {
            151,160,137,91,90,15, 131,13,201,95,96,53,194,233,7,225,140,
            36,103,30,69,142,8,99,37,240,21,10,23, 190, 6,148,247,120,234,
            75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33, 88,237,149,
            56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,
            245,40,244, 102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,
            208, 89,18,169,200,196, 135,130,116,188,159,86,164,100,109,198,173,
            186, 3,64,52,217,226,250,124,123, 5,202,38,147,118,126,255,82,85,212,
            207,206,59,227,47,16,58,17,182,189,28,42, 223,183,170,213,119,248,152,
            2,44,154,163, 70,221,153,101,155,167, 43,172,9, 129,22,39,253, 19,98,108,
            110,79,113,224,232,178,185, 112,104,218,246,97,228, 251,34,242,193,238,210,
            144,12,191,179,162,241, 81,51,145,235,249,14,239,107, 49,192,214, 31,181,199,
            106,157,184, 84,204,176,115,121,50,45,127, 4,150,254, 138,236,205,93,222,114,
            67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };
        Vector2[] gradient_2D = new Vector2[256];

        public Transform target;

        public ArrayList points = new ArrayList();

        // Use this for initialization
        void Start() {

            initData();

            Debug.DrawLine(new Vector2(0, 0), new Vector2(20, 0), Color.black);

            for(float i = 0;i<8.0f;i = i+0.1f)
            {
                double val = perlin(1.5f, i);
                //Debug.Log(String.Format("{0} {1}",i,val));
            }

            drawTexture(300, 300);
        }

        // Update is called once per frame
        void Update() {
            //drawSimpleNoise();
        }

        void initData()
        {
            for (int i = 0; i < gradient_2D.Length; i++)
            {
                gradient_2D[i] = PMath.HashOld22(new Vector2(i, gradient_2D.Length + i));
                gradient_2D[i].Normalize();
            }
        }

        void drawTexture(int width, int height)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            for (float i = 0; i < height; i++)
                for (float j = 0; j < width; j++)
                {
                    float xCoord = j / width;
                    float yCoord = i / height;
                    Vector2 p = new Vector2(xCoord, yCoord);

                    //Vector2 move = (new Vector2(1, 1) + PMath.Hash21(888)) * 100;
                    //p = (p + move) * 8;

                    double perLinNoiseVal = Mathf.Abs((float)perlin(p)) + 0.5f * Mathf.Abs((float)perlin(p * 2.0f)) + 0.25f * Mathf.Abs((float)perlin(p * 4.0f)) + 0.125f * Mathf.Abs((float)perlin(p * 8.0f));

                    //perLinNoiseVal = Mathf.Sin(p.x + (float)perLinNoiseVal);

                    Color col = new Color((float)perLinNoiseVal, (float)perLinNoiseVal, (float)perLinNoiseVal);
                    tex.SetPixel((int)(j), (int)(i), col);
                }
       
            tex.Apply();

            Utils.SaveBytesToLocal(tex.EncodeToPNG(), "D://perlinNoise.png");
        }

        private void drawLine(Vector2 start, Vector2 end)
        {
            if (start.x != end.x)
                Debug.DrawLine(start, end, Color.red);
        }

        private void drawSimpleNoise()
        {
            ArrayList points = new ArrayList();
            points.Add(new Vector2(0, (float)perlin(0.0f, 1.0f)));

            for (float i = 0; i < 10.0f; i = i + 0.05f)
            {
                Vector2 tmp = new Vector2(i, 1.0f);
                double noiseValue = perlin(tmp) + 0.5f * perlin(tmp * 2) + 0.25f * perlin(tmp * 4);
                Debug.Log("noiseValue:" + noiseValue);
                Vector2 newPoint = new Vector2(i, (float)noiseValue);

                Vector2 lastPoint = (Vector2)points[points.Count - 1];
                drawLine(lastPoint, newPoint);

                points.Add(newPoint);
            }
        }

        public int Perm(Vector2 p)
        {
            int p1 = permutation[((int)p.x) % 256];
            return p1 + (int)p.y;
        }

        public float Gradient_2D(int x, Vector2 y)
        {
            return Vector2.Dot(gradient_2D[x % gradient_2D.Length], y);
        }

        private double perlin(Vector2 p)
        {
            return perlin(p.x, p.y);
        }

        private double perlin(float x, float y)
        {
            float x0 = Mathf.Floor(x);
            float x1 = x0 + 1;
            float y0 = Mathf.Floor(y);
            float y1 = y0 + 1;

            //指向中点
            Vector2 lt = new Vector2(x - x0, y - y1);
            Vector2 ld = new Vector2(x - x0, y - y0);
            Vector2 rt = new Vector2(x - x1, y - y1);
            Vector2 rd = new Vector2(x - x1, y - y0);

#if TEST
            Vector2 ltGradient = new Vector2(Noise.NoiseRandom(x0), Noise.NoiseRandom(y1));
            Vector2 ldGradient = new Vector2(Noise.NoiseRandom(x0), Noise.NoiseRandom(y0));
            Vector2 rtGradient = new Vector2(Noise.NoiseRandom(x1), Noise.NoiseRandom(y1));
            Vector2 rdGradient = new Vector2(Noise.NoiseRandom(x1), Noise.NoiseRandom(y0));

            ltGradient.Normalize();
            ldGradient.Normalize();
            rtGradient.Normalize();
            rdGradient.Normalize();

            float ltFactor = Vector2.Dot(ltGradient, lt);
            float ldFactor = Vector2.Dot(ldGradient, ld);
            float rtFactor = Vector2.Dot(rtGradient, rt);
            float rdFactor = Vector2.Dot(rdGradient, rd);
#else

            float ltFactor = Vector2.Dot(gradient_2D[Perm(new Vector2(x0, y1)) % gradient_2D.Length], lt); //Gradient_2D(Perm(new Vector2(x0, y1)), lt);
            float ldFactor = Vector2.Dot(gradient_2D[Perm(new Vector2(x0, y0)) % gradient_2D.Length], ld); // Gradient_2D(Perm(new Vector2(x0, y0)), ld); 
            float rtFactor = Vector2.Dot(gradient_2D[Perm(new Vector2(x1, y1)) % gradient_2D.Length], rt); //Gradient_2D(Perm(new Vector2(x1, y1)), rt);  
            float rdFactor = Vector2.Dot(gradient_2D[Perm(new Vector2(x1, y0)) % gradient_2D.Length], rd); //Gradient_2D(Perm(new Vector2(x1, y0)), rd); 
#endif

            double u = Noise.Fade(x - x0);
            double v = Noise.Fade(y - y0);

            float x11 = Mathf.Lerp(ltFactor, rtFactor, (float)u);
            float x22 = Mathf.Lerp(ldFactor, rdFactor, (float)u);
            float average = Mathf.Lerp(x11, x22, 1.0f - (float)v);  //y方向
      
            return average;
        }
    }
}