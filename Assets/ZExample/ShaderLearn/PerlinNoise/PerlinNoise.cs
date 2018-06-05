using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

[ExecuteInEditMode]
public class PerlinNoise : MonoBehaviour {

    public Transform target;

    public ArrayList points = new ArrayList();

	// Use this for initialization
	void Start () {

        Debug.DrawLine(new Vector2(0, 0), new Vector2(20, 0), Color.black);

        drawTexture(10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        drawSimpleNoise();
    }

    void drawTexture(float width, float height)
    {
        float fineness = 100.0f;
        float interval = 1.0f / fineness;

        Texture2D tex = new Texture2D((int)(width * fineness), (int)(height * fineness), TextureFormat.RGBA32, false);
        for(float i=0; i < height; i = i + interval)
            for(float j = 0; j < width; j = j + interval)
            {
                Vector2 tmpX = new Vector2(j, 1.0f);
                Vector2 tmpY = new Vector2(i, 1.0f);
                double perlinNoiseX = perlin(tmpX) + 0.5 * perlin(tmpX * 2) + 0.85 * perlin(tmpX * 4) + 0.125 * perlin(tmpX * 8);
                double perlinNoiseY = perlin(tmpY) + 0.5 * perlin(tmpY * 2) + 0.85 * perlin(tmpY * 4) + 0.125 * perlin(tmpY * 8);
                double perlinNoise = (perlinNoiseX + perlinNoiseY) / 2.0f;

                perlinNoise = (perlinNoise + 1.0f) / 2.0f;

                Color col = new Color((float)perlinNoise, (float)perlinNoise, (float)perlinNoise);
                tex.SetPixel((int)(j * fineness), (int)(i * fineness), col);
            }

        tex.Apply();

        Utils.SaveBytesToLocal(tex.EncodeToPNG(), "D://perlinNoise.png");
    }

    private void drawLine(Vector2 start, Vector2 end)
    {
        if(start.x != end.x)
            Debug.DrawLine(start, end, Color.red);
    }

    private void drawSimpleNoise()
    {
        ArrayList points = new ArrayList();
        points.Add(new Vector2(0, (float)perlin(0.0f, 1.0f)));

        for(float i=0;i<10.0f;i=i+0.05f)
        {
            Vector2 tmp = new Vector2(i, 1.0f);
            double noiseValue = perlin(tmp) + 0.5 * perlin(tmp * 2) + 0.25 * perlin(tmp * 4);
            Debug.Log("noiseValue:" + noiseValue);
            Vector2 newPoint = new Vector2(i, (float)noiseValue);

            Vector2 lastPoint = (Vector2)points[points.Count - 1];
            drawLine(lastPoint, newPoint);

            points.Add(newPoint);
        }
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
        Vector2 center = new Vector2((x0 + x1)/2.0f,(y0+y1)/2.0f);

        //指向中点
        Vector2 lt = new Vector2(x0 - x, y1 - y);
        Vector2 ld = new Vector2(x0 - x, y0 - y);
        Vector2 rt = new Vector2(x1 - x, y1 - y);
        Vector2 rd = new Vector2(x1 - x, y0 - y);

        //随机的梯度
        Vector2 ltGradient = normalize(new Vector2(Noise.NoiseRandom(x0), Noise.NoiseRandom(y1)));
        Vector2 ldGradient = normalize(new Vector2(Noise.NoiseRandom(x0), Noise.NoiseRandom(y0)));
        Vector2 rtGradient = normalize(new Vector2(Noise.NoiseRandom(x1), Noise.NoiseRandom(y1)));
        Vector2 rdGradient = normalize(new Vector2(Noise.NoiseRandom(x1), Noise.NoiseRandom(y0)));

        float ltFactor = Vector2.Dot(ltGradient, lt);
        float ldFactor = Vector2.Dot(ldGradient, ld);
        float rtFactor = Vector2.Dot(rtGradient, rt);
        float rdFactor = Vector2.Dot(rdGradient, rd);

        double u = Noise.Fade(x - x0);
        double v = Noise.Fade(y - y0);

        float x11 = Mathf.Lerp(ltFactor, rtFactor, (float)u);
        float x22 = Mathf.Lerp(ldFactor, rdFactor, (float)u);
        float average = Mathf.Lerp(x11, x22, (float)v);
        return average;
    }

    private Vector2 normalize(Vector2 p)
    {
        Vector3 tmp = new Vector3(p.x, p.y, 0);
        tmp = Vector3.Normalize(tmp);
        return new Vector2(tmp.x, tmp.y);
    }
}
