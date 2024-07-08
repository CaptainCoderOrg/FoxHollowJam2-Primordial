using System;
using System.Collections.Generic;

[Flags]
public enum SpawnPoint
{
    North = 1,
    East = 2,
    South = 4,
    West = 8,

}

public static class SpawnPointExtensions
{
    public static int ToIndex(this SpawnPoint spawnPoint)
    {
        List<int> indices = new();
        if (spawnPoint.HasFlag(SpawnPoint.North)) { indices.Add(0); }
        if (spawnPoint.HasFlag(SpawnPoint.East)) { indices.Add(1); }
        if (spawnPoint.HasFlag(SpawnPoint.South)) { indices.Add(2); }
        if (spawnPoint.HasFlag(SpawnPoint.West)) { indices.Add(3); }
        return indices[UnityEngine.Random.Range(0, indices.Count)];
    }

    public static SpawnPoint Opposite(this SpawnPoint spawnPoint)
    {
        return spawnPoint switch
        {
            SpawnPoint.North => SpawnPoint.South,
            SpawnPoint.East => SpawnPoint.West,
            SpawnPoint.South => SpawnPoint.North,
            SpawnPoint.West => SpawnPoint.East,
            _ => throw new System.NotImplementedException($"Unknown directionL {spawnPoint}"),
        };
    }
}