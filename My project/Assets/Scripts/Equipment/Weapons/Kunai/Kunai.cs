using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : TargetedWeapon
{
    [Header("Kunai Specific Data")] public GameObject kunai;

    public AudioClip attackClip;


    protected override bool Attack(Collider enemy)
    {
        // Predict what the enemy's health will be after kunai is fired

        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        GameObject projectile = Instantiate(kunai, transform.position, Quaternion.identity);
        KunaiProjectile projectileScript = projectile.GetComponent<KunaiProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Target = enemy.transform;
        }

        attackAudio.PlayOneShot(attackClip);

        return false;
    }
}