using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PerlinNoise : MonoBehaviour
{
public int width = 256;
public int height = 256;
public float scale = 20f;
public float offsetX = 100f;
public float offsetY = 100f;

private Renderer _renderer;

private void Start() {
    _renderer = GetComponent<Renderer>();
    
    offsetX = Random.Range(0f, 99999f);
    offsetY = Random.Range(0f, 99999f);
}

void Update() {
    _renderer.material.mainTexture = GenerateTexture();
}

Texture2D GenerateTexture() {
    Texture2D texture = new Texture2D(width, height);

    for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
            Color color = CalculateColor(x, y);
            texture.SetPixel(x, y, color);
        }
    }
    texture.Apply();
    return texture;
}

Color CalculateColor(int x, int y) {
    float xCoord = (float)x / width * scale + offsetX;
    float yCoord = (float)y / height * scale + offsetY;
    
    float sample = Mathf.PerlinNoise(xCoord, yCoord);
    return new Color(sample, sample, sample);
}
}
