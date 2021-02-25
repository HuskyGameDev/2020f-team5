﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buildingPlaceable : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer rangeSpriteRenderer;
    private int _triggerCount;  // Number of triggers this building is currently intersecting 
    public GameObject range;
    public int cost;
    private int upgradecost = 200;
    public GameObject towerUI;
    private int timesUpgraded = 0;
    private SpriteRenderer _tileHighlight;
    int towerType;
    private bool beenPlaced = false;
    private bool toggleBool = false;

    public Sprite star1;
    public Sprite star2;
    public Sprite star3;
    private static Sprite[] _starSprites;
    private SpriteRenderer _upgradeSprite;


    void Start()
    {
        upgradecost = cost * 2;
        towerUI = GameObject.Find("TowerUI");
        towerUI.gameObject.SetActive(false);
        rangeSpriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize array of sprites for viewing upgrades
        _starSprites = new Sprite[3] { star1, star2, star3 };
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
   public void upgradeTimes()
    {
        if(Currency.amount >= upgradecost)
        {
            if (timesUpgraded < 3)
            {

                // Create upgrade viewer for first upgrade
                if (timesUpgraded == 0)
                {
                    GameObject upgradeViewer = new GameObject("UpgradeViewer");
                    _upgradeSprite = upgradeViewer.AddComponent<SpriteRenderer>();
                    upgradeViewer.transform.parent = transform;
                    upgradeViewer.transform.position = transform.position;
                }

                // Update star sprite
                _upgradeSprite.sprite = _starSprites[timesUpgraded];

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
        towerType = type;
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
    public void sell()
    {
        Currency.amount = Currency.amount + (upgradecost - (upgradecost / 2));
        Destroy(gameObject);
        
    }
    public void destroy()
    {
        Destroy(gameObject);

    }
}
