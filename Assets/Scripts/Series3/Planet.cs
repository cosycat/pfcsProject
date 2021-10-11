using System;
using System.Collections.Generic;
using UnityEngine;

namespace Series3
{
    public class Planet : MonoBehaviour
    {

        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private GameObject centerObject;
        [SerializeField] private float selfRotationsPerRotation = 3;


        private Vector3 v;
        private Vector3 a;
        
        private readonly List<Vector3> _posList = new List<Vector3>();

        private float T => 2 * Mathf.PI * radius / speed;
        private float w => 2 * Mathf.PI / T;
        
        public Vector3 Center => centerObject.transform.position;

        private void Awake()
        {
            v = GetStartVelocity();
            transform.position = new Vector3(Center.x + radius, Center.y, Center.z);
            a = transform.position - Center;
        }

        private void FixedUpdate()
        {
            _posList.Add(transform.position); // For Debug

            var t = Time.deltaTime;

            // Rotation
            var angle = 360 * t / T * selfRotationsPerRotation;
            transform.Rotate(Vector3.up, angle);
            
            // Updating the position
            transform.position += v * t;
            a = Center - transform.position;
            v += a * t;

            Debug.Log($"Current Radius: {(transform.position - Center).magnitude}");
        }
        
        private void OnDrawGizmos()
        {
            Debug.Log("OnDrawGizmos");
            Gizmos.color = Color.green;
            _posList.ForEach( pos =>
            {
                // Debug.Log($"OnDrawGizmos: {pos}");
                Gizmos.DrawSphere(pos, 1f);
            });
        }
        
        private Vector3 GetStartVelocity()
        {
            var aDirection = (Center - transform.position).normalized;
            var vDirection = new Vector3(aDirection.z, aDirection.y, -aDirection.x);
            var vMagnitude = speed;
            return vDirection * vMagnitude;
        }
        
    }
}
