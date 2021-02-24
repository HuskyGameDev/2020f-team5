using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class buildingPlacement : MonoBehaviour
{
    private Transform currentBuilding;
    private bool towerSelected;
    private bool hasPlaced;
    private buildingPlaceable buildingPlaceable;
    public SpriteRenderer rangeSpriteRenderer;
    public Sprite highlightSprite;

    //error text stuff
    private GameObject errorText_Money;                     // "not enouch money" error text object
    private GameObject errorText_Location;                  // "invalid location" error text object
    public static int errorTextCount;                       // tracks # of error texts (too many = lag)
    private static Queue<FloatingFadeText> errorTexts;      // list of currently active error-text wrapper objects
    private const int ERROR_TEXT_LIMIT = 3;                 // maximum error texts allowed at once
    
    // wrapper class for floating error texts
    private class FloatingFadeText {
        private GameObject errorTextObject;         // reference to actual text object
        private Vector2 fadeBegin;                  // mouse location where text starts
        private float fadeTime = 2.65f;             // seconds text will be visible
        private float fadeDistance = 0.7f;          // distance text travels
        private float timeRemaining;                // time until text is removed

        // constructor
        public FloatingFadeText(GameObject errorTextObject, Vector3 mousePosition) {

            this.errorTextObject = errorTextObject;
            fadeBegin = new Vector2(mousePosition.x, mousePosition.y);
            timeRemaining = fadeTime;

            errorTextObject.GetComponent<CanvasGroup>().alpha = 1;
            errorTextObject.GetComponent<Text>().transform.position = fadeBegin;
        }

        // updates the fade and location of the text
        public bool updateFadeAndLocation() {
            
            // updates alpha
            float currentAlpha = errorTextObject.GetComponent<CanvasGroup>().alpha;
            float newAlpha = Math.Max(0, currentAlpha - (Time.deltaTime / fadeTime));
            errorTextObject.GetComponent<CanvasGroup>().alpha = newAlpha;

            // updates position
            float currentY = errorTextObject.GetComponent<Text>().transform.position.y;
            float newY = currentY + ((Time.deltaTime / fadeTime) * fadeDistance);
            Vector2 newPosition = new Vector2(errorTextObject.GetComponent<Text>().transform.position.x, newY);
            errorTextObject.GetComponent<Text>().transform.position = newPosition;
            
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0) {
                Object.Destroy(errorTextObject);
                return false;
            }
            return true;
        }
    }

    // runs once
    public void Start() {
        errorText_Money = GameObject.Find("text_no_money");
        errorText_Location = GameObject.Find("text_invalid_location");
        errorTextCount = 0;
        towerSelected = false;
        errorTexts = new Queue<FloatingFadeText>();
    }

    // Update is called once per frame
    public void Update() {
        
        // updates location of all floating error-text
        int dequeus = 0;
        foreach (FloatingFadeText text in errorTexts) {
            if (! text.updateFadeAndLocation()) {
                dequeus++;
            }
        }

        // removes any floating error-text that have faded
        for (int i = 0; i < dequeus; i++) {
            errorTexts.Dequeue();
            errorTextCount--;
        }

        if (currentBuilding != null && !hasPlaced) 
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
            currentBuilding.position = new Vector2(worldPoint2d.x,worldPoint2d.y);

            if (buildingPlaceable.IsLegalPosition() && Currency.amount >= buildingPlaceable.getCost()) {
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
                if (Currency.amount >= buildingPlaceable.getCost()) {

                    // valid position, builds tower
                    if (buildingPlaceable.IsLegalPosition()) {
                        buildingPlaceable.SendMessage("hasPlaced", true);
                        hasPlaced = true;
                        towerSelected = false;
                        Currency.subtractCurrency(buildingPlaceable.getCost());
                        buildingPlaceable.range.GetComponent<SpriteRenderer>().enabled = false;
                    }

                    // invalid position, show error text
                    else {
                        if (errorTextCount < ERROR_TEXT_LIMIT) {
                            GameObject errorText = Object.Instantiate(errorText_Location, errorText_Location.transform, true);
                            Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            errorTexts.Enqueue(new FloatingFadeText(errorText, mousePos3d));
                            errorTextCount++;
                        }
                        return;
                    }

                }

                // not enough money, shows error text (higher prio than "invalid location")
                else {
                    if (errorTextCount < ERROR_TEXT_LIMIT) {
                        GameObject errorText = Object.Instantiate(errorText_Money, errorText_Money.transform, true);
                        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        errorTexts.Enqueue(new FloatingFadeText(errorText, mousePos3d));
                        errorTextCount++;
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
