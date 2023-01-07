using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{

    #region SingletonPattern

    public static Spawner Instance;
    
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
    
    [Header("Preferences")]
    public float maxSpawnDistance = 20f;
    
    public float minSpawnDistance = 10f;
    
    [Tooltip("Enemies per minute")]
    public float spawnRate = 200f;
    
    public GameObject enemy;

    public LayerMask groundMask;

    public float groundCheckDistance = 200f;
    
    public float maxYValue = 500f;

    public float spawnDistanceFromGround = 1f;

    [Header("Enemy Cap")] [Tooltip("Suggested at 100 for best performance")]
    public int enemyCap = 100;

    [FormerlySerializedAs("_numEnemies")]
    [Header("Current Number of Enemies")]
    [SerializeField]
    private int numEnemies = 0;

    private int _totalSpawnedEnemies = 0;
    

    private float _timeSinceLastSpawn = 0f;

    private bool CanSpawn() => _timeSinceLastSpawn > 1f / (spawnRate / 60f) && numEnemies < enemyCap;

    private void Start()
    {
        EnemyController.enemyDeath += () => numEnemies--;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawn())
        {
            float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized * spawnDistance + transform.position + Vector3.up * maxYValue;

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, maxYValue + groundCheckDistance, groundMask))
            {
                spawnPosition.y = hit.point.y + spawnDistanceFromGround;
                
                Vector3 target = spawnPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(spawnPosition);
                GameObject enemyObj = Instantiate(enemy, spawnPosition, rotation);
                enemyObj.name = "Enemy_" + (_totalSpawnedEnemies + 1);
                _timeSinceLastSpawn = 0f;
                numEnemies++;
                _totalSpawnedEnemies++;
            }
            

        }
        
        _timeSinceLastSpawn += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
    }
}
