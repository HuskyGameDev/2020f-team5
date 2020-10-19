using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buildingPlaceable : MonoBehaviour
{
    [HideInInspector]
    public List<Collider2D> colliders = new List<Collider2D>();
     public SpriteRenderer rangeSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
      rangeSpriteRenderer = GetComponent<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D c)
        {
        if(c.tag == "building")
            {
            colliders.Add(c);
        }
    }
     void OnTriggerExit2D(Collider2D c)
        {
        if(c.tag == "building")
            {
            colliders.Remove(c);
        }
    }

    void hasPlaced(bool hasPlaced)
    {
        this.SendMessage("placed", true);
    }
    public void Select()
        {

    }
}
