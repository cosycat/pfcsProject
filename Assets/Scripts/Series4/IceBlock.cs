using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Series4
{
    public class IceBlock : MonoBehaviour
    {
        private PlanePhysics plane;
        [SerializeField] private float mass = 1;
        private const float g = 1;//9.81f;

        private Vector3 v = Vector3.zero;//Vector3.right * 0.5f; // konstant erhöhen

        private float Height => transform.localScale.y;
        
        private void Awake()
        {
            plane = FindObjectOfType<PlanePhysics>();
        }

        private void FixedUpdate()
        {
            var steepestDescent = plane.SteepestDescent;
            // Vector3 forceN = plane.Normal * g * mass;
            Vector3 forceG = Vector3.down * g * mass;
            Vector3 forceNet = Vector3.Project(forceG, steepestDescent);
            var a = forceNet / mass;
            var t = Time.fixedDeltaTime;
            var v0 = v;
            
            // var a = Vector3.right;
            
            transform.position += v0 * t + 0.5f * a * t * t;
            
            v += a * t;
        }

        public void PlaneHasRotated(Vector3 center, float angleZ, float angleX)
        {
            
            // first around z axis, then around x axis.
            var position = transform.position;
            transform.RotateAround(center, plane.transform.forward, angleZ);
            transform.RotateAround(center, plane.transform.right, angleX);

            // Vector3 distance = center - transform.position;

            var angleZRad = angleZ / 360 * 2 * Mathf.PI;
            var angleXRad = angleX / 360 * 2 * Mathf.PI;
            // // https://stackoverflow.com/questions/14607640/rotating-a-vector-in-3d-space
            // // rotation around z axis
            // // x cos θ − y sin θ
            // // x sin θ + y cos θ
            // // z
            // Debug.Log("rotate var with angleZ: " + angleZRad);
            // v = new Vector3(
            //     v.x * Mathf.Cos(angleZRad) - v.y * Mathf.Sin(angleZRad),
            //     v.x * Mathf.Sin(angleZRad) + v.y * Mathf.Cos(angleZRad),
            //     v.z);
            //
            // // rotation around x axis
            // // x
            // // y cos θ − z sin θ
            // // y sin θ + z cos θ
            //
            // v = new Vector3(
            //     v.x,
            //     v.y * Mathf.Cos(angleXRad) - v.z * Mathf.Sin(angleXRad),
            //     v.y * Mathf.Sin(angleXRad) + v.z * Mathf.Cos(angleXRad)
            // );
            //
            //
            // https://math.stackexchange.com/questions/511370/how-to-rotate-one-vector-about-another
            var theta = angleZRad;
            var a = v;
            if (a.magnitude != 0)
            {
                var b = plane.transform.forward;
                var a_b = a - (Vector3.Dot(a, b) / Vector3.Dot(b, b)) * b;
                var w = Vector3.Cross(b, a_b);
                var x1 = Mathf.Cos(theta) / a_b.magnitude;
                var x2 = Mathf.Sin(theta) / w.magnitude;
                var aNew = a_b.magnitude * (x1 * a_b + x2 * w);
                v = aNew;
                Debug.Log("a = " + a);
                Debug.Log("aNew = " + aNew);
            }
            
            
            
            // theta = angleXRad;
            // a = v;
            // if (a.magnitude != 0)
            // {
            //     var b = plane.transform.right;
            //     var a_b = (v - (Vector3.Dot(v, b) / Vector3.Dot(b, b)) * b);
            //     var w = Vector3.Cross(b, a_b);
            //     var x1 = Mathf.Cos(theta) / a_b.magnitude;
            //     var x2 = Mathf.Sin(theta) / w.magnitude;
            //     var aNew = a_b.magnitude * (x1 * a_b + x2 * w);
            //     v = aNew;
            // }
            

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, v + transform.position);
        }
    }
}
