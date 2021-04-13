using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public int amount;
    private static float staticAmount;

    public float startScalar;             // Starting value of scalar
    public static float staticScalar;   // Value multiplied with currency rewards

    public float scalarAdd;
    public float scalarMult;

    // Applied after each round:
    // staticScalar = staticAdd + staticMult * staticScalar
    private static float staticAdd;      
    private static float staticMult;

    public static string displayCurrency = null;
    private static Text currencyText;

    // Start is called before the first frame update
    void Start()
    {
        staticAmount = amount;

        staticScalar = startScalar;
        staticAdd = scalarAdd;
        staticMult = scalarMult;

        displayCurrency = amount.ToString() + "$";
        currencyText = GetComponent<Text>();
        currencyText.text = displayCurrency;

        // Check to see if scalar will ever reach 0
        float testScalar = startScalar;
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log($"WAVE {i} SCALAR: {testScalar}");
            testScalar = staticAdd + staticMult * testScalar;
        }

    }

    // Update is called once per frame
    void Update()
    {
        displayCurrency = ((int)staticAmount).ToString() + "$";
        currencyText.text = displayCurrency;
    }
    public static void textChange()
    {
        displayCurrency = ((int)staticAmount).ToString() + "$";
        currencyText.text = displayCurrency;
    }
    public static void addCurrency(float changeAmount)
    {
        staticAmount = staticAmount + changeAmount;
        textChange();
    }
    public static void subtractCurrency(float changeAmount)
    {
        staticAmount = staticAmount - changeAmount;
        textChange();
   
    }

    // Adjust (decrease) currency award scalar according to vars
    public static void advanceScalar()
    {
        staticScalar = staticAdd + staticMult * staticScalar;
    }

    // Scale a currency reward for enemy death, etc. before passing to add/subtract
    public static float scaleReward(float amount)
    {
        return amount * staticScalar;
    }

    public static float getCurrency()
    {
        return staticAmount;
    }
}
