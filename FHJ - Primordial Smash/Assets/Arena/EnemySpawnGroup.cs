
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Group", menuName = "FHJ/Spawn Group")]
public class EnemySpawnGroup : ScriptableObject
{
    public List<SpawnGroupEntry> Entries;
}
