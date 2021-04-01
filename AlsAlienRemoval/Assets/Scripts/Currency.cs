using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public static int amount;
    public static string displayCurrency = null;
    private static Text currencyText;

    // Start is called before the first frame update
    void Start()
    {
        amount = 750;
        displayCurrency = amount.ToString() + "$";
        currencyText = GetComponent<Text>();
        currencyText.text = displayCurrency;
    }

    // Update is called once per frame
    void Update()
    {
        displayCurrency = amount.ToString() + "$";
        currencyText.text = displayCurrency;
    }
    public static void textChange()
    {
        displayCurrency = amount.ToString() + "$";
        currencyText.text = displayCurrency;
    }
    public static void addCurrency(int changeAmount)
    {
        amount = amount + changeAmount;
        textChange();
    }
    public static void subtractCurrency(int changeAmount)
    {
        amount = amount - changeAmount;
        textChange();
   
    }
    public int getCurrency()
    {
        return amount;
    }
}
