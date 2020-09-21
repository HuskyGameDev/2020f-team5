using System.Collections;
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
            Vector2 m = Input.mousePosition;
            currentBuilding.position = new Vector2(m.x/100,m.y/100);
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
