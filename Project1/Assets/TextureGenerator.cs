using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int size)
    {
        Texture2D texture = new Texture2D(size+1, size+1);
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
}