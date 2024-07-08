using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthBehaviour : MonoBehaviour
{
    public UnityEvent<float> OnDamaged;
    public UnityEvent OnDeath;
    public float Health;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Health <= 0) { return; }
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
