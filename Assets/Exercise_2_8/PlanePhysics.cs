using System;
using UnityEngine;

namespace Exercise_2_8
{
    public class PlanePhysics : MonoBehaviour
    {
        public Vector3 Normal { get; private set; } = Vector3.up;
        public float d { get; private set; } = 0;
    }
}
