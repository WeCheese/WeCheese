using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonAcorn : MonoBehaviour
{
    public static int acorn_number = 10;

    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text =
            string.Format("{0}", acorn_number);
    }
}
