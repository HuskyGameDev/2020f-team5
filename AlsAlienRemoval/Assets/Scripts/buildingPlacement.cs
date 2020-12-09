using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class buildingPlacement : MonoBehaviour
{
    private Transform currentBuilding;
    private bool hasPlaced;
    private buildingPlaceable buildingPlaceable;
    public SpriteRenderer rangeSpriteRenderer;
    // Update is called once per frame
    public void Update() 
    {
        if (currentBuilding != null && !hasPlaced) 
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
            currentBuilding.position = new Vector2(worldPoint2d.x,worldPoint2d.y);
            if (Input.GetMouseButtonDown(0)) 
            {
                if (buildingPlaceable.IsLegalPosition() && Currency.amount >= buildingPlaceable.getCost()) 
                {
                    hasPlaced = true;
                    Currency.subtractCurrency(buildingPlaceable.getCost());
                    buildingPlaceable.SendMessage("hasPlaced", true);
                    buildingPlaceable.range.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    buildingPlaceable.destroy();
                    currentBuilding = null;
                    
                }
            }
            
            if (hasPlaced) 
            {
                currentBuilding = null;
            }
        }
    }
    public void setItem(GameObject b) 
    {
        hasPlaced = false;
        currentBuilding = (Instantiate(b)).transform;
        buildingPlaceable = currentBuilding.GetComponent<buildingPlaceable>();
    }

    public void tower1()
    {
        buildingPlaceable.SendMessage("type", 1);
    }
    public void tower2()
    {
        buildingPlaceable.SendMessage("type", 2);
    }
    public void tower3()
    {
        buildingPlaceable.SendMessage("type", 3);
    }
    public void tower4()
    {
        buildingPlaceable.SendMessage("type", 4);
    }
    public void tower5()
    {
        buildingPlaceable.SendMessage("type", 5);
    }
    public void tower6()
    {
        buildingPlaceable.SendMessage("type", 6);
    }
}
