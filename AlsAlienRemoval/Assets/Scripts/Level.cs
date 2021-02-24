using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
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
    private Text _waveNumber;               // Wave number text
    private Text _enemiesRemaining;         // Enemies remaining text
    private Text _livestockRemaining;       // Livestock remaining text
    public WaveTimer waveTimer;

    // floating-error-text stuff
    public static int errorTextCount = 0;               // tracks # of error texts (too many = lag)
    public const int ERROR_TEXT_LIMIT = 10;             // maximum error texts can exist
    public static FloatingErrorText[] errorTextList;    // list of error-text wrapper objects
    public static int nextErrIndex = 0;                 // index of next available text

    private Button _utilityButton;

    // runs once
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
        _utilityButton = GameObject.Find("btn_utility").GetComponent<Button>();
        waveTimer.enabled = false;
        _waveInProgress = false;

        // hides floating error text
        GameObject errorTextObject = GameObject.Find("text_floating_error");
        errorTextObject.GetComponent<CanvasGroup>().alpha = 0;

        // fills error-text object array with wrapper objects by cloning the error-text from the scene
        errorTextList = new FloatingErrorText[ERROR_TEXT_LIMIT];
        for (int i = 0; i < ERROR_TEXT_LIMIT; i++) {
            GameObject newText = Object.Instantiate(errorTextObject, errorTextObject.transform, true);
            errorTextList[i] = new FloatingErrorText(newText);
        }
        
        // Play music
        GetComponent<AudioSource>().Play();
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

    private void Update() {
        // updates location of all active floating error-texts
        foreach (FloatingErrorText text in errorTextList) {
            if (text.IsActive()) {
                text.UpdateFadeAndLocation();
            }
        }
    }

    // updates independant of fps
    private void FixedUpdate() {
        
        _waveNumber.text = WaveNumber.ToString();
        _enemiesRemaining.text = EnemiesRemaining.ToString();
        _livestockRemaining.text = LivestockRemaining.ToString();

        // spawns wave enemies if "debug spawning" is enabled
        if (doDebugSpawning) {

            // fills spawn if timer is completed and spawning still needed
            if (_spawningInProgress && !waveTimer.enabled) {
                SpawnWaveChunk();
            }

            // wave cleared, enable button for timer starting
            if (!waveTimer.enabled && _waveInProgress && EnemiesRemaining == 0) {
                _utilityButton.interactable = true;
                _waveInProgress = false;
                Debug.Log($"WAVE {WaveNumber} DEFEATED");
                if (WaveNumber == 10)
                {
                    SceneManager.LoadScene("Win Screen");
                }

                WaveNumber++;
            }
            
        }
    }

    public void startWave()
    {
        if (!_waveInProgress)
        {
            waveTimer.enabled = true;
            _utilityButton.interactable = false;

            _waveInProgress = true;
            _spawningInProgress = true;
            _strengthLeftToSpawn = _waveStrengths[WaveNumber - 1];

            Debug.Log($"WAVE {WaveNumber} BEGINS");
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
            newCow.transform.position = lastWaypointArea.transform.TransformPoint(new Vector2(Random.Range(-(areaWidth - 1) / 2, (areaWidth - 1) / 2), Random.Range(-(areaHeight - 1) / 2, (areaHeight - 1) / 2 - 1)));

            // Add new cow to list
            cowList.Add(newCow);
        }
    }

    // wrapper class used to manage the floating-error-text objects
    public class FloatingErrorText {
        private GameObject errorTextObject;     // reference to an instantiated text object from scene
        private float fadeTime = 1f;            // seconds text will be visible
        private float fadeDistance = 0.7f;      // distance text travels
        private float timeRemaining;            // time until text is hidden
        private bool isActive;

        // constructor
        public FloatingErrorText(GameObject errorTextObject) {
            this.errorTextObject = errorTextObject;
        }

        // is the text currently shown
        public bool IsActive() {
            return isActive;
        }

        // sets the text of the object
        public void SetText(string text) {
            errorTextObject.GetComponent<Text>().text = text;
        }

        // hides the text of the object
        public void HideText() {
            errorTextObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        
        // resets the starting location and timer for the object
        public void SetupFadeCycle(Vector2 location) {
            timeRemaining = fadeTime;
            errorTextObject.GetComponent<Text>().transform.position = location;
            errorTextObject.GetComponent<CanvasGroup>().alpha = 1;
            isActive = true;
        }

        // repeatidly updates the fade and location of the object
        public void UpdateFadeAndLocation() {
            
            // updates alpha
            float currentAlpha = errorTextObject.GetComponent<CanvasGroup>().alpha;
            float newAlpha = Math.Max(0, currentAlpha - (Time.deltaTime / fadeTime));
            errorTextObject.GetComponent<CanvasGroup>().alpha = newAlpha;

            // updates position
            float currentY = errorTextObject.GetComponent<Text>().transform.position.y;
            float newY = currentY + ((Time.deltaTime / fadeTime) * fadeDistance);
            Vector2 newPosition = new Vector2(errorTextObject.GetComponent<Text>().transform.position.x, newY);
            errorTextObject.GetComponent<Text>().transform.position = newPosition;
            
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0) {
                HideText();
                isActive = false;
                errorTextCount--;
            }
        }
    }
}
