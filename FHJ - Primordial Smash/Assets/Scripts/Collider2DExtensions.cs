using UnityEngine;
public static class Collider2DExtensions
{

    public static bool IsPlayer(this Collider2D collider2D)
    {
        string tag = collider2D?.attachedRigidbody?.tag;
        return tag == "Player";
    }

    public static bool IsPlayerProjectile(this Collider2D collider2D)
    {
        string tag = collider2D?.attachedRigidbody?.tag;
        return tag == "Player Projectile";
    }

    public static bool IsPlayerProjectile(this Collider2D collider2D, out ProjectileStatsController damageInfo)
    {
        damageInfo = collider2D?.attachedRigidbody?.GetComponent<ProjectileStatsController>();
        string tag = collider2D?.attachedRigidbody?.tag;
        return tag == "Player Projectile";
    }

}