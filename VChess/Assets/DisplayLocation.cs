using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Unity.Location;

public class DisplayLocation : MonoBehaviour
{
    public TextMeshProUGUI location_text;

    void Update()
    {
        //location_text.text = EditorLocationProviderLocationLog.current_latlong;
    }
}
