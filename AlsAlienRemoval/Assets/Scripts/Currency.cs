using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public static int amount;
    public static string displayCurrency = null;
    // Start is called before the first frame update
    void Start()
    {
        amount = 500;
        displayCurrency = amount.ToString() + "$";
    Text currency = GameObject.Find("Canvas/MainUIPanel/CurrencyDisplay").GetComponent<Text>();
    currency.text = displayCurrency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void textChange()
    {
        displayCurrency = amount.ToString() + "$";
        Text currency = GameObject.Find("Canvas/MainUIPanel/CurrencyDisplay").GetComponent<Text>();
        currency.text = displayCurrency;
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
