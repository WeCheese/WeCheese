using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPointingNorth : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position - target.forward * 10f;
        //transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        transform.forward = new Vector3(-1,0,0);
    }
}
