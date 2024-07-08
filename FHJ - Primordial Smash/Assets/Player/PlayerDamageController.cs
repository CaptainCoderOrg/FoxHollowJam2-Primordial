using UnityEngine;
using UnityEngine.Events;

public class PlayerDamageController : MonoBehaviour
{
    public PlayerComponents Player;
    public AudioSource AudioSource;
    public UnityEvent<float> OnDamaged;

    void Awake()
    {
        Player = GetComponentInParent<PlayerComponents>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyWeaponController>(out var enemyWeapon))
        {
            OnDamaged?.Invoke(enemyWeapon.TotalDamage);
            if (AudioSource.isPlaying) { return; }
            if(enemyWeapon.DamageSounds.Length > 0)
            {
                AudioSource.clip = enemyWeapon.DamageSounds[Random.Range(0, enemyWeapon.DamageSounds.Length)];
                AudioSource.Play();
            }
        }
    }
}