using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public Animator AreaClearedAnimator;
    public Animator AreaReadyAnimator;

    [Button("Show Cleared")]
    public void ShowAreaCleared() => AreaClearedAnimator.SetTrigger("Show");
    [Button("Hide Cleared")]
    public void HideAreaCleared() => AreaClearedAnimator.SetTrigger("Hide");

    [Button("Show Ready")]
    public void ShowAreaReady() => AreaReadyAnimator.SetTrigger("Show");
    [Button("Hide Ready")]
    public void HideAreaReady() => AreaReadyAnimator.SetTrigger("Hide");

}