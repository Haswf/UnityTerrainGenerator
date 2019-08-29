using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public int mSize = 5;

    //offset of the height.
    public float seed = 0;
    public float mHeight = 0;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        DiamondSquare();
        UpdateMesh();

    }
    
    void CreateShape()
    {
        // how many vertices
        vertices = new Vector3[(mSize + 1) * (mSize + 1)];

        int i = 0;
        for (int z = 0; z <= mSize; z++)
        {
            for (int x = 0; x <= mSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        triangles = new int[mSize * mSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < mSize; z++)
        {
            for (int x = 0; x < mSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + mSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + mSize + 1;
                triangles[tris + 5] = vert + mSize + 2;
                
                vert++;
                tris += 6;
            }

            vert++;
        }


        //vertices[0].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[mSize].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[vertices.Length - 1].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[vertices.Length - 1 - mSize].y = UnityEngine.Random.Range(-mHeight, mHeight);

        //initial the height values for four vertexes of the origin square.
        vertices[0].y = seed;
        vertices[mSize].y = seed;
        vertices[vertices.Length - 1].y = seed;
        vertices[vertices.Length - 1 - mSize].y = seed;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }



    //Damiond square algorithm
    void DiamondSquare()
    {
        float average;

        int DATA_SIZE = mSize + 2;
        // 2d arrays contains height of each vertex
        float[,] heights = new float[DATA_SIZE, DATA_SIZE];

        // declare initial height of 
        heights[0,DATA_SIZE - 1] = seed;
        heights[0, 0] = seed;
        heights[DATA_SIZE - 1, 0] = seed;
        heights[DATA_SIZE - 1, DATA_SIZE - 1] = seed;

        for (int sideLength = DATA_SIZE-1; sideLength >= 2; sideLength /= 2)
        {
            int halfSize = sideLength / 2;
            // x and y stand for coordinates of each vertex in 2d array 
            for (int x = 0; x < DATA_SIZE- 1; x += sideLength)
            {
                for (int y = 0; y < DATA_SIZE - 1; y += sideLength)
                {
                    average = heights[x, y];
                    average += heights[x + sideLength, y];
                    average += heights[x, y + sideLength];
                    average += heights[x + sideLength, y + sideLength];
                    average /= 4.0f;

                    average += UnityEngine.Random.Range(0, mHeight);
                    heights[x + halfSize, y + halfSize] = average;
                }
            }

            for (int x = 0; x < DATA_SIZE - 1; x += halfSize)
            {
                for (int y = (x + halfSize) % sideLength; y < DATA_SIZE - 1; y += sideLength)
                {
                    float avg = heights[(x - halfSize + DATA_SIZE - 1) % (DATA_SIZE - 1), y] +
                        heights[(x + halfSize) % (DATA_SIZE - 1), y] +
                        heights[x, (y + halfSize) % (DATA_SIZE - 1)] +
                        heights[x, (y - halfSize + DATA_SIZE - 1) % (DATA_SIZE - 1)];

                    avg /= 4.0f;
                    avg = avg + UnityEngine.Random.Range(0, mHeight);

                    heights[x, y] = avg;

                    if (x == 0) heights[DATA_SIZE - 1, y] = avg;
                    if (y == 0) heights[x, DATA_SIZE - 1] = avg;

                }
            }
        }
        for (int x = 0; x < DATA_SIZE - 1; x++)
        {
            for (int y = 0; y < DATA_SIZE - 1; y++)
            {
                //print(x);
                //print(y);
                //print(heights[x, y]);
                vertices[x + y * (mSize + 1)].y = heights[x, y];

            }

        }
    }
}
