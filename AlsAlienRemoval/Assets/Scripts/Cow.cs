using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{
    public Level level;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Set this waypoint area as enemy's next destination
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            level.loseLife();
            Destroy(gameObject);
        }
    }
}
