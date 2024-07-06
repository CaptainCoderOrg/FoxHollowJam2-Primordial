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
    private Animator _animator;
    private bool _inAttackRange = false;
    public bool IsAttacking => _animator.GetBool("isAttacking");
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        AcquireTarget();
        CalculateDirection();
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = Vector2.zero;
        if (!IsAttacking)
        {
            _rigidbody.velocity = Direction * Speed;
        }
    }

    public void AcquireTarget()
    {
        if (Target != null) { return; }
        Target = FindFirstObjectByType<PlayerComponents>();
    }

    public void CalculateDirection()
    {
        if (Target == null) { return; }
        if (IsAttacking) { return; }
        Direction = (Vector2.MoveTowards(transform.position, Target.transform.position, 1) - (Vector2)transform.position).normalized;
        Body.transform.rotation = Quaternion.FromToRotation(Vector2.up, Direction);
    }

    public void StartAttack()
    {
        _inAttackRange = true;
        _animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        _inAttackRange = false;
    }

    public void CheckAttackRange()
    {
        _animator.SetBool("isAttacking", _inAttackRange);
    }
}
