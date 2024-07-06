using UnityEditor.MPE;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    [field: SerializeField]
    public Vector2 DirectionInput { get; private set; }

    [SerializeField]
    private PlayerComponents Player;
    public float MagnitudeDeadZone = 0.1f;
    private Vector2 _lastMovement;

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
            // Player.ProjectileSpawn
            ProjectileController projectile = Instantiate(Player.Weapon);
            projectile.Direction = DirectionInput;
            projectile.gameObject.transform.position = Player.ProjectileSpawn.position;
            projectile.gameObject.transform.rotation = Player.Turret.transform.rotation;
        }
    }

    
}
