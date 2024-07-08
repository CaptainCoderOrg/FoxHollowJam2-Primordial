using UnityEngine;
using UnityEngine.Events;

public class ArenaTriggerCollider : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.IsPlayer()) { OnPlayerExit?.Invoke(); }
        if (other.attachedRigidbody.TryGetComponent<ProjectileController>(out var projectile))
        {
            projectile.Despawn();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsPlayer())
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
