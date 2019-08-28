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
        DimondSquare();
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
    void DimondSquare(int row, int col, int size, float offset)
    {

        float average;
        int VertexPerLine = mSize + 1;
        // 2d arrays contains height of each vertex
        float[,] heights = new float[VertexPerLine, VertexPerLine];
        
        // declare initial height of 
        heights[0, VertexPerLine-1] = seed;
        heights[0, 0] = seed;
        heights[VertexPerLine-1, 0] = seed;
        heights[VertexPerLine-1, VertexPerLine-1] = seed;

        for (int sideLength = mSize; sideLength >= 2; sideLength /= 2)
        {
            int halfSize = sideLength / 2;
            // x and y stand for coordinates of each vertex in 2d array 
            for (int x = 0; x < VertexPerLine - 1; x += sideLength)
            {
                for (int y = 0; y < VertexPerLine - 1; y += sideLength)
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

            for (int x = 0; x < VertexPerLine - 1; x += halfSize)
            {
                for (int y = (x+halfSize) % sideLength; y < UPPER; y+=sideLength)
                {
                    
                }
            }
        }


//
//
//        int halfSize (int)(size * 0.5f);
//        int topLeft = row * (mSize + 1) + col;
//        int botLeft = (row + size) * (mSize + 1) + col;
//
//        int mid = (int)(row + halfSize) * (mSize + 1) + (int)(col + halfSize);
//        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[botLeft].y + vertices[botLeft + size].y) * 0.25f + UnityEngine.Random.Range(0, offset);
//        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[botLeft + size].y) * 0.25f + UnityEngine.Random.Range(0, offset);
//
//        vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
//        vertices[mid + halfSize].y = (vertices[topLeft].y + vertices[botLeft].y + vertices[mid].y) + UnityEngine.Random.Range(0, offset);
//        vertices[mid + halfSize].y = (vertices[topLeft + size].y + vertices[botLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
//        vertices[botLeft + halfSize].y = (vertices[botLeft].y + vertices[botLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
    }
}
