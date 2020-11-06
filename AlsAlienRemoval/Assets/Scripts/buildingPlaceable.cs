using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buildingPlaceable : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer rangeSpriteRenderer;
    private int _triggerCount;  // Number of triggers this building is currently intersecting 
    public GameObject range;
    public int cost = 100;
    private SpriteRenderer _tileHighlight;

    void Start()
    {
        cost = 100;
        rangeSpriteRenderer = GetComponent<SpriteRenderer>();  
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        _triggerCount++;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        _triggerCount--;
    }
    public int getCost()
    {
        return cost;
    }
    // Return true if collider is not currently intersecting any triggers (path, other towers, etc.)
    public bool IsLegalPosition()
    {
        if (_triggerCount == 0)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    void hasPlaced(bool hasPlaced)
    {
        this.SendMessage("placed", true);
    }
    void type(int type)
    {
        this.SendMessage("setTowerType", type);
    }
    public void Select()
        {

    }
}
