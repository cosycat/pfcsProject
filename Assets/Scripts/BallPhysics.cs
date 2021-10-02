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
        // Debug.Log("Collision Detection START");
        var n = collisionPlane.Normal;
        var d = collisionPlane.d;
        var p0 = transform.position;
        var distance = Radius;
        var v0 = _velocity;

        // Use the formula for distance, use P(t)=p0 + v0*t + 0.5gt^2 as P(x,y,z) values and solve for t.
        var planeDistanceConstant = (float)Math.Sqrt(n.x * n.x + n.y * n.y + n.z * n.z);
        var a = (0.5f * n.x * g.x + 0.5f * n.y * g.y + 0.5f * n.z * g.z) / planeDistanceConstant;
        var b = (n.x * v0.x + n.y * v0.y + n.z * v0.z) / planeDistanceConstant;
        var c = (n.x * p0.x + n.y * p0.y + n.z * p0.z + d - distance) / planeDistanceConstant;

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
        float tColl;
        if (t1 < 0 && t2 < 0)
        {
            Debug.Log("Both collisions lie in the past.");
            return false; // Both collisions lie in the past.
        }

        if (t1 >= 0 && t2 >= 0)
        {
            Debug.Log("Both collisions are in the future, take the first one.");
            tColl = Math.Min(t1, t2); // Both collisions are in the future, take the first one.
        }
        else
        {
            // Debug.Log("Only one collision is in the future, take the only one > 0.");
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
        _velocity = vNew;
        
        Debug.Log("n = " + n);
        Debug.Log("d = " + d);
        Debug.Log("distance = " + distance);
        Debug.Log("p0 = " + p0);
        Debug.Log("pColl = " + pColl);
        Debug.Log("newPos = " + newPos);
        Debug.Log("v0 = " + v0);
        Debug.Log("vNew = " + vNew);
        Debug.Log("tColl = " + tColl);
        Debug.Log("Time.fixedTime = " + Time.fixedTime);
        Debug.Log("Time.fixedDeltaTime = " + Time.fixedDeltaTime);

        return true;
    }
    
}