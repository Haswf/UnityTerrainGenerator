using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHeight : MonoBehaviour
{
    public float SeaLevel;
    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(0.0f, SeaLevel, 0.0f);
    }
}
