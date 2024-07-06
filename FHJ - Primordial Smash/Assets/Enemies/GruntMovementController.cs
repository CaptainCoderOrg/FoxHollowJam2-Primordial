using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntMovementController : MonoBehaviour
{
    public GameObject Body;
    public PlayerComponents Target;
    public Vector2 Direction;
    public float Speed = 1;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        AcquireTarget();
        Move();
    }

    public void AcquireTarget()
    {
        if (Target != null) { return; }
        Target = FindFirstObjectByType<PlayerComponents>();
    }

    public void Move()
    {
        if (Target == null) { return; }
        Direction = (Vector2.MoveTowards(transform.position, Target.transform.position, 1) - (Vector2)transform.position).normalized;
        Body.transform.rotation = Quaternion.FromToRotation(Vector2.up, Direction);
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = Direction * Speed;
    }
}
