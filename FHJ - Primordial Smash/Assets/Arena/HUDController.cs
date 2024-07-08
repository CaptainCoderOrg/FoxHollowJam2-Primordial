using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public Animator AreaClearedAnimator;

    [Button("Show Cleared")]
    public void ShowAreaCleared() => AreaClearedAnimator.SetTrigger("Show");
    [Button("Hide Cleared")]
    public void HideAreaCleared() => AreaClearedAnimator.SetTrigger("Hide");

}