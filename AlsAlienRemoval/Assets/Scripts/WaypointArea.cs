using UnityEngine;
using System.Collections.Generic;

public class WaypointArea : MonoBehaviour
{
    public GameObject waypointGameObject;   // Waypoint child gameobject which enemies in area move towards. Assigned in editor
    private Vector3 waypointPosition;       // Position of waypoint in world space
    public bool isLastWaypoint;             // Waypoint area is last in area. Enemies will be destroyed upon exiting

    private List<Cow> _cowList;      // List of cows on the final waypoint

    private void Awake()
    {
        // Get reference to game "Level" object
        Level level = GetComponentInParent<Level>();

        // Destroy waypoint sprite if hidden by level
        if (!Level.showWaypoints)
        {
            Destroy(GetComponentInChildren<SpriteRenderer>());
        }

        // Get poistion of waypoint converted from local space to world space
        waypointPosition = waypointGameObject.transform.position;

        if (isLastWaypoint)
        {
            _cowList = new List<Cow>();

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            float areaWidth = collider.size.x;
            float areaHeight = collider.size.y;

            for (int i = 0; i < Level.LivestockRemaining; i++)
            {
                // Create a cow prefab at a random location
                Cow newCow = Instantiate(level.Cow);
                //newCow.transform.parent = transform;
                newCow.transform.position = transform.TransformPoint(new Vector2(Random.Range(-(areaWidth - 1) / 2, (areaWidth - 1) / 2), Random.Range(-(areaHeight - 1) / 2, (areaHeight - 1) / 2)));
                _cowList.Add(newCow);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Set this waypoint area as enemy's next destination, or a random cow if this is the last waypoint
        if (enemy != null)
        {
            if (isLastWaypoint)
            {
                // Set enemy destination to first cow in list and remove it.
                Cow cowTarget = _cowList[0];
                _cowList.Remove(cowTarget);
                setAsNextDestination(enemy, cowTarget.transform.position);
            } 
            else
            {
                setAsNextDestination(enemy);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that exited. Null if other collider does not belong to an enemy (Tower, etc.)

        if (enemy != null)
        {
            // Make enemy pursue its next destination if it was successfully assigned one before leaving
            if (enemy.nextDestination != waypointPosition)
            {
                enemy.destination = enemy.nextDestination;
            }
            else if (isLastWaypoint)
            {
                // Destroy enemy if this was the last waypoint
                Destroy(enemy.gameObject);
            }
        }
    }

    public void setAsNextDestination(Enemy enemy, Vector2 dest)
    {
        // Assign this waypoint as enemy's next destination
        enemy.nextDestination = dest;

        // Set enemy's current destination as well if it does not have one (first waypoint in level, etc.) as indicated by zero vector
        if (enemy.destination == Vector3.zero)
        {
            enemy.destination = dest;
        }
    }

    public void setAsNextDestination(Enemy enemy)
    {
        setAsNextDestination(enemy, waypointPosition);
    }
    
}
