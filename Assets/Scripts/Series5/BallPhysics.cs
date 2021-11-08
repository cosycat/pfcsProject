using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Series5
{
    public class BallPhysics : MonoBehaviour
    {
        private Vector3 _velocity = Vector3.zero;
        private readonly Vector3 g = new Vector3(0, -9.81f, 0);
        public float Radius { get; private set; } = 0.5f;

        public float bounciness = 1;

        private bool _isMoving = false;
        public Vector3 StartPosition { get; private set; }

        #region Unity Variables

        private PlanePhysics _plane;

        #endregion

        private void Awake()
        {
            _plane = FindObjectOfType<PlanePhysics>();
            StartPosition = transform.position;
        }


        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_isMoving) return;
        
            if (CheckForCollision(_plane))
            {
                Debug.Log("Collision was detected and handled.");
            }
            else
            {
                var t = Time.fixedDeltaTime;
                transform.position += _velocity * t + 0.5f * g * t * t;
                ;
                _velocity += g * t;
            }
        }

        private bool CheckForCollision(PlanePhysics collisionPlane)
        {
            // Debug.Log("Collision Detection START");
            var n = collisionPlane.Normal;
            var d = collisionPlane.d;
            var p0 = transform.position;
            var r = Radius;
            var v0 = _velocity;
            var planeDistanceConstant = (float)Math.Sqrt(n.x * n.x + n.y * n.y + n.z * n.z);

            // First check, if we even are in a distance, where a collision is possible to happen this frame.
            var currDistance = Mathf.Abs(n.x * p0.x + n.y * p0.y + n.z * p0.z) / planeDistanceConstant;
            if (currDistance > 4 * r)
            {
                return false; // If we are so fast, that we would still hit next frame, we could also just clip through the plane anyway, so ignore it.
            }

            bool isClipping = false;
            if (currDistance < r)
            {
                // We clipped through the plane. Maybe the plane instead hit us.
                // Lets just move us exactly onto the plane, but only if we traveled from above.
                // TODO check if velocity points in the same direction like the normal, and if so, ignore everything.
                // if (false)
                //     return false;
                isClipping = true;
            }

            // Use the formula for distance, use P(t)=p0 + v0*t + 0.5gt^2 as P(x,y,z) values and solve for t.
            // This get's us the collision time with t0 being the start time of this frame.
            var a = (0.5f * n.x * g.x + 0.5f * n.y * g.y + 0.5f * n.z * g.z) / planeDistanceConstant;
            var b = (n.x * v0.x + n.y * v0.y + n.z * v0.z) / planeDistanceConstant;
            var c = (n.x * p0.x + n.y * p0.y + n.z * p0.z + d - r) / planeDistanceConstant;

            var D = b * b - 4 * a * c;
            if (D <= 0)
            {
            
                // If D < 0 there is never a collision and we can ignore it
                // If D == 0 the collision is there, but it doesn't change the direction.
                Debug.Log("There will be no collision in the current flight path");
                if (isClipping)
                {
                    MoveOntoPlane(collisionPlane, currDistance);
                }
                return false;
            }

            var t1 = (float)(-b + Math.Sqrt(D)) / (2 * a);
            var t2 = (float)(-b - Math.Sqrt(D)) / (2 * a);

            // TODO maybe also get t3 and t4 with negative distance (r), to allow to hit the plane from both sides.

            // only take the smallest positive t (as this will be the next time it hits in the future)
            // except if we are already clipping, we want the largest negative t, as we want to turn back time for the ball.
            float tColl;
            if (t1 < 0 && t2 < 0)
            {
                Debug.Log("Both collisions lie in the past.");
                if (isClipping)
                    tColl = Mathf.Max(t1, t2);
                else
                    return false; // Both collisions lie in the past and we are not clipping.
            }
            else if (t1 >= 0 && t2 >= 0)
            {
                Debug.Log("Both collisions are in the future, take the first one.");
                tColl = Math.Min(t1, t2); // Both collisions are in the future, take the first one.
            }
            else
            {
                // Debug.Log("Only one collision is in the future, take the only one > 0.");
                if (isClipping)
                    tColl = Math.Min(t1, t2); // Except if they are clipping, then take the one in the past (< 0)
                else
                    tColl = Math.Max(t1, t2); // Only one collision is in the future, take the only one > 0.
            }

            if (tColl > Time.fixedDeltaTime)
            {
                // Debug.Log("The collision won't happen this frame.");
                return false; // The collision won't happen this frame.
            }

            // Now we know there is a collision in this frame at time t0 + t.
            Debug.Log("COLLISION DETECTED!");

            // The velocity and position at the time of the collision:
            var vBeforeColl = v0 + g * tColl;
            var pColl = p0 + v0 * tColl + 0.5f * g * tColl * tColl;
        
            // Now mirror the velocity on the plane:
            var vAfterColl = vBeforeColl - 2 * Vector3.Dot(vBeforeColl, n) * n;
        
            // Calculate the new velocity and position at the end of the frame:
            var tRemaining = Time.fixedDeltaTime - tColl;
            var newPos = pColl + vAfterColl * tRemaining + 0.5f * g * tRemaining * tRemaining;
            var vNew = vAfterColl + g * tRemaining;
        
            // Now move the ball for the remaining time with the new velocity and update the velocity:
            transform.position = newPos;
            _velocity = vNew * bounciness;
        
            // Debug.Log("n = " + n);
            // Debug.Log("d = " + d);
            // Debug.Log("distance = " + distance);
            // Debug.Log("p0 = " + p0);
            // Debug.Log("pColl = " + pColl);
            // Debug.Log("newPos = " + newPos);
            // Debug.Log("v0 = " + v0);
            // Debug.Log("vNew = " + vNew);
            // Debug.Log("tColl = " + tColl);
            // Debug.Log("Time.fixedTime = " + Time.fixedTime);
            // Debug.Log("Time.fixedDeltaTime = " + Time.fixedDeltaTime);

            return true;
        }

        private void MoveOntoPlane(PlanePhysics collisionPlane, float currDistance)
        {
            var deltaDistance = Radius - currDistance;
            transform.position += collisionPlane.Normal * deltaDistance * 1.01f;
        }

        #region Input Events

        public void OnStartStopMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isMoving = !_isMoving;
            }
        }

        public void OnReset(InputAction.CallbackContext context)
        {
            transform.position = StartPosition;
            _velocity = Vector3.zero;
            _isMoving = false;
        }

        #endregion
    
    }
}