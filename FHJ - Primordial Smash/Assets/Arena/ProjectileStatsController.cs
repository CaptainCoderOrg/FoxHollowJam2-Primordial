using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileStatsController : MonoBehaviour
{
    public int Targets = 1;
    public float Damage = 1f;

    public void Despawn()
    {
        Destroy(gameObject);
    }
}