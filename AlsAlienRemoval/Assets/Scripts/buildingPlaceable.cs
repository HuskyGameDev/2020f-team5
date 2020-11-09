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
    private int upgradecost = 0;
    public GameObject towerUI;
    private int timesUpgraded = 0;
    private SpriteRenderer _tileHighlight;
    private bool beenPlaced = false;
    private bool toggleBool = false;
    void Start()
    {
        cost = 100;
        upgradecost = cost * 2;
        towerUI = GameObject.Find("TowerUI");
        towerUI.gameObject.SetActive(false);
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
    void upgrade()
    {
        if(Currency.amount >= upgradecost)
        {
            if (timesUpgraded < 3)
            {
                Currency.amount = Currency.amount - upgradecost;
                timesUpgraded = timesUpgraded + 1;
                upgradecost = upgradecost * 2;
            }
        }
       
    }
    void hasPlaced(bool hasPlaced)
    {
        this.SendMessage("placed", true);
        beenPlaced = true;
    }
    void type(int type)
    {
        this.SendMessage("setTowerType", type);
    }
    public void Select()
        {

    }
    void OnMouseUp()
    {
        
    }
 void OnMouseDown()
    {
        if(beenPlaced )
        {
            if(toggleBool)
            {
                towerUI.gameObject.SetActive(false);
                toggleBool = false;

            }
            else
            {
                towerUI.gameObject.SetActive(true);
                toggleBool = true;
            }
            
        }


       
    }
}
