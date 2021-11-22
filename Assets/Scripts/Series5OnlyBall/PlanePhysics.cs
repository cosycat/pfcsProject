using UnityEngine;
using UnityEngine.InputSystem;

namespace Series5OnlyBall
{
    public class PlanePhysics : MonoBehaviour
    {
        public Vector3 Normal => _meshFilter.transform.TransformDirection(_meshFilter.mesh.normals[0]);
        public float d { get; private set; } = 0;

        private Vector2 _turnMovement;
        [SerializeField] private float turnSpeed = 1f;//wie schnell sich die Plane dreht

        #region Unity Variables

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        #endregion

        private void Awake()
        {
            //Bei Start der App, filter und renderer Compontents werden geholt
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            //Vektor updaten der Ebene, abhängig vom Drehwinkel
            transform.Rotate(Vector3.forward, _turnMovement.x * turnSpeed);
        }

        public void OnTurn(InputAction.CallbackContext context)
        {
            //Steuern der Plane anhand der Pfeiltasten
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
    }
}