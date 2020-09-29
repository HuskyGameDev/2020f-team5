using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public WaypointArea firstWaypointArea;  // First waypoint in path. Default destination for enemies
    public GameObject spawnpoint;           // Spawn point for enemies
    public Enemy debugEnemy;

    public bool doDebugSpawning;            // Spawn random enemies for debug purposes
    public Enemy debugEnemy1;
    public Enemy debugEnemy2;
    public Enemy debugEnemy3;

    private Enemy[] _debugEnemies;
    private float _spawnTimer = 0f;         // Time since last spawn (debug)
    private float _spawnCooldown = .25f;    // Time required between spawning enemies (debug)

    private void Start()
    {
        _debugEnemies = new Enemy[] { debugEnemy1, debugEnemy2, debugEnemy3 };
    }
    public void spawnEnemy (Enemy enemyType)
    {
        // Create new enemy of specified type at spawnpoint and set its destination to first waypoint
        Enemy newEnemy = GameObject.Instantiate(enemyType, spawnpoint.transform.position, Quaternion.identity);

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
                int rand = (int)Random.Range(0f, 2.99f);
                Debug.Log(rand);
                spawnEnemy(_debugEnemies[rand]);
                _spawnTimer %= _spawnCooldown;
            }
        }
    }
}
