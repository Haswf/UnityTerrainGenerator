using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    // vector3 array of vertices
    private Vector3[] vertices;
    
    private int[] triangles;
    
    // range of height for generating height offset
    public float range;
    
    // smotthness of the terrain. 
    // 0 yields crystal world.
    public float smoothness;
    
    // 2^n - 1. This is n.
    public int dimension;
    
    // Assign random height to corners?
    public bool randomCornerHeight;
    
    // 2^n - 1
    private int size;

    // Start is called before the first frame update
    void Start()
    {    
        // Create Mesh of size `size`
        mesh = new Mesh();
        // Assign created mesh to the object attached to.
        GetComponent<MeshFilter>().mesh = mesh;
        size = (int) Mathf.Pow(2, dimension) - 1;
        CreateShape();
        DiamondSquare();
        UpdateMesh();
    }
    
    void CreateShape()
    {
        // how many vertices
        vertices = new Vector3[(size + 1) * (size + 1)];

        int i = 0;
        for (int z = 0; z <= size; z++)
        {
            for (int x = 0; x <= size; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        triangles = new int[size * size * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;
                
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
        print(mesh.bounds.size);

    }
    
    public void SetSeed(int seed) {
        UnityEngine.Random.InitState(seed);
    }
    
    //Damiond square algorithm
    void DiamondSquare()
    {
        float average;

        // 2D array of height
        float[,] heights = new float[size + 2, size + 2];

        // set seed for random number generator
        SetSeed((int)UnityEngine.Random.Range(0, 100));
        
        // Assign random value within (-range, range) to each corner
        if (randomCornerHeight)
        {
            heights[0,size+1] = UnityEngine.Random.Range(-range, range);
            heights[0, 0] = UnityEngine.Random.Range(-range, range);
            heights[size+1, 0] = UnityEngine.Random.Range(-range, range);
            heights[size+1, size+1] = UnityEngine.Random.Range(-range, range);
        }
        // o/w assign range to each corner
        else
        {
            heights[0,size+1] = range;
            heights[0, 0] = range;
            heights[size+1, 0] = range;
            heights[size+1, size+1] = range;
        }
        
        
        for (int sideLength = size + 1; sideLength >= 2; sideLength /= 2)
        {
            int halfSize = sideLength / 2;
            // x and y stand for coordinates of each vertex in 2d array 
            for (int x = 0; x < size + 1; x += sideLength)
            {
                for (int y = 0; y < size+1; y += sideLength)
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

            for (int x = 0; x < size+1; x += halfSize)
            {
                for (int y = (x + halfSize) % sideLength; y < size+1; y += sideLength)
                {
                    average = heights[(x - halfSize + size+1) % (size+1), y];
                    average += heights[(x + halfSize) % (size+1), y];
                    average += heights[x, (y + halfSize) % (size+1)];
                    average += heights[x, (y - halfSize + size+1) % (size+1)];
                    
                    // calc average
                    average /= 4.0f;
                    // offset height by a random value
                    average += UnityEngine.Random.value * (range * 2.0f) - range;

                    heights[x, y] = average;

                    if (x == 0)
                    {
                        heights[size+1, y] = average;
                    }

                    if (y == 0)
                    {
                        heights[x, size+1] = average;
                    }
                }
            }
            // Lower the random value range
            range -= range * 0.5f * smoothness;
        }
        
        // Map height to each vertex
        for (int x = 0; x < size+1; x++)
        {
            for (int y = 0; y < size+1; y++)
            {
                vertices[x + y * (size + 1)].y = heights[x, y];
            }

        }
    }
}
