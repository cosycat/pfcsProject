using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GewichtScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject feder;
    private Vector3 pos;
    void Start()
    {
         pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(pos.x, 
        feder.transform.position.y  - feder.transform.localScale.y - transform.localScale.y / 2,
            pos.z);
    }
}
