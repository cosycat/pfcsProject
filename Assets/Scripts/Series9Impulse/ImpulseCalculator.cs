using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEditor.UIElements;
using UnityEngine;

public class ImpulseCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public cubeLeftScript cube1 = FindObjectOfType<cubeLeftScript>();
    public cubeRightScript cube2 = FindObjectOfType<cubeRightScript>();
    
    //cubeLeft
    public float a1; 
    public float m1;
    public Vector3 p1Vec;
    public Vector3 v1Vec;
    public float v1Delta;
    
    //cubeRight
    public float a2; 
    public float m2;
    public Vector3 p2Vec;
    public Vector3 v2Vec;
    public float v2Delta;

    void Start()
    {
        a1 = 0.5f;
        m1 = 5f;
        p1Vec = m1 * v1Vec;

        a2 = 0.5f;
        m2 = 5f;
        p2Vec = m2 * v2Vec;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var collisionPosition = (m2 * a2) + (-m1 * a1);
        
    }
}
