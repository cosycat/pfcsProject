using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeRightScript : MonoBehaviour
{
    private float initialPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        var position = - transform.right;

        transform.position = new Vector3(1, 0, 0);
        
    }
}
