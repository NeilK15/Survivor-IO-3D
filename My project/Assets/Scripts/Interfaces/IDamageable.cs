using UnityEngine;

// Interface for what is damageable
public interface IDamageable
{

    // Method for damaging
    public void Damage(float damage);
    
    // Method for knockback
    public void Knockback(float knockback, Vector3 direction);

}
