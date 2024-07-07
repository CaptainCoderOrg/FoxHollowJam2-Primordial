using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public EnemySpawnGroup NextSpawn;

    public void Spawn(EnemySpawnGroup enemySpawnGroup)
    {
        foreach (SpawnGroupEntry entry in enemySpawnGroup.Entries)
        {
            foreach (EnemyData enemy in entry.Enemies)
            {
                GameObject newEnemy = Instantiate(enemy.Prefab);
                float xOff = Random.Range(-2, 2);
                float yOff = Random.Range(-2, 2);
                Vector3 position = SpawnPoints[(int)entry.SpawnPoint].position;
                position.x += xOff;
                position.y += yOff;
                newEnemy.transform.position = position;
            }
        }
    }

    [Button("SpawnNext")]
    public void SpawnNext()
    {
        Spawn(NextSpawn);
    }
}
