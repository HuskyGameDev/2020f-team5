using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public WaypointArea firstWaypointArea;  // First waypoint in path. Default destination for enemies
    public GameObject spawnpoint;           // Spawn point for enemies
    public Enemy debugEnemy;

    public bool doDebugSpawning;            // Spawn random enemies for debug purposes

    private float _spawnTimer = 0f;         // Time since last spawn (debug)
    private float _spawnCooldown = .25f;    // Time required between spawning enemies (debug)

    public void spawnEnemy (Enemy enemyType)
    {
        // Create new enemy of specified type at spawnpoint and set its destination to first waypoint
        Enemy newEnemy = GameObject.Instantiate(enemyType, spawnpoint.transform.position, Quaternion.identity);

        // FOR NOW: Give enemy random, but proportional speed and size stats
        float enemyScalar = Random.Range(1f, 4f);   // Speed/size scale
        newEnemy.speed *= enemyScalar;
        newEnemy.transform.localScale *= (5f - enemyScalar);
        firstWaypointArea.setAsNextDestination(newEnemy);
    }

    private void FixedUpdate()
    {
        // Spawn random enemies is debug spawning enabled
        if (doDebugSpawning)
        {
            _spawnTimer += Time.fixedDeltaTime;

            if (_spawnTimer >= _spawnCooldown)
            {
                spawnEnemy(debugEnemy);
                _spawnTimer %= _spawnCooldown;
            }
        }
    }
}
