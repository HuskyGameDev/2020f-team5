using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cow : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Destroy enemy and cow and subtract life if colliding enemy is this cow's attacker
        if (enemy != null)
        {
            if (enemy.destination == transform.position)
            {
                // Destroy attacking enemy and itself, and remove a life
                Destroy(enemy.gameObject);
                Destroy(gameObject);
                Level.EnemiesRemaining--;
                Level.LivestockRemaining--;

                // todo: add a horrific bloody explosion
            }
        }
    }
}
