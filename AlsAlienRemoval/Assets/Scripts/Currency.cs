using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
    Text currency = GameObject.Find("Canvas/MainUIPanel/CurrencyDisplay").GetComponent<Text>();
    currency.text = "500$";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TextChange()
    {
        //currency.text ="500$" ;
    }
}
