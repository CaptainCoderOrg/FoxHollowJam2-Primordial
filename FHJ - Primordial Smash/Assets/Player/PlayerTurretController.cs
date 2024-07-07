using System.Collections;
using UnityEditor.Experimental.GraphView;
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
    [field: SerializeField]
    public Vector2 MousePosition { get; private set; }

    void Awake()
    {
        Player = GetComponent<PlayerComponents>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnalog();
        HandleMouse();
    }

    private void HandleMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (mousePosition != MousePosition)
        {
            MousePosition = mousePosition; 
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);
            DirectionInput = (Vector2.MoveTowards(transform.position, worldPosition, 1) - (Vector2)transform.position).normalized;
            Debug.DrawLine(transform.position, DirectionInput + (Vector2)transform.position);
            Player.Turret.transform.rotation = Quaternion.FromToRotation(Vector2.up, DirectionInput);
        
        }
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    private void HandleAnalog()
    {
        
        Vector2 input = default;
        input.x = Input.GetAxis("TurretHorizontal");
        input.y = Input.GetAxis("TurretVertical");
        if (input.x == 0 && input.y == 0) { return; }
        
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
