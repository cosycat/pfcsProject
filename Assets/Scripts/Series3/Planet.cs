using System;
using UnityEngine;

namespace Series3
{
    public class Planet : MonoBehaviour
    {

        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private GameObject centerObject;

        private Vector3 v;
        private Vector3 a;
        
        private float T => 2 * Mathf.PI * radius / speed;
        private float w => 2 * Mathf.PI / T;
        private float AccelerationMagnitude => speed * speed / radius;
        
        public Vector3 Center => centerObject.transform.position;

        private void Awake()
        {
            v = GetStartVelocity();
            transform.position = new Vector3(Center.x + radius, Center.y, Center.z);
            a = transform.position - Center;
        }

        private void FixedUpdate()
        {
            // var deltaTheta = w * Time.fixedDeltaTime;
            // var theta = w * Time.time;
            // var newPosition = new Vector3(radius * Mathf.Cos(theta), transform.position.y, radius * Mathf.Sin(theta));
            
            
            var t = Time.deltaTime;

            transform.position += v * t;
            a = Center - transform.position;
            v += a * t;
            // var newPosition = p0 + v * t + 0.5f * a * t * t;
            // transform.position = newPosition;

            Debug.Log($"Current Radius: {(transform.position - Center).magnitude}");
        }
        
        private Vector3 GetStartVelocity()
        {
            var aDirection = (Center - transform.position).normalized;
            var vDirection = new Vector3(aDirection.z, aDirection.y, -aDirection.x);
            var vMagnitude = speed;
            return vDirection * vMagnitude;
        }

        // private Vector3 GetAccelerationForPosition()
        // {
        //     var aDirection = (centerObject.transform.position - transform.position).normalized;
        //     var aMagnitude = AccelerationMagnitude;
        //     return aDirection * aMagnitude;
        // }

        // private Vector3 GetAccelerationForTime()
        // {
        //     var t = Time.time;
        // }
    }
}
