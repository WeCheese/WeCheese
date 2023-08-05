using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class LoseWeight : MonoBehaviour
{
    public Vector2d realWorldPosition;

    public AbstractMap map;

    private Vector3 burntCal = new Vector3(0.1f,0f,0.1f);

    public float walkingThreshold = 10f;
    private float startingDistance = 0;
    
    void Start()
    {
        // startingDistance for each tree should be the current distance of player
        startingDistance = TrackMovementTime.distance;
    }

    void Update()
    {
        if (GainedWeight())
        {
            // walking every step lose some weight
            if (WalkForAWhile()){
                transform.localScale -= burntCal;
            }
        }

    }

    bool WalkForAWhile()
    {
        if (TrackMovementTime.distance - startingDistance >= walkingThreshold)
        {
            startingDistance = TrackMovementTime.distance;
            return true;
            
        }else{
            return false;
        }
    }

    bool GainedWeight()
    {
        return transform.localScale.x > 1f && transform.localScale.z > 1f ;
    }
}
