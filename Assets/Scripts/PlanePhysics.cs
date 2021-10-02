using System;
using UnityEngine;

public class PlanePhysics : MonoBehaviour
{
    public Vector3 Normal { get; private set; } = new Vector3(-1, 1, 0).normalized;
    public float d { get; private set; } = 0;
}