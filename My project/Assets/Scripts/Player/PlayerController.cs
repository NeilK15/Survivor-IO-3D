using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

//Responsible for handling weapon and equipment on the player
public class PlayerController : MonoBehaviour
{
    
    #region SingletonPattern

    public static PlayerController Instance;
    
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
    
    
    // All the changeable properties for this script
    [FormerlySerializedAs("rangeRadius")] [Header("Properties")]
    public float attackRangeRadius = 10f;
    public LayerMask enemyMask;

    [Space(10)]
    public float pickUpRadius = 1f;
    public float attrackRadius = 4f;
    public float attrackForce = 10f;
    public LayerMask itemMask;

    [Header("Player Stats TEMP")] 
    public float money = 0;
    
    [HideInInspector]
    public delegate void OnEnemiesInRange(Collider[] enemies);
    public static OnEnemiesInRange EnemiesInRange;

    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForEnemies();
        
        
    }

    private void LateUpdate()
    {
        CheckForPickUps();
        
    }
    
    
    private void CheckForEnemies()
    {
        if (Physics.CheckSphere(transform.position, attackRangeRadius, enemyMask))
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, attackRangeRadius, enemyMask);
            
            EnemiesInRange?.Invoke(enemies);
        }
    }

    private void CheckForPickUps()
    {

        if (Physics.CheckSphere(transform.position, attrackRadius, itemMask))
        {
            Collider[] attrackItems = Physics.OverlapSphere(transform.position, attrackRadius, itemMask);
            Collider[] pickUpItems = Physics.OverlapSphere(transform.position, pickUpRadius, itemMask);
            
            foreach (Collider item in attrackItems)
            {
                ICanPickUp canPickUp = item.GetComponent<ICanPickUp>();

                if (canPickUp != null)
                {
                    print("Attracting");

                    float force = attrackForce + _rb.velocity.magnitude * 0.5f;
                    
                    canPickUp.Attract(transform.position, force * Time.deltaTime);
                }
            }

            foreach (Collider item in pickUpItems)
            {
                ICanPickUp canPickUp = item.GetComponent<ICanPickUp>();

                if (canPickUp != null)
                {
                    print("Attracting");
                    money += canPickUp.PickUp();
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attrackRadius);
    }
    
}