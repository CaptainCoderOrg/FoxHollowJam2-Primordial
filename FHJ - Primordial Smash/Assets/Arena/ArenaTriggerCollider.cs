using UnityEngine;
using UnityEngine.Events;

public class ArenaTriggerCollider : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.IsPlayer()) { OnPlayerExit?.Invoke(); }
        if (other.IsPlayerProjectile(out var projectile))
        {
            projectile.Despawn();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.IsPlayer())
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
