using UnityEngine;

namespace Exercise_2_8
{
    public class BallPhysics : MonoBehaviour
    {
        private Vector3 _velocity = Vector3.zero;
        private static readonly Vector3 g = new Vector3(0, -9.81f, 0);

        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }


        private void FixedUpdate()
        {
            if (CheckAndApplyCollision())
            {
            }
            ApplyGravitation();
        }

        private bool CheckAndApplyCollision()
        {
            if (transform.position.y - _sphereCollider.radius < 0)
            {
                _velocity = -_velocity;
                return true;
            }
            return false;
        }

        private void ApplyGravitation()
        {
            var t = Time.fixedDeltaTime;

            transform.position += _velocity * t + 0.5f * g * t * t;// new Vector3(0, _velocity * t + 0.5f * g * t * t);
            // transform.position += new Vector3(0, velocity * t); // This produces the unity physics _bug, where the Ball gains height in every jump
            _velocity += g * t;
        }
    
    }
}
