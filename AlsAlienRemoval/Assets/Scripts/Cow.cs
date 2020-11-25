using UnityEngine;

public class Cow : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();   // Enemy script of other collider that entered. Null if other collider does not belong to an enemy (Tower, etc.)

        // Destroy enemy and cow and subtract life if colliding enemy is this cow's designated attacker
        if (enemy != null)
        {
            if (enemy.destination == transform.position)
            {
                // Destroy attacking enemy and itself, remove a life, and mark attacking enemy as dead
                Destroy(enemy.gameObject);
                Destroy(gameObject);
                Level.EnemiesRemaining--;
                Level.LivestockRemaining--;

                // todo: add a horrific bloody explosion
            }
        }
    }
}
