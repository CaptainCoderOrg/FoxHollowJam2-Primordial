using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    [field: SerializeField]
    public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField]
    public GameObject Body { get; private set; }
    [field: SerializeField]
    public GameObject Turret { get; private set; }
    [field: SerializeField]
    public Transform ProjectileSpawn { get; private set; }
    [field: SerializeField]
    public WeaponData Weapon { get; set; }
    [field: SerializeField]
    public Animator Animator { get; private set; }
    public List<PowerUpData> PowerUps = new();

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    public float SpeedMultiplier()
    {
        float multiplier = 1;
        foreach (var powerup in PowerUps)
        {
            multiplier *= powerup.SpeedMultiplier;
        }
        return multiplier;
    }

    
    public float CoolDownMultiplier()
    {
        float multiplier = 1;
        foreach (var powerup in PowerUps)
        {
            multiplier *= powerup.CoolDownMultiplier;
        }
        return multiplier;
    }

    public float DamageMultiplier()
    {
        float multiplier = 1;
        foreach (var powerup in PowerUps)
        {
            multiplier *= powerup.DamageMultiplier;
        }
        return multiplier;
    }

    public float HealthMultiplier()
    {
        float multiplier = 1;
        foreach (var powerup in PowerUps)
        {
            multiplier *= powerup.HealthMultiplier;
        }
        return multiplier;
    }

}
