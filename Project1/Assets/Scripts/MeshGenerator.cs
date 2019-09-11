using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]


public class MeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private Mesh mesh;
    // vector3 array of vertices
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Color[] colorMap;
    private int[] triangles;

    // 2^n. This is n.
    public int dimension;

   

    // number of vertices
    private int size;
    void Start()
    {
        size = (int)Mathf.Pow(2, dimension);
        // Create new mesh
        mesh = new Mesh();
        // Assign created mesh to the object attached to.
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();

        UpdateMesh();
        UpdateMeshCollider();

    }


    void CreateMesh()
    {
        vertices = new Vector3[size * size];
        colorMap = new Color[size * size];
        uvs = new Vector2[size * size];
        triangles = new int[(size - 1) * (size - 1) * 6];
        // create vertices
        int i = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                colorMap[i] = new Color(30,144,255);
                i++;
            }
        }

        // create triangles
        int vert = 0;
        int tris = 0;
        for (int y = 0; y < size - 1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size;
                triangles[tris + 5] = vert + size + 1;
                uvs[vert] = new Vector2(x / (float)size, y / (float)size);
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMeshCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colorMap;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

    }
}
