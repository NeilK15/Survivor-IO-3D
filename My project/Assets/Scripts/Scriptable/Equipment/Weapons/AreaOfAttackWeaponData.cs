using UnityEngine;


[CreateAssetMenu(fileName = "Area of Attack Weapon", menuName = "Inventory/AreaOfAttackWeapon", order = 0)]
public class AreaOfAttackWeaponData : WeaponData
{
    [Header("Area of Attack Weapon Settings")] [Tooltip("The time that the weapon is on for (in seconds)")]
    public float timeOn = 10f;

    [Tooltip("The time that the weapon is off for (in seconds)")]
    public float timeOff = 5f;
}