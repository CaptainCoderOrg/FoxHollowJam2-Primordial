using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthBehaviour : MonoBehaviour
{
    public UnityEvent<float> OnDamaged;
    public UnityEvent OnDeath;
    public UnityEvent<Vector2, ChargeAttackController> OnKnockback;
    public float Health;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        Debug.Assert(_collider != null);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Health <= 0) { return; }
        if (other.IsPlayerTrampleAttack(out var attack))
        {
            Vector2 closestPoint = other.ClosestPoint(transform.position);
            Vector2 knockbackDirection = ((Vector2)transform.position - closestPoint).normalized;
            OnKnockback.Invoke(knockbackDirection, attack);
        }
        if (other.IsPlayerProjectile(out var damageInfo))
        {
            Health -= damageInfo.Damage;
            OnDamaged?.Invoke(damageInfo.Damage);
            if (Health <= 0) { Kill(); }
            damageInfo.Targets--;
            if (damageInfo.Targets <= 0) { Destroy(damageInfo.gameObject); }
        }
    }

    public void Kill()
    {
        OnDeath?.Invoke();
    }
}
