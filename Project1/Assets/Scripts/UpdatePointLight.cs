using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePointLight : MonoBehaviour
{
    public Shader shader;
    public PointLight pointLight;
    // Update is called once per frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }
}
