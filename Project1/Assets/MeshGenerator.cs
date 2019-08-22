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
    public int xSize = 20;
    public int zSize = 20;

    public float mHeight;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }
    
    void CreateShape()
    {
        // how many vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                
                vert++;
                tris += 6;
            }

            vert++;
        }


        //vertices[0].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[xSize].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[vertices.Length - 1].y = UnityEngine.Random.Range(-mHeight, mHeight);
        //vertices[vertices.Length - 1 - xSize].y = UnityEngine.Random.Range(-mHeight, mHeight);

        vertices[0].y = 5;
        vertices[xSize].y = 5;
        vertices[vertices.Length - 1].y = 5;
        vertices[vertices.Length - 1 - xSize].y = 5;




        int iterations = (int)Mathf.Log(xSize, 2);
        int numSquares = 1; // num_square* num_square 个小正方形
        int squareSize = xSize; //每个小正方形边长

        for (int n = 0; n < iterations; n++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    DimondSquare(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;
        }




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




    void DimondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (xSize + 1) + col;
        int botLeft = (row + size) * (xSize + 1) + col;

        int mid = (int)(row + halfSize) * (xSize + 1) + (int)(col + halfSize);
        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[botLeft].y + vertices[botLeft + size].y) * 0.25f + UnityEngine.Random.Range(0, offset);
        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[botLeft + size].y) * 0.25f + UnityEngine.Random.Range(0, offset);

        vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
        vertices[mid + halfSize].y = (vertices[topLeft].y + vertices[botLeft].y + vertices[mid].y) + UnityEngine.Random.Range(0, offset);
        vertices[mid + halfSize].y = (vertices[topLeft + size].y + vertices[botLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
        vertices[botLeft + halfSize].y = (vertices[botLeft].y + vertices[botLeft + size].y + vertices[mid].y) / 3 + UnityEngine.Random.Range(0, offset);
    }
}
