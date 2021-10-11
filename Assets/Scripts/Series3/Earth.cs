using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Series3
{
    public class Earth : MonoBehaviour
    {

        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private GameObject sun;

        private Vector3 _velocity;
        
        private float T => 2 * Mathf.PI * radius / speed;
        private float w => 2 * Mathf.PI / T;
        private float AccelerationMagnitude => speed * speed / radius;

        private List<Vector3> posList = new List<Vector3>();

        public Vector3 Center => sun.transform.position;

        private void Awake()
        {
            transform.position = new Vector3(sun.transform.position.x + radius, sun.transform.position.y, sun.transform.position.z);
            Debug.Log("T = " + T);
        }

        private void FixedUpdate()
        {
            // var deltaTheta = w * Time.fixedDeltaTime;
            // var theta = w * Time.time;
            // var newPosition = new Vector3(radius * Mathf.Cos(theta), transform.position.y, radius * Mathf.Sin(theta));
            
            var v = GetVelocity();
            var t = Time.deltaTime;
            var a = GetAccelerationForPosition();
            var p0 = transform.position;

            var newPosition = p0 + v * t + 0.5f * a * t * t;
            posList.Add(transform.position);
            transform.position = newPosition;

            Debug.Log($"Current Radius: {(transform.position - sun.transform.position).magnitude}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            posList.ForEach( pos => Gizmos.DrawSphere(pos, 1f));
            
        }

        private Vector3 GetVelocity()
        {
            var aDirection = (sun.transform.position - transform.position).normalized;
            var vDirection = new Vector3(aDirection.z, aDirection.y, -aDirection.x);
            var vMagnitude = speed;
            return vDirection * vMagnitude;
        }

        private Vector3 GetAccelerationForPosition()
        {
            var aDirection = (sun.transform.position - transform.position).normalized;
            var aMagnitude = AccelerationMagnitude;
            return aDirection * aMagnitude;
        }
    }
}
