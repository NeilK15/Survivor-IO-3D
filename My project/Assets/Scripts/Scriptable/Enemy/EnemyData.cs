using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{

    [Header("Enemy Data Settings")] 
    public new string name = "Enemy";
    public float health = 100f;
    public float damage = 20f;
    public float speed = 5f;
    public float attackSpeed = 10f;

    [Space(10)]
    public GemData[] drops;
    public float[] dropsChance;

}
