using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathBehaviour : MonoBehaviour
{
    public GameObject ToDespawn;
    public void Despawn()
    {
        Destroy(ToDespawn);
    }

}