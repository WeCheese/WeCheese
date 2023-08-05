using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class TreePositionSize : MonoBehaviour
{
    public Vector2d realWorldPosition;

    public AbstractMap map;

    private Vector3 fullSize = new Vector3(40f, 40f, 40f);

    public float fullDistance;

    private float percentage = 0;
    private float startingDistance = 0;
    private bool fullyGrown = false;

    void Start()
    {
        // startingDistance for each tree should be the current distance of player
        startingDistance = TrackMovementTime.distance;
    }


    void Update()
    {
        transform.position = map.GeoToWorldPosition(realWorldPosition);
        if (!fullyGrown)
        {
            UpdatePercentage();
            transform.localScale = fullSize * percentage;
            Debug.Log(percentage);
        }

    }

    void UpdatePercentage()
    {
        percentage = (TrackMovementTime.distance - startingDistance) / fullDistance;
        percentage = (percentage < 0.4f) ? 0.4f : percentage;         // To make sure percentage not start from 0
        if (percentage >= 1)
        {
            fullyGrown = true;
            percentage = 1;
            Debug.Log("increase revenue");
        }
    }
}