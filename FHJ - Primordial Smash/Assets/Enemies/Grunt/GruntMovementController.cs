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
    public bool IsMoving => _isKnockedBack || (!_animator.GetBool("isAttacking") && !_animator.GetBool("isDead"));
    public string MidAirLayer = "EnemyMidAir";
    public string EnemyLayer = "Enemy";
    public GameObject DeathSound;
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

    public float KnockbackDuration = 0.25f;
    private bool _isKnockedBack = false;

    public void Knockback(Vector2 knockbackDirection, ChargeAttackController attack) => StartCoroutine(DoKnockback(knockbackDirection, attack));
    private IEnumerator DoKnockback(Vector2 direction, ChargeAttackController attack)
    {
        _isKnockedBack = true;
        _animator.SetBool("isStunned", true);
        _animator.SetBool("isAttacking", false);
        Direction = direction * attack.Power;
        yield return new WaitForSeconds(KnockbackDuration);
        Direction = Vector2.zero;
        yield return new WaitForSeconds(attack.StunDuration);
        _animator.SetBool("isStunned", false);
        _isKnockedBack = false;
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
        if (_isKnockedBack) { return; }
        Direction = (Vector2.MoveTowards(transform.position, Target.transform.position, 1) - (Vector2)transform.position).normalized;
        Body.transform.rotation = Quaternion.FromToRotation(Vector2.up, Direction);
    }

    public void StartAttack()
    {
        if (_isKnockedBack) { return; }
        _inAttackRange = true;
        _animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        _inAttackRange = false;
    }

    public void CheckAttackRange()
    {
        if (_isKnockedBack) { _inAttackRange = false; }
        _animator.SetBool("isAttacking", _inAttackRange);
    }
    
    [Button("Kill")]
    public void Kill()
    {

        _animator.SetBool("isDead", true);
        
    }
}
