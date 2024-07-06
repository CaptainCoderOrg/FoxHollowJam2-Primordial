using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    [field: SerializeField]
    public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField]
    public GameObject Body { get; private set; }
    [field: SerializeField]
    public GameObject Turret { get; private set; }

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
}
