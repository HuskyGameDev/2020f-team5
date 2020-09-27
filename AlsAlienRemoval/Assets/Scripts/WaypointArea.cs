﻿using UnityEngine;

public class WaypointArea : MonoBehaviour
{
    public GameObject waypointGameObject;   // Waypoint child gameobject which enemies in area move towards. Assigned in editor
    private Vector2 waypointPosition;       // Position of waypoint in world space
    public bool isLastWaypoint;             // Waypoint area is last in area. Enemies will be destroyed upon exiting

    private void Start()
    {
        // Get poistion of waypoint converted from local space to world space
        waypointPosition = waypointGameObject.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Set this waypoint area as enemy's next destination
        if (enemy != null)
        {
            setAsNextDestination(enemy);
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
                GameObject.Destroy(enemy.gameObject);
            }
        }
    }

    public void setAsNextDestination(Enemy enemy)
    {
        // Assign this waypoint as enemy's next destination
        enemy.nextDestination = waypointPosition;

        // Set enemy's current destination as well if it does not have one (first waypoint in level, etc.) as indicated by zero vector
        if (enemy.destination == Vector2.zero)
        {
            enemy.destination = waypointPosition;
        }
    }
}
