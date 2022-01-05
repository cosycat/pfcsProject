using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScriptElastic : MonoBehaviour
{
    public Vector3 velocity;
    public float mass = 1;
    
    private Vector3 Momentum()
    {
        return velocity * mass;
    }

    private float Energy()
    {
        return 0.5f * velocity.magnitude * velocity.magnitude * mass;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherCube = other.gameObject.GetComponent<CubeScriptElastic>();
        var totalMomentum = Momentum() * Energy() + otherCube.Momentum() * otherCube.Energy(); //Impuls- und Energieerhaltungssatz
        var totalMass = mass + otherCube.mass;
        
        //(m1 * v1 + m2 * (2 * v2-v1)) / (m1 + m2)
        velocity = mass * velocity + otherCube.mass * (2 * otherCube.velocity - velocity) / totalMass;
        //velocity = totalMomentum * (1.0f / totalMass);
        otherCube.velocity = velocity;
    }
}
