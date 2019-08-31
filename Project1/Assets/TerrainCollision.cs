using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]

public class TerrainCollision : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        print("Collision!");
//        Camera cam = GetComponent<Camera>();
//        cam.transform.position = col.transform.position + new Vector3(0, 1, 0);
    }
}
