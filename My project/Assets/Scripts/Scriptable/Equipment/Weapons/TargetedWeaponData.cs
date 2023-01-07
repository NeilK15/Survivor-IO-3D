using UnityEngine;


[CreateAssetMenu(fileName = "Targeted Weapon", menuName = "Inventory/TargetedWeapon", order = 0)]
public class TargetedWeaponData : WeaponData
{
    [Header("Targeted Weapon Settings")] [Tooltip("Measured In RPM")]
    public float attackSpeed = 300f;
    public float projectileSpeed;
}