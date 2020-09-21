using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTriggerTest : MonoBehaviour
{
    private SpriteRenderer _tileHighlight;

    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();   // All spriterenderers in this gameobject and children

        // Find spriterenderer of tilehighlight (spriterenderer belonging to only child of this gameobject)
        foreach (SpriteRenderer s in spriteRenderers)
        {
            if (s.gameObject.transform.parent == this.transform)
            {
                _tileHighlight = s;
                break;
            } 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // _tileHighlight.transform.position = new Vector2(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y - 1));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _tileHighlight.color = Color.red;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _tileHighlight.color = Color.green;
    }
}
