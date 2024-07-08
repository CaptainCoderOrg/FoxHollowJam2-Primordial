using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materials Data", menuName = "FHJ/Materials")]
public class MaterialsData : ScriptableObject
{
    public string Name = "???";
    public PowerUpData[] PowerUps;

    public PowerUpData GetPowerUp()
    {
        return PowerUps[Random.Range(0, PowerUps.Length)];
    }
}
