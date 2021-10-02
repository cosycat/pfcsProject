using System;
using UnityEngine;

public class PlanePhysics : MonoBehaviour
{
    public Vector3 Normal { get; private set; } = new Vector3(-1, 1, 0).normalized;
    public float d { get; private set; } = 0;
    
    #region Unity Variables

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;


    #endregion

    private void Awake()
    {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        var normal = _meshFilter.transform.TransformDirection(_meshFilter.mesh.normals[0]);
        
    }
    
    
}