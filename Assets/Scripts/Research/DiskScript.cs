using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 velocity;
    public float mass;
    public bool calculate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (!calculate)
            return;
        var otherCube = other.gameObject.GetComponent<DiskScript>();
        // https://director-online.dasdeck.com/buildArticle.php?id=532
        var position = transform.position;
        var x1 = position.x;
        var y1 = position.z;

        var position1 = otherCube.transform.position;
        var x2 = position1.x;
        var y2 = position1.z;
        
        var dx = x2 - x1;
        var dy = y2 - y1;

        var m1 = mass;
        var m2 = otherCube.mass;

        var phi = 0f;

        if (dx == 0)
        {
            phi = Mathf.PI / 2;
        }
        else
        {
            phi = Mathf.Atan(dy / dx);
        }
        var term = Mathf.PI / 180;
        var v1i = velocity.magnitude;
        var v2i = otherCube.velocity.magnitude;
        var ang1 = FindAngle() * term;
        var ang2 = otherCube.FindAngle() * term;

        var v1xr = v1i * Mathf.Cos(ang1 - phi);
        var v1yr = v1i * Mathf.Sin(ang1 - phi);

        var v2xr = v2i * Mathf.Cos(ang2 - phi);
        var v2yr = v2i * Mathf.Sin(ang2 - phi);
        
        var v1fxr = ((m1 - m2) * v1xr + (m2 + m2) * v2xr) / (m1 + m2);
        var v2fxr = ((m1 + m1) * v1xr + (m2 - m1) * v2xr) / (m1 + m2);
        var v1fyr = v1yr;
        var v2fyr = v2yr;

        var v1fx = Mathf.Cos(phi) * v1fxr + Mathf.Cos(phi + Mathf.PI / 2) * v1fyr;
        var v1fy = Mathf.Sin(phi) * v1fxr + Mathf.Sin(phi + Mathf.PI / 2) * v1fyr;
        
        var v2fx = Mathf.Cos(phi) * v2fxr + Mathf.Cos(phi + Mathf.PI / 2) * v2fyr;
        var v2fy = Mathf.Sin(phi) * v2fxr + Mathf.Sin(phi + Mathf.PI / 2) * v2fyr;
        velocity = new Vector3(v1fx, 0, v1fy);
        Debug.Log(velocity);
        otherCube.velocity = new Vector3(v2fx, 0, v2fy);
        Debug.Log(otherCube.velocity);
    }

    float FindAngle()
    {
        var term = Mathf.PI / 180f;
        var x = velocity.x;
        var y = velocity.z;
        if (x < 0)
            return 180f + Mathf.Atan(y / x) / term;
        if (x > 0 && y >= 0)
            return Mathf.Atan(y / x) / term;
        if(x > 0 && y < 0)
            return 360f + Mathf.Atan(y / x) / term;
        if (x == 0 && y == 0)
            return 0f;
        if (x == 0 && y >= 0)
            return 90f;
        return 270f;
    }
}

