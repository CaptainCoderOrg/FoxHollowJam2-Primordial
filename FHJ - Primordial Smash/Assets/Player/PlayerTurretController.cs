using System.Collections;
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
        if (mousePosition != MousePosition || Input.GetMouseButton(0))
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
        CreateProjectile(0);
        for (int ix = 0; ix < Player.SpreadBonus(); ix++)
        {
            CreateProjectile(Random.Range(-30f, 30f));
        }
        

        StartCoroutine(CoolDownWeapon(Player.Weapon.CoolDown * Player.CoolDownMultiplier()));
    }

    private void CreateProjectile(float angle)
    {
        Quaternion rotation = Quaternion.Euler(Player.Turret.transform.rotation.eulerAngles + new Vector3(0, 0, angle));
        GameObject parent = Instantiate(Player.Weapon.Projectile, Player.ProjectileSpawn.position, rotation);
        foreach (var stats in parent.GetComponentsInChildren<ProjectileStatsController>())
        {
            stats.Targets += Player.PiercingBonus();
        }
        for (int ix = parent.transform.childCount - 1; ix >= 0; ix--)
        {
            Transform child = parent.transform.GetChild(ix);
            child.SetParent(null);
        }
        Destroy(parent.gameObject);
    }

    private IEnumerator CoolDownWeapon(float duration)
    {
        CanFire = false;
        yield return new WaitForSeconds(duration);
        CanFire = true;
    }

    
}
