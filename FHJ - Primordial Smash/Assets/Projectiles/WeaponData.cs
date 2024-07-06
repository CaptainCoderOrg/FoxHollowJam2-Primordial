using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public ProjectileController Projectile;
    public float CoolDown = 0.05f;
    
}