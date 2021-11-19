using System;
using System.Collections.Generic;
using UnityEngine;

//Das ist die Version wo die Erde sich auf einer Ellipse bewegt
//korrekte Version
namespace Series3
{
    public class Planet : MonoBehaviour
    {

        [SerializeField] private float radius;//vorgegeben
        [SerializeField] private float speed;//vorgegeben
        [SerializeField] private GameObject centerObject;//Das Zentrum. In unserem Fall die Sonne
        //Anzahl wie oft sich das Objekt (Erde) um sich selber dreht pro Umrundung (um die Sonne)
        [SerializeField] private float selfRotationsPerRotation = 3;


        private Vector3 v; //velocity
        private Vector3 a; //acceleration
        
        private readonly List<Vector3> _posList = new List<Vector3>(); //posListe des Objekts

        private float T => 2 * Mathf.PI * radius / speed;
        private float w => 2 * Mathf.PI / T;
        
        public Vector3 Center => centerObject.transform.position; //Das Zentrum stellt die Sonne dar in unserem Beispiel

        private void Awake()
        {
            //Startwerte bei Beginn der Applikation
            v = GetStartVelocity();
            transform.position = new Vector3(Center.x + radius, Center.y, Center.z); //Positionsberechnung der Erde
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
