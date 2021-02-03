using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class buildingPlacement : MonoBehaviour
{
    private Transform currentBuilding;
    private bool hasPlaced;
    private buildingPlaceable buildingPlaceable;
    public SpriteRenderer rangeSpriteRenderer;
    private TextFader textFader;                    // for fading error text
    private GameObject money_text;                  // cloned to create error text
    private int errorTextCount;                     // tracks # of error texts (too many = lag)

    public void Start() {
        money_text = GameObject.Find("text_no_money");
        money_text.AddComponent<TextFader>();
        errorTextCount = 0;
    }

    // controls the fading and rising of the "not enough money" text
    private class TextFader : MonoBehaviour
    {
        public float fadeTime = 1.5f;
        private Vector3 mousePos3d;
        private Vector2 fadeBegin;
        private Vector2 fadeEnd;

        public void RiseAndFade(GameObject obj) {
            mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fadeBegin = new Vector2(mousePos3d.x, mousePos3d.y);
            fadeEnd = new Vector2(fadeBegin.x, fadeBegin.y + 0.8f);
            obj.GetComponent<Text>().color = new Color(0, 0, 0, 255);
            StartCoroutine(RiseAndFadeRoutine(obj));
        }

        // uses a lerp loop to fade color and move text upward
        private IEnumerator RiseAndFadeRoutine(GameObject obj) {

            for (float t = 0f; t < fadeTime; t += (2*Time.deltaTime))
            {
                obj.GetComponent<Text>().color = Color.Lerp(Color.black, Color.clear, Mathf.Min(1, t/fadeTime));
                obj.GetComponent<Text>().transform.position = Vector2.Lerp(fadeBegin, fadeEnd, Mathf.Min(1, t/fadeTime));
                yield return null;
            }
            Object.Destroy(obj);
        }
    }

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
                    buildingPlaceable.SendMessage("hasPlaced", true);
                    hasPlaced = true;
                    Currency.subtractCurrency(buildingPlaceable.getCost());
                    buildingPlaceable.range.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    // trying to build without enough money, display error message at cursor
                    if (buildingPlaceable.IsLegalPosition() && Currency.amount < buildingPlaceable.getCost()) {

                        if (errorTextCount < 5) {
                            errorTextCount++;
                            GameObject errorText = Object.Instantiate(money_text, money_text.transform, true);
                            errorText.GetComponent<TextFader>().RiseAndFade(errorText);
                        }
                        return;
                    }
                    else
                    {
                        // todo: implement a better solution that doesn't remove the tower from the cursor
                        // todo: maybe a right-click will do this. It's currently very annoying having
                        // todo: to re-click the button every time you mis-click on the path.
                        buildingPlaceable.destroy();
                        currentBuilding = null;
                    }
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
