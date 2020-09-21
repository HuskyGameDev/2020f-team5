﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPlacement : MonoBehaviour
{
    private Transform currentBuilding;
    private bool hasPlaced;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    public void Update()
    {
        if(currentBuilding != null && !hasPlaced)
            {
         
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
            currentBuilding.position = new Vector2(worldPoint2d.x,worldPoint2d.y);

            if(Input.GetMouseButtonDown(0))
                {
                hasPlaced = true;
            }
            if(hasPlaced)
                {
                currentBuilding = null;
            }
        }
        
    }
    public void setItem(GameObject b)
        {
        hasPlaced = false;
       currentBuilding = ((GameObject)Instantiate(b)).transform;
        }
}