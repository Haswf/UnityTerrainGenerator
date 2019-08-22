using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{    
    public float moveSpeed = 10.0f;
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    
    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {    
        // Make cursor invisible
        Cursor.visible = false;
        // lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        
        _yaw += horizontalSpeed * Input.GetAxis("Mouse X");
        _pitch -= verticalSpeed * Input.GetAxis("Mouse Y");
        
        // change orientation of camera
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        
        // Move object in the direction of camera
        if (Input.GetKey(KeyCode.W)){
            transform.position += Camera.main.transform.forward * moveSpeed * Time.deltaTime;
        }
        // - is used to get backward direction
        if (Input.GetKey(KeyCode.S)){
            transform.position += Camera.main.transform.forward * -moveSpeed * Time.deltaTime;
        }
        // - is used to get left direction
        if (Input.GetKey(KeyCode.A)){
            transform.position += Camera.main.transform.right * -moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)){
            transform.position += Camera.main.transform.right * moveSpeed * Time.deltaTime;
        }
    }
}
