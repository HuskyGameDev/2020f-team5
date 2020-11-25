using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class Level : MonoBehaviour
{
    public WaypointArea firstWaypointArea;  // First waypoint in path. Default destination for enemies
    public WaypointArea lastWaypointArea;   // Last waypoint in path. Contains cows
    public GameObject spawnpoint;           // Spawn point for enemies

    public static bool showWaypoints;       // Show red dots at waypoint locations
    public bool doDebugSpawning;            // Spawn random enemies for debug purposes
    public Enemy DebugEnemy1;               // Pool of enemies for random spawning
    public Enemy DebugEnemy2;
    public Enemy DebugEnemy3;
    private Enemy[] _debugEnemies;

    // Cow/livestock visual representation variables
    public Cow cowPrefab;                  // Cow prefab for spawning. Assigned in editor
    public static List<Cow> cowList;       // List of cows on the final waypoint

    // wave variables
    private int[] _enemyCosts;
    private int[] _waveStrengths;
    bool waveInProgress;

    // public game status variables
    public static int WaveNumber;               // global wave #
    public static int EnemiesRemaining;         // global enemies remaining
    public static int LivestockRemaining;       // global livestock remaining

    // private status # ui elements
    private Text _waveNumber;                   // Wave number text
    private Text _enemiesRemaining;             // Enemies remaining text
    private Text _livestockRemaining;           // Livestock remaining text
    private WaveTimer _waveTimer;

    private void Start() {

        if (!showWaypoints) {
            Destroy(spawnpoint.GetComponent<SpriteRenderer>());
        }
        
        // ASSIGNS GAME SETTINGS
        _debugEnemies = new Enemy[] { DebugEnemy1, DebugEnemy2, DebugEnemy3 };
        _enemyCosts = new int[] { 5, 10, 20 };
        _waveStrengths = new int[] { 100, 150, 225, 335, 500, 750, 1125, 1700, 2500, 5000 };
        WaveNumber = 1;
        EnemiesRemaining = 0;
        LivestockRemaining = 20;

        // gets UI text elements so they can be updated
        _waveNumber = GameObject.Find("wave_number").GetComponent<Text>();
        _enemiesRemaining = GameObject.Find("enemies_remaining").GetComponent<Text>();
        _livestockRemaining = GameObject.Find("livestock_remaining").GetComponent<Text>();

        // Mark final waypoint and spawn cows in it
        lastWaypointArea.isLastWaypoint = true;
        spawnCows();

        // timer crap
        GameObject MainUIPanel = GameObject.Find("MainUIPanel");
        _waveTimer = MainUIPanel.AddComponent<WaveTimer>();
        _waveTimer.enabled = false;
        waveInProgress = false;

    }

    public void spawnEnemy (Enemy enemyType) {

        // Create new enemy of specified type at spawnpoint and set its destination to first waypoint
        Enemy newEnemy = GameObject.Instantiate(enemyType, spawnpoint.transform.position, Quaternion.identity);

        firstWaypointArea.setAsNextDestination(newEnemy);
    }

    // updates independant of fps
    private void FixedUpdate() {

        _waveNumber.text = WaveNumber.ToString();
        _enemiesRemaining.text = EnemiesRemaining.ToString();
        _livestockRemaining.text = LivestockRemaining.ToString();

        // Spawn random enemies if "debug spawning" is enabled
        if (doDebugSpawning) {

            if (!waveInProgress) {
                waveInProgress = true;
                SpawnWave(WaveNumber);
            }

            if (!_waveTimer.enabled && waveInProgress && EnemiesRemaining == 0) {
                _waveTimer.enabled = true;
                Debug.Log($"WAVE {WaveNumber} DEFEATED");
            }

            if (_waveTimer.enabled && _waveTimer.timeRemaining <= 0) {
                WaveNumber++;
                Debug.Log($"WAVE {WaveNumber} BEGINS");
                _waveTimer.enabled = false;
                waveInProgress = false;
            }

        }
    }

    // spwans the given wave number (waves are indexed from 1)
    private void SpawnWave(int waveNumber) {

        int totalWaveStrength = _waveStrengths[waveNumber - 1];

        while (totalWaveStrength > 0) {

            // determines enemy, subtracts cost, spawns enemy
            int rand = (int)Random.Range(0f, 2.99f);
            Debug.Log($"Spawning random enemy: {rand}, subtracting cost: {_enemyCosts[rand]}");
            totalWaveStrength -= _enemyCosts[rand];
            spawnEnemy(_debugEnemies[rand]);
            EnemiesRemaining++;
        }

        Debug.Log($"All enemies for wave {waveNumber} have spawned");
    }

    // Spawn herd of cows in final waypoint to represent remaining livestock
    private void spawnCows()
    {
        // Create list of remaining cows
        cowList = new List<Cow>();

        // Get collider/bounds of last waypoint area
        BoxCollider2D collider = lastWaypointArea.GetComponent<BoxCollider2D>();
        float areaWidth = collider.size.x;
        float areaHeight = collider.size.y;

        // Spawn cows at randomized locations in final waypoint area
        for (int i = 0; i < LivestockRemaining; i++)
        {
            // Create a cow prefab at a random location
            Cow newCow = Instantiate(cowPrefab);
            newCow.transform.position = lastWaypointArea.transform.TransformPoint(new Vector2(Random.Range(-(areaWidth - 1) / 2, (areaWidth - 1) / 2), Random.Range(-(areaHeight - 1) / 2, (areaHeight - 1) / 2)));

            // Add new cow to list
            cowList.Add(newCow);
        }
    }

    // timer class for displaying the time until the next wave
    private class WaveTimer : MonoBehaviour {

        Text _timerText;                    // timer text on the main ui panel
        Text _labelText;                    // label text "Next wave:"
        public float timeRemaining;         // time remaining on this timer
        
        private void Awake() {
            
            // retrieves timer crap
            _timerText = GameObject.Find("next_wave_timer").GetComponent<Text>();
            _labelText = GameObject.Find("next_wave_timer_label").GetComponent<Text>();
            _timerText.enabled = false;
            _labelText.enabled = false;
        }

        // enables the timer text, sets to 10s
        void OnEnable() {

            // todo: make this value dynamic
            timeRemaining = 10;

            _timerText.enabled = true;
            _labelText.enabled = true;
        }

        // disables the timer tex
        void OnDisable() {
            _timerText.enabled = false;
            _labelText.enabled = false;
        }

        // updates time text. self-disables when it reaches 0
        void FixedUpdate() {
            if (timeRemaining > 0) {
                timeRemaining -= Time.fixedDeltaTime;
                if (timeRemaining < 0)
                    timeRemaining = 0;
                _timerText.text = timeRemaining.ToString();
            }
            else {
                this.enabled = false;
            }
        }
    }
}
