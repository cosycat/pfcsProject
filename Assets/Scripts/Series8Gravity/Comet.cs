using System;
using UnityEngine;

namespace Series8Gravity
{
    public class Comet : MonoBehaviour
    {
        private const double Scale = 0.0000005;
        
        [SerializeField] private GameObject earth;
        private double massEarth = 5.972e24 * Scale; // M in Kg
        private double massComet = 1 * Scale; // m
        private double radiusEarth = 6371000 * Scale; // in Meter
        private double radiusComet = 1000000 * Scale; // in Meter


        private const double G = 6.67e-11;

        private Vector3 _velocity;

        private float r => (float)((earth.transform.position - transform.position).magnitude);
        private Vector3 a => (earth.transform.position - this.transform.position).normalized * (float)(massEarth * G / (r * r));

        private void Awake()
        {
            _velocity = CalculateStartVelocity();
            earth.transform.localScale = new Vector3((float)(radiusEarth * 2), (float)(radiusEarth * 2), (float)(radiusEarth * 2));
            transform.localScale = new Vector3((float)(radiusComet * 2), (float)(radiusComet * 2), (float)(radiusComet * 2));
        }

        private static Vector3 CalculateStartVelocity()
        {
            // TODO
            return Vector3.zero;
        }

        private void FixedUpdate()
        {
            var M = massEarth;
            var m = massComet;
            var dt = (float)(Time.deltaTime * Scale);
            var aMagnitude = M * G / (r * r);
            var sDirection = (earth.transform.position - this.transform.position).normalized;
            var a = sDirection * (float)aMagnitude;

            _velocity += a * dt;
            transform.position += _velocity * dt;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(earth.transform.position, this.transform.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + a * Time.fixedDeltaTime * (float)Scale);
        }
    }
}
