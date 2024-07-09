using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "FHJ/Enemy Wave")]
public class EnemyWaveData : ScriptableObject
{
    public List<WaveEntry> Entries;
    
    public int MinRow = 0;
    public int MaxRow = 6;
}

[Serializable]
public class WaveEntry
{
    public float Delay;
    public EnemySpawnGroup SpawnGroup;
}