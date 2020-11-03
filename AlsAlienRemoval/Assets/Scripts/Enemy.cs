using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;                             // Speed in units/second
    public Vector2 destination = Vector2.zero;      // Current point enemy is moving towards in world space
    public Vector2 nextDestination = Vector2.zero;  // Next point enemy will move to. Assigned when new waypoint area is entered, and set to destination when previous area is left
    public float health;
    public int moneyDropped;

    void Hit(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Currency.addCurrency(moneyDropped);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
    }
}
