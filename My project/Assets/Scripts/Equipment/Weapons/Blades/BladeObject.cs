using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeObject : MonoBehaviour
{
    [Header("Blade Data")] public AreaOfAttackWeaponData weaponData;
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Knockback(weaponData.knockback, Vector3.Scale(other.transform.position - other.ClosestPoint(transform.position), new Vector3(1,0,1)).normalized);
            damageable.Damage(weaponData.damage);
        }
    }
}
