using UnityEngine;
public static class Collider2DExtensions
{

    public static bool IsPlayer(Collider2D collider2D)
    {
        string tag = collider2D?.attachedRigidbody?.tag;
        return tag == "Player";
    }

    public static bool IsPlayerProjectile(Collider2D collider2D)
    {
        string tag = collider2D?.attachedRigidbody?.tag;
        return tag == "Player Projectile";
    }

}