using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonBuySeed : MonoBehaviour
{
    public int seed_type;
    private int price;
    public AudioClip good_sound_effect;

    private void Start()
    {
        price = (seed_type + 1) * 2;
    }

    private void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text =
            ButtonSpawnTreeInteract.all_seed_unlock_status[seed_type] ?
            string.Format("seed{0}: ${1}", seed_type + 1, price) :
            "Locked";
    }

    public void buySeed()
    {
        if ((DisplayCurrency.currency - price >= 0) 
            && ButtonSpawnTreeInteract.all_seed_unlock_status[seed_type] )
        {
            DisplayCurrency.currency -= price;
            ButtonSpawnTreeInteract.all_seed_number[seed_type]++;
            AudioSource.PlayClipAtPoint(good_sound_effect, Camera.main.transform.position);
        }

    }
}
