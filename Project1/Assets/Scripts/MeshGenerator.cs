using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
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
    public float smoothness;
    // 2^n. This is n.
    public int dimension;
    // Assign random height to corners?
    public bool randomCornerHeight;

    public TerrainType[] regions;
    
    // number of vertices
    private int size;
    private float minHeight;
    private float maxHeight;
    private float[,] heightMap;
    
    private Color[] colourMap;
    // Start is called before the first frame update
    void Start()
    {    
        size = (int) Mathf.Pow(2, dimension);
        // Create new mesh
        mesh = new Mesh();
        // Assign created mesh to the object attached to.
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        GenerateHeightMap();
        UpdateMaxMinHeight();
        GenerateTerrain();
        UpdateMesh();
        UpdateMeshCollider();
    }
    
    void CreateMesh()
    {
        vertices = new Vector3[size * size];
        uvs = new Vector2[size * size];
        triangles = new int[(size-1) * (size-1) * 6];
        colourMap = new Color[size * size];
        // create vertices
        int i = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        // create triangles
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

    // update newly generated mesh to mesh collider
    void UpdateMeshCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colourMap;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    public void UpdateMaxMinHeight()
    {
        // find max and min value in heightMap
        maxHeight = int.MinValue;
        minHeight = int.MaxValue;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float currentHeight = heightMap[x, y];
                if (currentHeight > maxHeight)
                {
                    maxHeight = currentHeight;
                }

                if (currentHeight < minHeight)
                {
                    minHeight = currentHeight;
                }
            }
        }
    }

    void GenerateTerrain()
    {    
        regions[0].height = minHeight + (maxHeight - minHeight) * 0.4f;
        // sand
        regions[1].height = minHeight + (maxHeight - minHeight) * 0.42f;
        // wood
        regions[2].height = minHeight + (maxHeight - minHeight) * 0.8f;
        // mountain
        regions[3].height = minHeight + (maxHeight - minHeight) * 0.85f;
        // earth
        regions[4].height = minHeight + (maxHeight - minHeight) * 0.9f;
        // snow
        regions[5].height = minHeight + (maxHeight - minHeight);
        
        // Map height to each vertex
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float currentHeight = heightMap[x, y];
                vertices[y * size + x].y = currentHeight;
                for (int i = 0; i < regions.Length; i++)
                {    
                    if (currentHeight <= regions[i].height)
                    {   
                        // Assign colour 
                        colourMap[y * size + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
    }
    // Set seed for random generator
    public void SetSeed(int seed) {
        UnityEngine.Random.InitState(seed);
    }
    
    public void GenerateHeightMap()
    {
        float range = this.range;
        // 2D array of height
        heightMap = new float[size + 1, size + 1];

        // set seed for random number generator
        SetSeed(UnityEngine.Random.Range(0, 100));
        
        // Assign random value within (-range, range) to each corner
        if (randomCornerHeight)
        {
            heightMap[0,size] = UnityEngine.Random.Range(-range, range);
            heightMap[0, 0] = UnityEngine.Random.Range(-range, range);
            heightMap[size, 0] = UnityEngine.Random.Range(-range, range);
            heightMap[size, size] = UnityEngine.Random.Range(-range, range);
        }
        // o/w assign range to each corner
        else
        {
            heightMap[0,size] = range;
            heightMap[0, 0] = range;
            heightMap[size, 0] = range;
            heightMap[size, size] = range;
        }
        
        float average;

        for (int sideLength = size; sideLength >= 2; sideLength /= 2)
        {
            int halfSize = sideLength / 2;
            // x and y stand for coordinates of each vertex in 2d array 
            for (int x = 0; x < size; x += sideLength)
            {
                for (int y = 0; y < size; y += sideLength)
                {
                    average = heightMap[x, y]; // left top corner
                    average += heightMap[x + sideLength, y]; // right top corner
                    average += heightMap[x, y + sideLength]; // left bottom corner
                    average += heightMap[x + sideLength, y + sideLength]; // left bottom corner
                    
                    // calc average
                    average /= 4.0f;
                    // offset height by a random value
                    average += UnityEngine.Random.value * (range * 2.0f) - range;
                    // Assign new height to mid point
                    heightMap[x + halfSize, y + halfSize] = average;
                }
            }

            for (int x = 0; x < size; x += halfSize)
            {
                for (int y = (x + halfSize) % sideLength; y < size; y += sideLength)
                {
                    average = heightMap[(x - halfSize + size) % size, y];
                    average += heightMap[(x + halfSize) % size, y];
                    average += heightMap[x, (y + halfSize) % size];
                    average += heightMap[x, (y - halfSize + size) % size];
                    
                    // calculate average
                    average /= 4.0f;
                    // offset height by a random value
                    average += UnityEngine.Random.value * (range * 2.0f) - range;

                    heightMap[x, y] = average;

                    if (x == 0)
                    {
                        heightMap[size, y] = average;
                    }

                    if (y == 0)
                    {
                        heightMap[x, size] = average;
                    }
                }
            }
            // Lower the random value range
            range -= range * 0.5f * smoothness;
        }
    }
    
}
// structure to define terrain colour
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}