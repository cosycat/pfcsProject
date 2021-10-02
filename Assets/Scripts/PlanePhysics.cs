using System;
using UnityEngine;

public class PlanePhysics : MonoBehaviour
{
    public Vector3 Normal { get; private set; } = Vector3.up;
    public float d { get; private set; } = 0;
}