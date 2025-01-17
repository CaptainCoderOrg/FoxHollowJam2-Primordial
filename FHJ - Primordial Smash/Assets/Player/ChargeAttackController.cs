using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ChargeAttackController : MonoBehaviour
{
    public UnityEvent<float> OnCoolDownChange;
    [SerializeField]
    private PlayerComponents Player;
    [field: SerializeField]
    public bool IsCharging { get; private set; } = false;
    [field: SerializeField]
    public float CoolDownDuration { get; set; } = 5;
    [field: SerializeField]
    public float Duration { get; set; } = 1;
    private Vector2 ChargeDirection;
    public float ChargeForce = 50;
    public AnimationCurve ChargeSpeed;
    public float ChargeAmount = 0;
    public bool CanCharge => !_isTrampling && _coolDownRemaining <= 0;
    public float Power = 10;
    public float StunDuration = 1;

    private bool _isTrampling = false;
    [SerializeField]
    private float _coolDownRemaining = 0;

    void Awake()
    {
        Player = GetComponent<PlayerComponents>();
    }

    public void Update()
    {
        if (Input.GetButton("ChargeAttack") && CanCharge)
        {
            IsCharging = true;
        }
        else
        {
            IsCharging = false;
        }
        if (Input.GetButtonUp("ChargeAttack") && ChargeAmount >= 0.5f)
        {
            
            StartCoroutine(Charge(ChargeAmount * Player.DashPowerMultiplier()));
        }
        Player.Animator.SetBool("isCharging", IsCharging);
    }

    private IEnumerator Charge(float amount)
    {
        float totalCoolDown = CoolDownDuration * Player.DashCoolDownMultiplier();
        _coolDownRemaining = totalCoolDown;
        ChargeDirection = Player.MovementController.Direction;
        Player.MovementController.enabled = false;
        _isTrampling = true;
        Player.Animator.SetBool("isTrampling", true);
        float duration = Duration;
        while (duration > 0)
        {
            yield return new WaitForFixedUpdate();
            duration -= Time.fixedDeltaTime;
            float percent = ChargeSpeed.Evaluate((Duration - duration)/Duration);
            Player.Rigidbody.velocity = ChargeDirection * ChargeForce*amount * percent;
        }
        _isTrampling = false;
        Player.Animator.SetBool("isTrampling", false);
        Player.MovementController.enabled = true;
        OnCoolDownChange?.Invoke(1);
        while (_coolDownRemaining > 0)
        {
            yield return null;
            _coolDownRemaining -= Time.deltaTime;
            float percent = Mathf.Clamp(_coolDownRemaining / totalCoolDown, 0, 1);
            OnCoolDownChange?.Invoke(percent);
        }
        OnCoolDownChange?.Invoke(0);
    }
}