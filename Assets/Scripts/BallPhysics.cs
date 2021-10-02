using UnityEngine;

public class BallPhysics : MonoBehaviour
{

    private Vector3 _velocity = Vector3.zero;
    private static readonly Vector3 g = new Vector3(0, -9.81f, 0);
    public float Radius => _sphereCollider.radius;
        
        

    #region Unity Variables

    private SphereCollider _sphereCollider;
    private PlanePhysics plane;

    #endregion

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        plane = FindObjectOfType<PlanePhysics>();
    }


    private void FixedUpdate()
    {
        var t = Time.fixedDeltaTime;
        var newPosition = transform.position + _velocity * t + 0.5f * g * t * t;
        if (CheckForCollision(newPosition))
        {
            _velocity = -_velocity;
            // todo new newPosition
        }
        else
        {
            transform.position = newPosition;
        }
        ApplyGravitation(t);
    }

    private bool CheckForCollision(Vector3 newPosition)
    {
        return newPosition.y - Radius < 0;
    }

    private void ApplyGravitation(float t)
    {
        _velocity += g * t;
    }
    
}