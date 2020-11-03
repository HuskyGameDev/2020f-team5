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
    private int cost = 100;
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
                if (buildingPlaceable.IsLegalPosition() && Currency.amount >= cost) 
                {
                    hasPlaced = true;
                    Currency.subtractCurrency(cost);
                    buildingPlaceable.SendMessage("hasPlaced", true);
                    buildingPlaceable.range.GetComponent<SpriteRenderer>().enabled = false;
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
}
