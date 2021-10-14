using System;
using UnityEngine;

namespace Series4
{
    public class IceBlock : MonoBehaviour
    {
        
        private void FixedUpdate()
        {
            
        }


        public void PlaneHasRotated(Vector3 center, Vector3 axis1, float angle1, Vector3 axis2, float angle2)
        {
            transform.RotateAround(center, axis1, angle1);
            transform.RotateAround(center, axis2, angle2);
        }
    }
}
