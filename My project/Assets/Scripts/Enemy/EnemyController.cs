using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyController : MonoBehaviour, IDamageable
{

    // Reference to ScriptableObject
    [Header("Enemy ScriptableObject Reference")]
    public EnemyData enemy;
    
    // Other variables
    [HideInInspector] public new string name;
    
    // Private variables for enemy
    [Header("Preferences")]
    private float _health;
    private float _damage;
    private float _attackSpeed;
    private float _predictedHealth;
    public Material damageFlash;
    public float flashTimeMilis = 250f;

    [Header("References")] 
    public ParticleSystem deathEffect;
    
    public delegate void OnEnemyDeath();
    public static OnEnemyDeath enemyDeath;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rb;

    private Dropper _dropper;

    

    private void Start()
    {
        // Set private variables to values in scriptable object
        _health = enemy.health;
        _damage = enemy.damage;
        _attackSpeed = enemy.attackSpeed;
        _predictedHealth = _health;

        name = enemy.name;
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();

        _dropper = gameObject.AddComponent(typeof(Dropper)) as Dropper;
        if (_dropper)
            _dropper.DropperConstructor(enemy.drops, enemy.dropsChance);
        else
        {
            print("Uh oh");
        }

    }

    private void Update()
    {
        CheckDeath();
    }

    public void Damage(float damage)
    {
        // Damaging the enemy
        _health -= damage;
        StartCoroutine(DamageFlash(damageFlash, flashTimeMilis));
    }

    public void Knockback(float knockback, Vector3 direction)
    {
        _rb.AddForce(knockback * direction.normalized, ForceMode.Impulse);
    }
    
    private void CheckDeath()
    {
        // Check if current health is lower than or equal to zero
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Enemy Death effect
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        
        // Enemy death sound effect
        
        // Invoke OnEnemyDeath delegate
        enemyDeath?.Invoke();
        
        // Drop All Items
        _dropper.DropAll(transform.position, Quaternion.identity, 0);
        
        // Destroy enemy
        Destroy(gameObject);
    }

    /// <summary>
    /// Flashed the mesh to the specified material for the seconds specified by timeOn
    /// </summary>
    /// <param name="material">The material to flash</param>
    /// <param name="timeOn">In milliseconds</param>
    /// <returns></returns>
    private IEnumerator DamageFlash(Material material, float timeOn)
    {
        Material beforeMaterial = _meshRenderer.material;
        _meshRenderer.material = material;
        
        yield return new WaitForSeconds(timeOn / 1000);

        _meshRenderer.material = beforeMaterial;
    }

}
