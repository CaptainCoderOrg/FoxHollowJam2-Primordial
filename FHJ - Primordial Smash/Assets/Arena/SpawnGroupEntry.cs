using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Group Entry", menuName = "FHJ/Spawn Group Entry")]
public class SpawnGroupEntry : ScriptableObject
{
    public SpawnPoint SpawnPoint;
    public List<EnemyData> Enemies;
}