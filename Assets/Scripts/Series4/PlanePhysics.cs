using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Series4
{
    public class PlanePhysics : MonoBehaviour
    {
        public Vector3 Normal => _meshFilter.transform.TransformDirection(_meshFilter.mesh.normals[0]).normalized;
        public float d { get; private set; } = 0;
        public Vector3 SteepestDescent => new Vector3(Normal.x / Normal.y, -(Normal.x * Normal.x + Normal.z * Normal.z) / (Normal.y * Normal.y), Normal.z / Normal.y).normalized;

        private Vector2 _turnMovement;
        [SerializeField] private float turnSpeed = 1f;
        private readonly List<IceBlock> _iceBlocks = new List<IceBlock>();

        #region Unity Variables

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        #endregion

        private void Awake()
        {
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _iceBlocks.AddRange(FindObjectsOfType<IceBlock>());
        }

        private void FixedUpdate()
        {
            var angleZ = _turnMovement.x * turnSpeed * Time.fixedDeltaTime;
            var angleX = angleZ == 0 ? _turnMovement.y * turnSpeed * Time.fixedDeltaTime : 0;
            if (!(angleZ == 0 && angleX == 0))
            {
                transform.Rotate(Vector3.forward, angleZ);
                transform.Rotate(Vector3.right, angleX);
                _iceBlocks.ForEach(iceBlock => iceBlock.PlaneHasRotated(transform.position, angleZ, angleX));
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
            // Gizmos.DrawLine(transform.position, SteepestDescent);
        }
    }
}