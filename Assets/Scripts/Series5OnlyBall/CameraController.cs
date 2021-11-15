using UnityEngine;
using UnityEngine.InputSystem;

namespace Series5OnlyBall
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        public Vector3 StartPosition { get; private set; }
        [SerializeField] private float zoomSpeed = 1f;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            StartPosition = transform.position;
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            var newPos = transform.position;
            newPos.z += zoomSpeed * context.ReadValue<float>();
            transform.position = newPos;
        }

        public void OnReset(InputAction.CallbackContext context)
        {
            transform.position = StartPosition;
        }
    
    }
}
