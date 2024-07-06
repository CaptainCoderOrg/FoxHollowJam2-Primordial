using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [field: SerializeField]
    public float MovementSpeed { get; private set; } = 5;
    [field: SerializeField]
    public Vector2 MovementInput { get; private set; }

    [SerializeField]
    private PlayerComponents Player;
    public float MagnitudeDeadZone = 0.0001f;
    private Vector2 _lastMovement;

    void Awake()
    {
        Player = GetComponent<PlayerComponents>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = default;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        MovementInput = input;
        if (MovementInput.magnitude > MagnitudeDeadZone)
        {
            Player.Body.transform.rotation = Quaternion.FromToRotation(Vector2.up, MovementInput.normalized);
        }
    }

    void FixedUpdate()
    {
        Player.Rigidbody.velocity = MovementInput * MovementSpeed;
        Player.Animator.SetFloat("velocity", Player.Rigidbody.velocity.magnitude);
    }

    
}
