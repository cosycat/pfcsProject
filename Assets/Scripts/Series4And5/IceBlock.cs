using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Series4And5
{
    public class IceBlock : MonoBehaviour
    {
        private PlanePhysics plane;
        [SerializeField] private float mass = 1;
        private const float g = 9.81f;

        private Vector3 v = Vector3.zero;//Vector3.right * 0.5f; // konstant erhöhen

        //serie 5: Reibung
        private const float Muek = 0.3f; //kinetic, Gleitreibungskoeffizient Eisblock auf Eis.
        private const float Mues = 0.5f; //static
        private float forceFriction = 0;  //Reibungskraft
        private float gleitReibung = 0.5f;  //Gleitreibung
        private float haftReibung = 0.5f;  //Haftreibung
        private float luftReibung = 0.5f;  //Luftreibung
        private Vector3 FH = Vector3.zero;
        private Vector3 FR = Vector3.zero;

        private float Height => transform.localScale.y;
        
        private void Awake()
        {
            plane = FindObjectOfType<PlanePhysics>();
        }

        //FixedUpdate für Serie 4
        // private void FixedUpdate()
        // {
        //     var steepestDescent = plane.SteepestDescent;
        //     // Vector3 forceN = plane.Normal * g * mass;
        //     Vector3 forceG = Vector3.down * g * mass; //Gravitaion ohne Reibung. Nötig für Serie 4
        //     Vector3 forceNet = Vector3.Project(forceG, steepestDescent); //Normalenvektor
        //     var a = forceNet / mass;
        //     var t = Time.fixedDeltaTime;
        //     var v0 = v;
        //     
        //     // var a = Vector3.right;
        //     
        //     transform.position += v0 * t + 0.5f * a * t * t;
        //     
        //     v += a * t;
        // }

        //FixedUpdate für Serie 5
        private void FixedUpdate()
        {
            var steepestDescent = plane.SteepestDescent;
            var FG = mass * g;
            //Neigungswinkel
            var alpha = 90 - (Math.Abs((Vector3.Angle(-steepestDescent, Vector3.up))));
            alpha = alpha / 360 * 2 * Mathf.PI;
            //Hangantriebskraft aktualisieren
            FH = steepestDescent * (FG * Mathf.Sin(alpha)); //Hangantriebskraft wird "aufgeblasen" in Richtung von steepestDescent
            var FN = plane.Normal * FG * Mathf.Cos(alpha); //Normalkraft wird "aufgeblasen" in Richtung von steepestDescent
            //Reibungskraft aktualisieren
            FR = -steepestDescent * FN.magnitude * (v == Vector3.zero ? Mues : Muek); //wenn v=0, dann statisches, sonst kinetisches Mü
            var FTotal = FH + FR;
            //prüfe: wenn FR grösser als FN, soll FTotal auf 0 gesetzt werden ("abbremsen")
            if (FR.magnitude > FH.magnitude && v.Equals(Vector3.zero))
            {
                FTotal = Vector3.zero;
            }
            var a = FTotal / mass; //Beschleunigung des IceBlocks
            var t = Time.fixedDeltaTime; //Zeitabschnitt
            var v0 = v; //VectorZero
            transform.position += v0 * t + 0.5f * a * t * t; //IceBlockPosition aktualisiert
            v += a * t; //aufsummieren der velocity
        }

        public void PlaneHasRotated(Vector3 center, float angleZ, float angleX)
        {
            
            // first around z axis, then around x axis.
            var position = transform.position;
            transform.RotateAround(center, plane.transform.forward, angleZ);
            transform.RotateAround(center, plane.transform.right, angleX);

            v = Quaternion.Euler(angleX, 0, angleZ) * v;
            
            // Vector3 distance = center - transform.position;

            var angleZRad = angleZ / 360 * 2 * Mathf.PI;
            var angleXRad = angleX / 360 * 2 * Mathf.PI;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var position = transform.position;
            Gizmos.DrawLine(position, v + position);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, position + FH);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(position, position + FR);
        }
    }
}
