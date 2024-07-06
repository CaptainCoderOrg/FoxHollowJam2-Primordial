using System.Collections;
using UnityEditor.MPE;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    [field: SerializeField]
    public Vector2 DirectionInput { get; private set; }

    [SerializeField]
    private PlayerComponents Player;
    public float MagnitudeDeadZone = 0.1f;
    [field: SerializeField]
    public bool CanFire { get; private set; } = true;

    void Awake()
    {
        Player = GetComponent<PlayerComponents>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = default;
        input.x = Input.GetAxis("TurretHorizontal");
        input.y = Input.GetAxis("TurretVertical");
        DirectionInput = input;
        if (DirectionInput.magnitude > MagnitudeDeadZone)
        {
            Player.Turret.transform.rotation = Quaternion.FromToRotation(Vector2.up, DirectionInput.normalized);
            Fire();
            
        }
    }

    private void Fire()
    {
        if (!CanFire) { return; }
        ProjectileController projectile = Instantiate(Player.Weapon.Projectile);
        projectile.Direction = DirectionInput;
        projectile.gameObject.transform.SetPositionAndRotation(Player.ProjectileSpawn.position, Player.Turret.transform.rotation);
        StartCoroutine(CoolDownWeapon(Player.Weapon.CoolDown));
    }

    private IEnumerator CoolDownWeapon(float duration)
    {
        CanFire = false;
        yield return new WaitForSeconds(duration);
        CanFire = true;
    }

    
}
