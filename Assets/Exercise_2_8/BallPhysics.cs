using UnityEngine;

namespace Exercise_2_8
{
    public class BallPhysics : MonoBehaviour
    {
        private float velocity = 0;
        private const float g = -9.81f;

        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }


        private void FixedUpdate()
        {
            ApplyCollision();
            ApplyGravitation();
        }

        private void ApplyCollision()
        {
            if (transform.position.y - _sphereCollider.radius < 0)
            {
                velocity = -velocity;
            }
        }

        private void ApplyGravitation()
        {
            var t = Time.fixedDeltaTime;
            transform.position += new Vector3(0, velocity * t + 0.5f * g * t * t);
            // transform.position += new Vector3(0, velocity * t); // This produces the unity physics _bug, where the Ball gains height in every jump
            velocity += g * t;
        }
    
    }
}
