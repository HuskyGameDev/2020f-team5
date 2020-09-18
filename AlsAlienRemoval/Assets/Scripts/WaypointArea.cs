using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PathFollowerTest follower = other.GetComponent<PathFollowerTest>();
        if (follower != null)
        {
            follower.destination = this.transform.position;
        }
    }
}
