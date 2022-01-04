using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class CubeScript : MonoBehaviour
{ 
    public Vector3 velocity;
    public float mass = 1;
    
    private Vector3 Momentum()
    {
        return velocity * mass;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherCube = other.gameObject.GetComponent<CubeScript>();
        var totalMomentum = Momentum() + otherCube.Momentum();
        var totalMass = mass + otherCube.mass;
        
        velocity = totalMomentum * (1.0f / totalMass);
        otherCube.velocity = velocity;
    }
}
