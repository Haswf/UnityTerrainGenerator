﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FlyingVehicle : MonoBehaviour
{    
    public float moveSpeed = 10.0f;
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    // drag MeshGen to this GameObject
    public GameObject terrain;
    public float SeaLevel;
    
    private float _yaw = 0.0f;
    private float _pitch = 0.0f;

    // Update is called once per frame
    void Update()
    {   
        // Get mesh from MeshGen object
        Mesh mesh = terrain.GetComponent<MeshFilter>().mesh;

        // Make cursor invisible
        Cursor.visible = false;
        // lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        
        _yaw += horizontalSpeed * Input.GetAxis("Mouse X");
        _pitch -= verticalSpeed * Input.GetAxis("Mouse Y");
        
        // change orientation of camera
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        
        // vector to store movement
        Vector3 movement = Vector3.zero;
        
        // Move object in the direction of camera
        if (Input.GetKey(KeyCode.W)){
            movement += Camera.main.transform.forward * moveSpeed * Time.deltaTime;
        }
        // - is used to get backward direction
        if (Input.GetKey(KeyCode.S)){
            movement += Camera.main.transform.forward * -moveSpeed * Time.deltaTime;
        }
        // - is used to get left direction
        if (Input.GetKey(KeyCode.A)){
            movement += Camera.main.transform.right * -moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)){
            movement += Camera.main.transform.right * moveSpeed * Time.deltaTime;
        }
        // check x coordinate if within boundary
        if (transform.position.x + movement.x <= mesh.bounds.size.x - 5 && transform.position.x + movement.x >= 5 &&
            // check z coordinate if within boundary
            transform.position.z + movement.z <= mesh.bounds.size.z -5 && transform.position.z + movement.z >= 5 && 
            transform.position.y + movement.y > SeaLevel + 5)
        {
            // move camera
            transform.position += movement;
        }

    }
    private void OnCollisionEnter(Collision col)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }
}
