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
        
        private void Awake()
        {
            plane = FindObjectOfType<PlanePhysics>();
        }

        private void FixedUpdate()
        {
            var nx = plane.Normal.x;
            var ny = plane.Normal.y;
            var nz = plane.Normal.z;
            var steepestDescent = new Vector3(nx / ny, -(nx * nx + nz * nz) / (ny * ny), nz / ny).normalized;
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
            transform.RotateAround(center, Vector3.forward, angleZ);
            transform.RotateAround(center, Vector3.right, angleX);

            // Vector3 distance = center - transform.position;

            var angleZRad = angleZ / 360 * 2 * Mathf.PI;
            var angleXRad = angleX / 360 * 2 * Mathf.PI;
            // https://stackoverflow.com/questions/14607640/rotating-a-vector-in-3d-space
            // rotation around z axis
            // x cos θ − y sin θ
            // x sin θ + y cos θ
            // z
            Debug.Log("rotate var with angleZ: " + angleZRad);
            v = new Vector3(
                v.x * Mathf.Cos(angleZRad) - v.y * Mathf.Sin(angleZRad),
                v.x * Mathf.Sin(angleZRad) + v.y * Mathf.Cos(angleZRad),
                v.z);
            
            // rotation around x axis
            // x
            // y cos θ − z sin θ
            // y sin θ + z cos θ
            
            v = new Vector3(
                v.x,
                v.y * Mathf.Cos(angleXRad) - v.z * Mathf.Sin(angleXRad),
                v.y * Mathf.Sin(angleXRad) + v.z * Mathf.Cos(angleXRad)
            );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, v + transform.position);
        }
    }
}
