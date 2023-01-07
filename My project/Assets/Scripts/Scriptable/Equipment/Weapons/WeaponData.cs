using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
public class WeaponData : EquipmentData
{
    [Header("Weapon Settings")] public float damage = 20f;

    public float knockback = 5f;

    [Space(height: 10)]
    // Where the weapon will be held
    public WeaponSlot weaponSlot;
}