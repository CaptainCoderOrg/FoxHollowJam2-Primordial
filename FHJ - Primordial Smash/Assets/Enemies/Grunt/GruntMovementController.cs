using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GruntMovementController : MonoBehaviour
{
    public SpriteRenderer[] Renderers;
    public GameObject Body;
    public PlayerComponents Target;
    public Vector2 Direction;
    public float MinSpeed = 1;
    public float MaxSpeed = 2;
    public float Speed = 1;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _inAttackRange = false;
    public bool IsMoving => !_animator.GetBool("isAttacking") && !_animator.GetBool("isDead");
    public string MidAirLayer = "EnemyMidAir";
    public string EnemyLayer = "Enemy";
    void Awake()
    {
        Speed = Random.Range(MinSpeed, MaxSpeed);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        AcquireTarget();
        CalculateDirection();
    }

    public void Airborn()
    {
        foreach (var renderer in Renderers)
        {
            renderer.sortingLayerName = MidAirLayer;
        }
    }

    public void Ground()
    {
        foreach(var renderer in Renderers)
        {
            renderer.sortingLayerName = EnemyLayer;
        }
    }

    void FixedUpdate()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (!IsMoving)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        _rigidbody.velocity = Vector2.zero;
        if (IsMoving)
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
        if (!IsMoving) { return; }
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
    
    [Button("Kill")]
    public void Kill()
    {

        _animator.SetBool("isDead", true);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
