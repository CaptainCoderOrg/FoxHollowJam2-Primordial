using UnityEngine;
public class ShowHideController : MonoBehaviour 
{
    public Animator Animator;

    public bool IsVisible { get; private set; }

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Debug.Assert(Animator != null);
    }

    public void Toggle()
    {
        if (IsVisible) { Hide(); }
        else { Show(); }
    }

    public void Show()
    {
        if (IsVisible) { return; }
        IsVisible = true;
        Animator.SetTrigger("Show");
    }

    public void Hide()
    {
        if (!IsVisible) { return; }
        IsVisible = false;
        Animator.SetTrigger("Hide");
    }

    
}