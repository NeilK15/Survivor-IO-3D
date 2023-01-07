using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Crate : MonoBehaviour, IDamageable
{

    [Header("Destroyed Crate Prefab")] 
    public GameObject destroyedCrate;


    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Damage(float damage)
    {
        Destroy(gameObject); 
    }

    public void Knockback(float force, Vector3 direction)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
        GameObject destroyed = Instantiate(destroyedCrate, transform.position, transform.rotation);

        foreach (Rigidbody rb in destroyed.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
        
        Damage(0);
    }
    
}
