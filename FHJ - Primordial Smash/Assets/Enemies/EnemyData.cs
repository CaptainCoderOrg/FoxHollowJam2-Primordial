
using System;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "Enemy", menuName = "FHJ/Enemy")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
}
