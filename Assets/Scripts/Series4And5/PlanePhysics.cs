using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Series4And5
{
    public class PlanePhysics : MonoBehaviour
    {
        //Normalenvektor der Plane
        public Vector3 Normal
        {
            get
            {
                var meshNormal = _meshFilter.mesh.normals[0];
                return _meshFilter.transform.TransformDirection(meshNormal).normalized;
            }
        }
        
        public float d { get; private set; } = 0;
        
        //Neigungswinkel der Plane
        public Vector3 SteepestDescent => new Vector3(Normal.x / Normal.y, -(Normal.x * Normal.x + Normal.z * Normal.z) / (Normal.y * Normal.y), Normal.z / Normal.y).normalized;

        private Vector2 _turnMovement; //Pfeilbewegungen
        [SerializeField] private float turnSpeed = 1f; //Schnelligkeit der Bewegung der Plane bei gedrückten Pfeiltasten
        private readonly List<IceBlock> _iceBlocks = new List<IceBlock>(); //IceBlockListe

        #region Unity Variables

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        #endregion

        private void Awake()
        {
            //Bei Start vorhanden: Filter, Renderer und IceBlocks
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _iceBlocks.AddRange(FindObjectsOfType<IceBlock>());
        }

        private void FixedUpdate()
        {
            //Bewegung der Plane anhand der Pfeiltasten wird aktualisiert
            transform.RotateAround(transform.position, Vector3.forward, _turnMovement.x * turnSpeed);
            transform.RotateAround(transform.position, Vector3.right, _turnMovement.y * turnSpeed);
            
            //solange Ebene in Bewegung ist, bewege auch die IceBlocks
            if (!_turnMovement.Equals(Vector2.zero))
            {
                _iceBlocks.ForEach(iceBlock => iceBlock.PlaneHasRotated(transform.position, _turnMovement.x * turnSpeed, _turnMovement.y * turnSpeed));
            }
        }

        public void OnTurn(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _turnMovement = context.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Canceled:
                    _turnMovement = context.ReadValue<Vector2>();
                    break;
            }
        }

        public void OnReset(InputAction.CallbackContext context)
        {
            transform.rotation = Quaternion.identity;
        }

        private void OnDrawGizmos()
        {
            try
            {
                Gizmos.DrawLine(transform.position, SteepestDescent);
            }
            catch (Exception e)
            {
                
            }

        }
    }
}