using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScriptElastic : MonoBehaviour
{
    public float velocity;
    public float mass = 1;
    public bool calculate;
    

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += velocity * Vector3.forward * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!calculate)
            return;
        // m1 * v1'^2 * 0.5 + m2 * v2'^2 * 0.5 = m1 * v1^2 * 0.5 + m2 * v2^2 * 0.5
        // m1 * v1' + m2 * v2' = m1 * v1 + m2 * v2
        var otherCube = other.gameObject.GetComponent<CubeScriptElastic>();
        
        var m1 = mass;
        var m2 = otherCube.mass;
        var v1 = velocity;
        var v2 = otherCube.velocity;
        velocity = (m1 * v1 + m2 * v2 - (m2 * (2 * m1 * v1 - m1 * v2 + m2 * v2) / (m1 + m2)) )/ m1;
        otherCube.velocity = (2 * m1 * v1 - m1 * v2 + m2 * v2) / (m1 + m2);
    }
}
