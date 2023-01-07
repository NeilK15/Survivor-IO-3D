using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    #region SingletonPattern

    public static WeaponManager Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Weapon Manager???");
            return;
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    [Header("The currently equiped weapons")]
    public WeaponData[] weapons = new WeaponData[6];

    private int _index = 0;

    // Dictionary holding the prefabs of each weapon so multiple of same weapon don't exist
    private Dictionary<WeaponData, GameObject> _currentWeapons = new Dictionary<WeaponData, GameObject>();
    
    public Transform[] WeaponSlots = new Transform[Enum.GetNames(typeof(WeaponSlot)).Length];

    private void Start()
    {

        UpdateWeapons();
        
        
    }

    private void UpdateWeapons()
    {
        foreach (WeaponData weapon in weapons)
        {
            if (weapon && !_currentWeapons.ContainsKey(weapon))
            {
                _currentWeapons.Add(weapon, weapon.prefab);

                Instantiate(weapon.prefab, WeaponSlots[(int) weapon.weaponSlot]);
            }
        }
    }
    

    // Called when UI is used to update a weapon
    public void UpdateWeapon(WeaponData weapon)
    {
        // Call AddWeapon if new weapon
        if (!_currentWeapons.ContainsKey(weapon))
            AddWeapon(weapon);

        // Call UpgradeWeapon if upgrading weapon
        else
            UpgradeWeapon(weapon);
    }

    private void AddWeapon(WeaponData newWeapon)
    {   
        // Increment index
        _index++;
        
        // Call UpdateWeapons
        UpdateWeapons();
    }

    private void UpgradeWeapon(WeaponData weapon)
    {
        // Upgrade weapon
    }

    public void ClearWeapons()
    {
        
    }
    
}

public enum WeaponSlot {
    HiddenCenter,
    Front,
    OrbitLeft,
    OrbitRight
}