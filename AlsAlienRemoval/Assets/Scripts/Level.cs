using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Level : MonoBehaviour
{
    public WaypointArea firstWaypointArea;  // First waypoint in path. Default destination for enemies
    public GameObject spawnpoint;           // Spawn point for enemies

    public bool showWaypoints;              // Show red dots at waypoint locations
    public bool doDebugSpawning;            // Spawn random enemies for debug purposes
    public Enemy debugEnemy1;               // Pool of enemies for random spawning
    public Enemy debugEnemy2;
    public Enemy debugEnemy3;
    public Cow cow;

    private Enemy[] _debugEnemies;
    private float _spawnTimer = 0f;         // Time since last spawn (debug)
    private float _spawnCooldown = .25f;    // Time required between spawning enemies (debug)

    public int lives;                     // Number of player lives/cows

    private void Start()
    {
        if (!showWaypoints)
        {
            Destroy(spawnpoint.GetComponent<SpriteRenderer>());
        }

        Text livestock = GameObject.Find("Canvas/MainUIPanel/LivestockDisplay").GetComponent<Text>();
        livestock.text = lives.ToString();

        _debugEnemies = new Enemy[] { debugEnemy1, debugEnemy2, debugEnemy3 };
    }
    public void spawnEnemy (Enemy enemyType)
    {
        // Create new enemy of specified type at spawnpoint and set its destination to first waypoint
        Enemy newEnemy = GameObject.Instantiate(enemyType, spawnpoint.transform.position, Quaternion.identity);

        firstWaypointArea.setAsNextDestination(newEnemy);
    }

    public void loseLife()
    {
        lives--;
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
