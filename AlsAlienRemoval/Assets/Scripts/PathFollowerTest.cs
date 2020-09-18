using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerTest : MonoBehaviour
{
    public float speed;
    public Vector2 destination;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed);
    }
}
