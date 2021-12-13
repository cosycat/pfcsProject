using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FederScript : MonoBehaviour
{
    public float K;
    private float initialLength;
    public float weight;
    private float speed;
    
    // Start is called before the first frame update
    void Start()
    {
		this.initialLength = this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        var position = transform.position;
        
        var federKraft = K * (initialLength - scale.y);
        var gewichtsKraft = weight;
        
        var delta = federKraft + gewichtsKraft;
        speed += delta * Time.fixedDeltaTime * 0.1f; 
        
        transform.localScale = new Vector3(scale.x, scale.y + speed, scale.z);
        transform.position = new Vector3(position.x, position.y - speed, position.z);
    }
}
