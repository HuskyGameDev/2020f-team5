using UnityEngine;
using UnityEngine.SceneManagement;

public class Cow : MonoBehaviour {

    public float timeUntilBehaviorChange;       // time until cow's movement behavior may change
    public Vector2 destination;                 // current destination of the cow
    public float speed;                         // current speed of this cow
    private bool active;                        // is this cow moving?

    // starts random movement
    void Start(){
        timeUntilBehaviorChange = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider) {

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

                if (Level.LivestockRemaining <= 0)
                {
                    SceneManager.LoadScene("Lose Screen");
                }

                // todo: add a horrific bloody explosion
            }
        }
    }

    // makes each cow randomly move around in the pen
    public void RandomMovement() {

        // time to do somthing new
        if (timeUntilBehaviorChange <= 0) {

            timeUntilBehaviorChange = Random.Range(4f,10f);

            // 70% chance to just stand there (like a cow)
            if (Random.Range(0f, 1f) < 0.7f) {
                active = false;
            }

            // otherwise, calculate next random direction to move and speed to get there in time
            else {
                active = true;
                destination.x = Random.Range(Level.cowPenXmin, Level.cowPenXmax);
                destination.y = Random.Range(Level.cowPenYmin, Level.cowPenYmax);
                if (destination.x > transform.position.x)
                    GetComponent<SpriteRenderer>().flipX = true;
                else
                    GetComponent<SpriteRenderer>().flipX = false;

                speed = Mathf.Sqrt(Mathf.Pow(destination.x-transform.position.x, 2) + Mathf.Pow(destination.y-transform.position.y, 2)) / timeUntilBehaviorChange;
            }
        }
        
        // actual movement
        if (active) {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        }

        timeUntilBehaviorChange -= Time.deltaTime;
    }
}
