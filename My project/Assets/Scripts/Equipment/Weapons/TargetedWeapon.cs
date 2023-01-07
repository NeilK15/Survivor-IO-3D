using UnityEngine;

public class TargetedWeapon : Weapon
{
    [Header("Targeted Weapon Data")] public TargetedWeaponData weaponData;
    
    
    private void Start()
    {
        PlayerController.EnemiesInRange += Attack;
    }
    
    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    private void Attack(Collider[] enemies)
    {
        
        Collider enemy = DecideEnemy(enemies);
                
        if (enemy)
        {
            // If enemy was found, fire targeted at it
            FireTargeted(enemy);
        }
    }

    protected virtual bool Attack(Collider enemy)
    {
        
        return false;
    }
    
    private float _timeSinceLastShot = 0f;
    
    private bool CanShoot() => _timeSinceLastShot > 1f / (weaponData.attackSpeed / 60f);

    private bool FireTargeted(Collider enemy)
    {

        if (CanShoot())
        {
            _timeSinceLastShot = 0;
            return Attack(enemy);
        }

        return false;
    }

    Collider DecideEnemy (Collider[] enemies)
    {
        Collider bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Collider potentialTarget in enemies)
        {
            // Math for detecting closest enemy
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
    
    
    
