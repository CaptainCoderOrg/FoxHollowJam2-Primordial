using UnityEngine;
using UnityEngine.Events;

public class ExitController : MonoBehaviour
{
    public LayerMask WallLayer;
    public LayerMask None;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter");
        if (other.IsPlayer())
        {
            other.excludeLayers = WallLayer;
            OnEnter?.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exit");
        if (other.IsPlayer())
        {
            other.excludeLayers = None;
            OnExit?.Invoke();
        }
    }

}
