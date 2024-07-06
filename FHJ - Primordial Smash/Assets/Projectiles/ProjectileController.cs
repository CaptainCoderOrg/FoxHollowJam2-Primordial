using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    public float Speed;
    public Vector2 Direction
    {
        get => _rigidbody.velocity;
        set => _rigidbody.velocity = value.normalized * Speed;
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
}
