using UnityEngine;

public class EnemyCountController : MonoBehaviour
{
    [SerializeField]
    private ArenaController _arenaController;
    void Awake()
    {
        _arenaController = FindFirstObjectByType<ArenaController>();
        _arenaController.LivingEnemies++;
    }

    public void Despawn()
    {
        _arenaController.LivingEnemies--;
        Destroy(gameObject);        
    }
}