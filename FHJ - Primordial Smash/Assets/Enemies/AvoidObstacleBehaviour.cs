using UnityEngine;

public class AvoidObstacleBehaviour : MonoBehaviour
{
    public MonoBehaviour[] DisabledWhenActive;
    public Transform Body;
    private bool _isActive = false;
    private float _direction = 1;

    void Awake()
    {
        Debug.Assert(Body != null);
    }

    public void Update()
    {
        if (!_isActive) { return; }
        Body.Rotate(new Vector3(0, 0, 45));
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger stay");
        if (!_isActive){ Activate(); }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Deactivate();
    }

    public void Activate()
    {
        foreach (var behaviour in DisabledWhenActive)
        {
            behaviour.enabled = false;
        }
    }

    public void Deactivate()
    {
        foreach (var behaviour in DisabledWhenActive)
        {
            behaviour.enabled = true;
        }
    }
}