using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{    
    public float spinSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update () {
        transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(1.0f, 0.0f, 0.0f));
    }
}
