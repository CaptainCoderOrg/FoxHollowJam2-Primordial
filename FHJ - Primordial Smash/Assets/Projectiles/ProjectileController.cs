using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    public Transform Forward;
    public float Speed;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * Speed;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
    
}
