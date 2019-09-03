using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    // vector3 array of vertices
    private Vector3[] vertices;
    private Vector2[] uvs;
    
    private int[] triangles;
    
    // range of height for generating height offset
    public float range;
    
    // smoothness of the terrain. 
    // 0 yields crystal world.
    public float smoothness;

    // 2^n - 1. This is n.
    public int dimension;
    
    // Assign random height to corners?
    public bool randomCornerHeight;

    public Renderer textureRender;    
    public TerrainType[] regions;

    // 2^n - 1
    // number of vertices
    private int size;
    
    // Start is called before the first frame update
    void Start()
    {    
        // Create Mesh of size `size`
        mesh = new Mesh();
        // Assign created mesh to the object attached to.
        GetComponent<MeshFilter>().mesh = mesh;
        size = (int) Mathf.Pow(2, dimension);
        CreateShape();
        DiamondSquare();
        UpdateMesh();
    }
    
    void CreateShape()
    {
        // how many vertices
        vertices = new Vector3[size * size];
        uvs = new Vector2[size * size];

        int i = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        triangles = new int[(size-1) * (size-1) * 6];

        int vert = 0;
        int tris = 0;
        for (int y = 0; y < size-1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size;
                triangles[tris + 5] = vert + size + 1;
                uvs[vert] = new Vector2(x/(float)size, y/(float)size);
                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }
    
    public void SetSeed(int seed) {
        UnityEngine.Random.InitState(seed);
    }
    
    //Damiond square algorithm
    void DiamondSquare()
    {
        float average;
        Color[] colourMap = new Color[(size) * (size)];
        // 2D array of height
        float[,] heights = new float[size + 1, size + 1];

        // set seed for random number generator
        SetSeed((int)UnityEngine.Random.Range(0, 100));
        
        // Assign random value within (-range, range) to each corner
        if (randomCornerHeight)
        {
            heights[0,size] = UnityEngine.Random.Range(-range, range);
            heights[0, 0] = UnityEngine.Random.Range(-range, range);
            heights[size, 0] = UnityEngine.Random.Range(-range, range);
            heights[size, size] = UnityEngine.Random.Range(-range, range);
        }
        // o/w assign range to each corner
        else
        {
            heights[0,size] = range;
            heights[0, 0] = range;
            heights[size, 0] = range;
            heights[size, size] = range;
        }
        
        
        for (int sideLength = size; sideLength >= 2; sideLength /= 2)
        {
            int halfSize = sideLength / 2;
            // x and y stand for coordinates of each vertex in 2d array 
            for (int x = 0; x < size; x += sideLength)
            {
                for (int y = 0; y < size; y += sideLength)
                {
                    average = heights[x, y]; // left top corner
                    average += heights[x + sideLength, y]; // right top corner
                    average += heights[x, y + sideLength]; // left bottom corner
                    average += heights[x + sideLength, y + sideLength]; // left bottom corner
                    
                    // calc average
                    average /= 4.0f;
                    // offset height by a random value
                    average += UnityEngine.Random.value * (range * 2.0f) - range;
                    // Assign new height to mid point
                    heights[x + halfSize, y + halfSize] = average;
                }
            }

            for (int x = 0; x < size; x += halfSize)
            {
                for (int y = (x + halfSize) % sideLength; y < size; y += sideLength)
                {
                    average = heights[(x - halfSize + size) % (size), y];
                    average += heights[(x + halfSize) % (size), y];
                    average += heights[x, (y + halfSize) % (size)];
                    average += heights[x, (y - halfSize + size) % (size)];
                    
                    // calc average
                    average /= 4.0f;
                    // offset height by a random value
                    average += UnityEngine.Random.value * (range * 2.0f) - range;

                    heights[x, y] = average;

                    if (x == 0)
                    {
                        heights[size, y] = average;
                    }

                    if (y == 0)
                    {
                        heights[x, size] = average;
                    }
                }
            }
            // Lower the random value range
            range -= range * 0.5f * smoothness;
        }
        
        // Map height to each vertex
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float currentHeight = heights[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {   
                        colourMap[y * (size) + x] = regions[i].colour;
                        break;
                    }
                }

                vertices[y * (size) + x].y = currentHeight;
            }

        }
        Texture2D texture = TextureFromColourMap(colourMap, size, size);
        textureRender.sharedMaterial.mainTexture = texture;
    }
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}