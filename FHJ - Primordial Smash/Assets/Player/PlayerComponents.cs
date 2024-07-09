using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    public int BaseHealth = 3;
    public int MaxHealth => BaseHealth + HealthBonus();
    public int Health => MaxHealth - Damage;
    public float InvlunerableTime = 0.25f;
    public bool IsInvulnerable = false;
    [SerializeField]
    private int _maxHealth = 3;
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private int _damage = 0;
    public bool IsDead => Health <= 0;
    public int Regen => 0 + RegenBonus();
    public UnityEvent<int> HealthChanged; 
    public int Damage
    {
        get => _damage;
        set
        {
            int newDamage = Mathf.Clamp(value, 0, MaxHealth);
            _damage = newDamage;
            HealthChanged?.Invoke(Health);
            if (Health <= 0)
            {
                Animator.SetTrigger("Death");
            }
        }
    }

    [Button("Sync")]
    public void Sync()
    {
        Damage = _damage;
    }

    public void TakeDamage()
    {
        if (IsInvulnerable) { return; }
        Damage++;
        StartCoroutine(Invulnerability());
    }

    private IEnumerator Invulnerability()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(InvlunerableTime);
        IsInvulnerable = false;
    }


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        HealthChanged.AddListener(value => 
        {
            _health = value;
            _maxHealth = MaxHealth;
        });
    }

    public void NotifyHealthChanged()
    {
        HealthChanged?.Invoke(Health);
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

    public int HealthBonus()
    {
        int bonus = 0;
        foreach (var powerup in PowerUps)
        {
            bonus += powerup.HealthBonus;
        }
        return bonus;
    }

    internal int PiercingBonus()
    {
        int bonus = 0;
        foreach (var powerup in PowerUps)
        {
            if (powerup.IsPiercing) { bonus++; }
        }
        return bonus;
    }

    internal int RegenBonus()
    {
        int bonus = 0;
        foreach (var powerup in PowerUps)
        {
            if (powerup.IsRegen) { bonus++; }
        }
        return bonus;
    }

    internal int SpreadBonus()
    {
        int bonus = 0;
        foreach (var powerup in PowerUps)
        {
            if (powerup.IsSpreadShot) { bonus++; }
        }
        return bonus;
    }
}
