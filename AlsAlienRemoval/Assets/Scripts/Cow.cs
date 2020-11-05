using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cow : MonoBehaviour
{
    public Level level;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Destroy enemy and cow and subtract life if colliding enemy is this cow's attacker
        if (enemy != null)
        {
            if (enemy.destination == transform.position)
            {
                // Subtract life
                level.loseLife();
                Text livestock = GameObject.Find("Canvas/MainUIPanel/LivestockDisplay").GetComponent<Text>();
                livestock.text = level.lives.ToString();

                // Destroy attacking enemy and itself
                Destroy(enemy.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
