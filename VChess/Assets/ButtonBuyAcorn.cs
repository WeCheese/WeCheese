using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuyAcorn : MonoBehaviour
{
    public AudioClip good_sound_effect;

    private int price = 10;

    public void BuyAcorn()
    {
        if (DisplayCurrency.currency - price >= 0)
        {
            DisplayCurrency.currency -= price;
            ButtonAcorn.acorn_number++;
            AudioSource.PlayClipAtPoint(good_sound_effect, Camera.main.transform.position);
        }
    }
}
