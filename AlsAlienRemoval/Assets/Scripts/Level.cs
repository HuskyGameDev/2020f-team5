using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour
{
    // public game status variables
    public static int WaveNumber;               // global wave #
    public static int EnemiesRemaining;         // global enemies remaining
    public static int LivestockRemaining;       // global livestock remaining
    public static int enemiesInSpawn;           // # of enemies within the spawn location

    public WaypointArea lastWaypointArea;   // Last waypoint in path. Contains cows
    public WaypointArea spawnArea;           // Spawn point for enemies

    public static bool showWaypoints;       // Show red dots at waypoint locations
    public bool doDebugSpawning;            // Spawn random enemies for debug purposes
    public Enemy DebugEnemy1;               // Pool of enemies for random spawning
    public Enemy DebugEnemy2;
    public Enemy DebugEnemy3;
    private Enemy[] _debugEnemies;

    // Cow / livestock visual representation variables
    public Cow cowPrefab;                  // Cow prefab for spawning. Assigned in editor
    public static List<Cow> cowList;       // List of cows on the final waypoint

    // spawning / wave variables
    private int[] _enemyCosts;
    private int[] _waveStrengths;
    private int _strengthLeftToSpawn;
    private bool _waveInProgress;
    private bool _spawningInProgress;
    private int _enemiesInSpawnLimit;

    // private status # ui elements
    private Text _waveNumber;                   // Wave number text
    private Text _enemiesRemaining;             // Enemies remaining text
    private Text _livestockRemaining;           // Livestock remaining text
    private WaveTimer _waveTimer;

    private void Start() {

        // ASSIGNS GAME SETTINGS
        _debugEnemies = new Enemy[] { DebugEnemy1, DebugEnemy2, DebugEnemy3 };
        _enemyCosts = new int[] { 5, 10, 20 };
        _waveStrengths = new int[] { 100, 150, 225, 335, 500, 750, 1125, 1700, 2500, 5000 };
        enemiesInSpawn = 0;
        _enemiesInSpawnLimit = 30;
        WaveNumber = 1;
        EnemiesRemaining = 0;
        LivestockRemaining = 20;

        // gets UI text elements so they can be updated
        spawnArea = GameObject.Find("WaypointAreaSpawn").GetComponent<WaypointArea>();
        _waveNumber = GameObject.Find("wave_number").GetComponent<Text>();
        _enemiesRemaining = GameObject.Find("enemies_remaining").GetComponent<Text>();
        _livestockRemaining = GameObject.Find("livestock_remaining").GetComponent<Text>();

        // marks first and last waypoints then spawns cows
        //spawnArea.isSpawnPoint = true;
        lastWaypointArea.isLastWaypoint = true;
        spawnCows();

        // timer stuff
        _waveTimer = GameObject.Find("MainUIPanel").AddComponent<WaveTimer>();
        _waveTimer.enabled = false;
        _waveInProgress = false;
    }

    // Create new enemy of specified type at the spawn area
    public void spawnEnemy (Enemy enemyType) {

        // Get collider/bounds of spawn area
        BoxCollider2D collider = GameObject.Find("WaypointAreaSpawn").GetComponent<BoxCollider2D>();
        float areaWidth = collider.size.x;
        float areaHeight = collider.size.y;

        // spawns enemy at random location within the spawn area
        enemiesInSpawn++;
        Vector3 randomSpawnPoint = spawnArea.transform.TransformPoint(new Vector2(
            Random.Range(-(areaWidth - 1) / 2, (areaWidth - 1) / 2), 
            Random.Range(-(areaHeight - 1) / 2, (areaHeight - 1) / 2)));
        Instantiate(enemyType, randomSpawnPoint, Quaternion.identity);
    }

    // updates independant of fps
    private void FixedUpdate() {

        _waveNumber.text = WaveNumber.ToString();
        _enemiesRemaining.text = EnemiesRemaining.ToString();
        _livestockRemaining.text = LivestockRemaining.ToString();

        // spawns wave enemies if "debug spawning" is enabled
        if (doDebugSpawning) {

            // begins the wave
            if (!_waveInProgress) {
                _waveInProgress = true;
                _spawningInProgress = true;
                _strengthLeftToSpawn = _waveStrengths[WaveNumber - 1];
            }

            // fills spawn
            if (_spawningInProgress) {
                SpawnWaveChunk();
            }

            // wave cleared, start timer
            if (!_waveTimer.enabled && _waveInProgress && EnemiesRemaining == 0) {
                _waveTimer.enabled = true;
                Debug.Log($"WAVE {WaveNumber} DEFEATED");
                if (WaveNumber == 10)
                {
                    SceneManager.LoadScene("Win Screen");
                }
            }

            // timer reaches 0, spawn next wave
            if (_waveTimer.enabled && _waveTimer.timeRemaining <= 0) {
                WaveNumber++;
                Debug.Log($"WAVE {WaveNumber} BEGINS");
                _waveTimer.enabled = false;
                _waveInProgress = false;
            }

        }
    }

    // spawns as many enemies from the current wave that will fit in spawn (waves are indexed from 1)
    private void SpawnWaveChunk() {
        
        // continually spawns enemies until the wave strength is depleted
        // limits the number of enemies that can be in spawn
        while (_strengthLeftToSpawn > 0 && enemiesInSpawn < _enemiesInSpawnLimit) {

            // determines enemy, subtracts cost, spawns enemy
            int rand = (int)Random.Range(0f, 2.99f);
            _strengthLeftToSpawn -= _enemyCosts[rand];
            spawnEnemy(_debugEnemies[rand]);
            EnemiesRemaining++;
        }

        // spawning complete
        if (_strengthLeftToSpawn <= 0)
            _spawningInProgress = false;

        Debug.Log($"All enemies for wave {WaveNumber} have spawned");
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
            timeRemaining = 5;

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
                _timerText.text = timeRemaining.ToString("0.0");
            }
            else {
                this.enabled = false;
            }
        }
    }
}
