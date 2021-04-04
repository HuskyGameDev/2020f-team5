using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class buildingPlacement : MonoBehaviour
{
    private Transform currentBuilding;
    private bool towerSelected = false;
    private bool hasPlaced;
    private buildingPlaceable buildingPlaceable;
    public SpriteRenderer rangeSpriteRenderer;
    public Sprite highlightSprite;

    // runs once for EACH INSTANCE (one for each tower...)
    public void Start() {

    }

    // Update is called once per frame
    public void Update() {

        if (currentBuilding != null && !hasPlaced) 
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
            currentBuilding.position = new Vector2(worldPoint2d.x,worldPoint2d.y);

            if (buildingPlaceable.IsLegalPosition() && Currency.getCurrency() >= buildingPlaceable.getCost()) {
                buildingPlaceable.range.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, .25f);
            }
            else {
                buildingPlaceable.range.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, .25f);
            }

            // RIGHT CLICK
            if (Input.GetMouseButtonDown(1)) {
                towerSelected = false;
                buildingPlaceable.destroy();
                currentBuilding = null;
            }

            // LEFT CLICK
            if (Input.GetMouseButtonDown(0)) {

                // has enough money
                if (Currency.getCurrency() >= buildingPlaceable.getCost()) {

                    // valid position, builds tower
                    if (buildingPlaceable.IsLegalPosition()) {
                        buildingPlaceable.SendMessage("hasPlaced", true);
                        hasPlaced = true;
                        towerSelected = false;
                        Currency.subtractCurrency(buildingPlaceable.getCost());
                        buildingPlaceable.range.GetComponent<SpriteRenderer>().enabled = false;

                        // display -$123 floating buy text
                        if (Level.currencyTextCount < Level.CRNCY_TEXT_LIMIT) {
                            Level.currencyTextList[Level.nextCrncyIndex].SetText($"- ${buildingPlaceable.getCost()}");
                            Level.currencyTextList[Level.nextCrncyIndex].SetColor(Color.red);
                            Level.currencyTextList[Level.nextCrncyIndex].StartFadeCycle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            Level.currencyTextCount++;
                            Level.nextCrncyIndex++;
                            if (Level.nextCrncyIndex == Level.CRNCY_TEXT_LIMIT)
                                Level.nextCrncyIndex = 0;
                        }
                    }

                    // invalid position, show error text
                    else {
                        if (Level.errorTextCount < Level.ERROR_TEXT_LIMIT) {
                            Level.errorTextList[Level.nextErrIndex].SetText("Invalid Location");
                            Level.errorTextList[Level.nextErrIndex].StartFadeCycle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            Level.errorTextCount++;
                            Level.nextErrIndex++;
                            if (Level.nextErrIndex == Level.ERROR_TEXT_LIMIT)
                                Level.nextErrIndex = 0;
                        }
                        return;
                    }
                }

                // not enough money, shows error text (higher prio than "invalid location")
                else {
                    if (Level.errorTextCount < Level.ERROR_TEXT_LIMIT) {
                        Level.errorTextList[Level.nextErrIndex].SetText("Not Enough $$");
                        Level.errorTextList[Level.nextErrIndex].StartFadeCycle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        Level.errorTextCount++;
                        Level.nextErrIndex++;
                        if (Level.nextErrIndex == Level.ERROR_TEXT_LIMIT)
                            Level.nextErrIndex = 0;
                    }
                    return;
                }
            }
            
            if (hasPlaced) {
                currentBuilding = null;
            }
        }
    }

    public void setItem(GameObject b) 
    {
        if (towerSelected == false)
        {
            towerSelected = true;
            hasPlaced = false;
            currentBuilding = (Instantiate(b)).transform;
            buildingPlaceable = currentBuilding.GetComponent<buildingPlaceable>();
        }
        else
        {
            towerSelected = false;
            buildingPlaceable.destroy();
            currentBuilding = null;
        }
           
    }

    public void tower1() {
        buildingPlaceable.SendMessage("type", 1);
    }

    public void tower2() {
        buildingPlaceable.SendMessage("type", 2);
    }

    public void tower3() {
        buildingPlaceable.SendMessage("type", 3);
    }

    public void tower4() {
        buildingPlaceable.SendMessage("type", 4);
    }

    public void tower5() {
        buildingPlaceable.SendMessage("type", 5);
    }

    public void tower6() {
        buildingPlaceable.SendMessage("type", 6);
    }
}
