using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : ScriptableObject
{

    [Header("Equipment Data Settings")]
    public new string name = "equipment";

    public Sprite icon;
    
    [Header("Prefab for Equipment")]
    public GameObject prefab;

}
