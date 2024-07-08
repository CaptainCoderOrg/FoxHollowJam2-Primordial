using UnityEngine;

public class HealthPillController : MonoBehaviour
{

    public Animator Animator;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Debug.Assert(Animator != null);
        
    }

    public void Heal()
    {
        Animator.ResetTrigger("Damage");
        Animator.SetTrigger("Heal");
    } 

    public void Damage()
    {
        Animator.ResetTrigger("Heal");
        Animator.SetTrigger("Damage");
    }

}