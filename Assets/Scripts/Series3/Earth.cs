using System;
using UnityEngine;

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
        
        public Vector3 Center => sun.transform.position;

        private void Awake()
        {
            transform.position = new Vector3(sun.transform.position.x + radius, sun.transform.position.y, sun.transform.position.z);
            Debug.Log("T = " + T);
        }

        private void FixedUpdate()
        {
            // var deltaTheta = w * Time.fixedDeltaTime;
            var theta = w * Time.time;
            var newPosition = new Vector3(radius * Mathf.Cos(theta), transform.position.y, radius * Mathf.Sin(theta));
            transform.position = newPosition;
        }


        // private Vector3 GetVelocity()
        // {
        //     var aDirection = (sun.transform.position - transform.position).normalized;
        //     var vDirection = Vector3.OrthoNormalize();
        // }

        private Vector3 GetAccelerationForPosition()
        {
            var aDirection = (sun.transform.position - transform.position).normalized;
            var aMagnitude = AccelerationMagnitude;
            return aDirection * aMagnitude;
        }

        // private Vector3 GetAccelerationForTime()
        // {
        //     var t = Time.time;
        // }
    }
}
