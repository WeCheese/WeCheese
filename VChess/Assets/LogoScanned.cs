using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScanned : MonoBehaviour
{
    
    private int price = 250;
    // Start is called before the first frame update
    void Start()
    {
        if (DisplayCurrency.logo_scanned)
            return;
        
        DisplayCurrency.logo_scanned = true;
        // give money to user
        DisplayCurrency.currency += price;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
