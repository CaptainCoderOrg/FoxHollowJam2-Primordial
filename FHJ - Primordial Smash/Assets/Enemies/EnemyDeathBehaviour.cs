using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathBehaviour : MonoBehaviour
{
    [SerializeField]
    private ArenaController _arenaController;
    void Awake()
    {
        _arenaController = FindFirstObjectByType<ArenaController>();
        _arenaController.LivingEnemies++;
    }
    public GameObject ToDespawn;
    public void Despawn()
    {
        _arenaController.LivingEnemies--;
        Destroy(ToDespawn);
    }

}