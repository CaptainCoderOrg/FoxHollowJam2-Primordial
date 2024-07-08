using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Showable : MonoBehaviour
{

    public Animator Animator;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Debug.Assert(Animator != null);
    }

    public void Show() => Animator.SetTrigger("Show");
    public void Hide() => Animator.SetTrigger("Hide");
}