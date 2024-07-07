using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public GameObject Projectile;
    public float CoolDown = 0.05f;
    
}