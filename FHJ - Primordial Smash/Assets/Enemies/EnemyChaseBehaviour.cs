using NaughtyAttributes;
using UnityEngine;

public class EnemyChaseBehaviour : MonoBehaviour
{
    public GameObject Body;
    public PlayerComponents Target;
    public Vector2 Direction;
    public float MinSpeed = 1;
    public float MaxSpeed = 2;
    public float Speed = 1;
    private Rigidbody2D _rigidbody;
    void Awake()
    {
        Speed = Random.Range(MinSpeed, MaxSpeed);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        AcquireTarget();
        CalculateDirection();
    }


    void FixedUpdate()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.velocity = Direction * Speed;
    }

    public void AcquireTarget()
    {
        if (Target != null) { return; }
        Target = FindFirstObjectByType<PlayerComponents>();
    }

    public void CalculateDirection()
    {
        if (Target == null) { return; }
        Direction = (Vector2.MoveTowards(transform.position, Target.transform.position, 1) - (Vector2)transform.position).normalized;
        Body.transform.rotation = Quaternion.FromToRotation(Vector2.up, Direction);
    }
}
