using UnityEngine;
using UnityEngine.Events;

public class Showable : MonoBehaviour
{

    public UnityEvent OnShow;
    public UnityEvent OnHide;
    public Animator Animator;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Debug.Assert(Animator != null);
    }

    public void Show()
    {
        Animator.SetTrigger("Show");
        OnShow?.Invoke();
    }
    public void Hide()
    {
        Animator.SetTrigger("Hide");
        OnHide?.Invoke();
    }
}