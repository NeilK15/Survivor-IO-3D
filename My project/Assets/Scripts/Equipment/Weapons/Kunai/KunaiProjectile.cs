using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiProjectile : MonoBehaviour
{
    
    public TargetedWeaponData weaponData;
    public float lifeSpan;

    private Rigidbody _rb;
    
    private Transform _target;

    [HideInInspector]
    public Transform Target
    {
        get
        {
            return _target;
        }

        set
        {
            _target = value;
        }
    }


    private void Start()
    {
        //StartCoroutine(DieAfterTime(lifeSpan));
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_target == null)
            Die();
        else
        {
            MoveTowardsTarget();    
            StartCoroutine(Predict());
        }
    }

    IEnumerator Predict()
    {
        Vector3 prediction = transform.position + _rb.velocity * Time.fixedDeltaTime;

        RaycastHit hit2;
        int layerMask = ~ LayerMask.GetMask("Bullet");
        Debug.DrawLine(transform.position, prediction);

        if (Physics.Linecast(transform.position, prediction, out hit2, layerMask))
        {
            transform.position = hit2.point;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            _rb.isKinematic = true;
            yield return 0;
            OnCollisionEnter(hit2.collider);
        }
        
    }

    private void MoveTowardsTarget()
    {
        // Moving towards enemy
        _rb.AddForce((_target.position - transform.position).normalized * weaponData.projectileSpeed, ForceMode.VelocityChange);

        transform.rotation = Quaternion.LookRotation(_target.position - transform.position);

    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnter(collision.collider);
    }

    private void OnCollisionEnter(Collider collider)
    {
        IDamageable enemy = collider.GetComponent<IDamageable>();
        if (enemy != null)
        {
            enemy.Knockback(weaponData.knockback, Vector3.Scale(collider.transform.position - transform.position, new Vector3(1,0,1)));
            enemy.Damage(weaponData.damage);
        }

        Die();
    }

    IEnumerator DieAfterTime(float time)
    {

        yield return new WaitForSeconds(time);
        Die();

    }

    private void Die()
    {
        // Play death effect
        
        Destroy(gameObject);
        
    }
}
