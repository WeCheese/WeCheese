using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShop : MonoBehaviour
{
    public GameObject panel;

    public void OpenAndCloseShop()
    {
        panel.SetActive(!panel.gameObject.activeSelf);
    }
}
