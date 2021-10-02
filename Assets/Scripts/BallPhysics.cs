using System;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    private readonly Vector3 g = new Vector3(0, -9.81f, 0);
    public float Radius => _sphereCollider.radius;


    #region Unity Variables

    private SphereCollider _sphereCollider;
    private PlanePhysics _plane;

    #endregion

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _plane = FindObjectOfType<PlanePhysics>();
    }


    private void FixedUpdate()
    {
        if (CheckForCollision(_plane))
        {
            Debug.Log("Collision was detected and handled.");
        }
        else
        {
            var t = Time.fixedDeltaTime;
            transform.position += _velocity * t + 0.5f * g * t * t;;
            _velocity += g * t;
        }

    }

    private bool CheckForCollision(PlanePhysics collisionPlane)
    {
        var n = collisionPlane.Normal;
        var d = collisionPlane.d;
        var p0 = transform.position;
        var distance = Radius;
        var v0 = _velocity;

        // Use the formula for distance, use P(t)=p0 + v0*t + 0.5gt^2 as P(x,y,z) values and solve for t.
        var a = (n.x * p0.x + n.y * p0.y + n.z * p0.z + d) / distance;
        var b = (n.x * v0.x + n.y * v0.y + n.z * v0.z) / distance;
        var c = (0.5f * n.x * g.x + 0.5f * n.y * g.y + 0.5f * n.z * g.z) / distance;

        var D = b * b - 4 * a * c;
        if (D <= 0)
        {
            // If D < 0 there is never a collision and we can ignore it
            // If D == 0 the collision is there, but it doesn't change the direction.
            return false;
        }

        var t1 = (float)(-b + Math.Sqrt(D)) / (2 * a);
        var t2 = (float)(-b - Math.Sqrt(D)) / (2 * a);

        // TODO also get t3 and t4 with negative distance, to allow to hit the plane from both sides.

        // only take the smallest positive t (as this will be the next time it hits in the future)
        float t;
        if (t1 < 0 && t2 < 0)
            return false; // Both collisions lie in the past.
        if (t1 >= 0 && t2 >= 0)
            t = Math.Min(t1, t2); // Both collisions are in the future, take the first one.
        else
            t = Math.Max(t1, t2); // Only one collision is in the future, take the only one > 0;

        if (t > Time.fixedDeltaTime)
            return false; // The collision won't happen this frame.
        
        // Now we know there is a collision in this frame at time t0 + t.
        
        // The velocity and position at the time of the collision:
        var vBeforeColl = v0 + g * t;
        var pColl = p0 + v0 * t + 0.5f * g * t * t;
        
        // Now mirror the velocity on the plane:
        var vAfterColl = vBeforeColl - 2 * Vector3.Dot(vBeforeColl, n) * n;
        
        // Calculate the new velocity and position at the end of the frame:
        var tRemaining = Time.fixedDeltaTime - t;
        var newPos = pColl + vAfterColl * tRemaining + 0.5f * g * tRemaining * tRemaining;
        var vNew = vAfterColl + g * tRemaining;
        
        // Now move the ball for the remaining time with the new velocity and update the velocity:
        transform.position = newPos;
        _velocity = vNew;

        return true;
    }
    
}