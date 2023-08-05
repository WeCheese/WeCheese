using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCurrency : MonoBehaviour
{
    public TextMeshProUGUI displaytext;
    public static int currency = 100;
    public static bool logo_scanned = false;

    // Update is called once per frame
    void Update()
    {
        displaytext.text = string.Format("${0}", currency);
    }
}
