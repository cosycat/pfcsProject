using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class cubeLeftScript : MonoBehaviour
{
    private float initialPos;
    private Vector3 _velocity = Vector3.right;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        var t = Time.fixedDeltaTime;
        var a = new Vector3(0,0,0);
        var position = transform.right;
        transform.position = new Vector3(0, 0, 0);
        _velocity += a * t;
    }
}
