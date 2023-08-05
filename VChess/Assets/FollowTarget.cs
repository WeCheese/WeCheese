using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float ease_factor = 0.1f;
    public Vector3 offset = Vector3.zero;

    //public bool angle_in_direction_of_movement = false;
    Vector3 previous_position = Vector3.zero;

    void Start(){
        previous_position = transform.position;
    }
    void Update()
    {
        Vector3 desired_pos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired_pos, 0.1f);

        // if (angle_in_direction_of_movement)
        // {
        Vector3 position_delta = transform.position - previous_position;
        if (position_delta.magnitude > 0.0001f){
            //Debug.Log(position_delta);
            transform.forward = position_delta;
        }

        previous_position = transform.position;
            
        //}
    }
}
