using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class LandmarkPosition : MonoBehaviour
{
    public Vector2d realWorldPosition;
    public AbstractMap map;

    void Start()
    {
        //transform.position = map.GeoToWorldPosition(realWorldPosition);
        //Debug.Log("Landmark tansform position:");
        //Debug.Log(transform.position);
    }

    void Update()
    {
        transform.position = map.GeoToWorldPosition(realWorldPosition);
    }
}
