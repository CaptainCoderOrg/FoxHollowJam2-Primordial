
using System;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "PowerUp", menuName = "FHJ/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string Name;
    public string Description;
    public float SpeedMultiplier = 1;
    public float CoolDownMultiplier = 1;
    public int HealthBonus = 0;
    public float DamageMultiplier = 1;
    public bool IsSpreadShot = false;
    public bool IsPiercing = false;
    public bool HealAll = false;
    
}
