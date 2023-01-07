using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfAttackWeapon : Weapon
{
    [Header("Area of Attack Weapon Data")] public AreaOfAttackWeaponData weaponData;


    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        AttackOn();

        yield return new WaitForSeconds(weaponData.timeOn);
        
        AttackOff();

        yield return new WaitForSeconds(weaponData.timeOff);

        StartCoroutine(Attack());

    }


    protected virtual void AttackOn()
    {
        print("attack on");
    }

    protected virtual void AttackOff()
    {
        print("attack off");
    }
}