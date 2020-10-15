using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTriggerTest : MonoBehaviour
{
    private SpriteRenderer _tileHighlight;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        _tileHighlight.color = Color.red;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _tileHighlight.color = Color.green;
    }
}
