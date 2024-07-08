using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    public float Damage = 1;
    public float TotalDamage => Damage;
    public AudioClip[] DamageSounds;
}