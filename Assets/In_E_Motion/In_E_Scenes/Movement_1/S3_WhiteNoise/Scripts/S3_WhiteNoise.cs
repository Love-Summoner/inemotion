using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public Renderer targetRenderer;
    public float refreshRate = 0.05f; // Time in seconds between texture updates

    private Texture2D noiseTexture;
    private Coroutine noiseCoroutine;

    void Start()
    {
        noiseTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        targetRenderer.material.mainTexture = noiseTexture;
        StartNoise();
    }

    void OnEnable()
    {
        StartNoise();
    }

    void OnDisable()
    {
        StopNoise();
    }

    void StartNoise()
    {
        if (noiseCoroutine == null)
        {
            noiseCoroutine = StartCoroutine(UpdateNoiseTexture());
        }
    }

    void StopNoise()
    {
        if (noiseCoroutine != null)
        {
            StopCoroutine(noiseCoroutine);
            noiseCoroutine = null;
        }
    }

    IEnumerator UpdateNoiseTexture()
    {
        while (true)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                for (int x = 0; x < noiseTexture.width; x++)
                {
                    Color color = Color.white * Random.Range(0f, 1f);
                    noiseTexture.SetPixel(x, y, color);
                }
            }
            noiseTexture.Apply();
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
