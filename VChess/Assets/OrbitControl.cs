using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitControl : MonoBehaviour
{
    public Transform target;

    private float sensitivity = 3f;

    private float rotationX;
    private float rotationY;

    public Vector3 offset = new Vector3(0,120,-75);

    private float r = 0f;
    private float angle = 0f;
    private float height = 1f; 

    void Start()
    {
        Vector3 center = new Vector3(target.position.x, transform.position.y, target.position.z);
        r = Vector3.Distance(transform.position, center);
        // height *= transform.position.y;
    }

    void Update()
    {
        
        if (Input.GetMouseButton (0))
        {
            
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            height += mouseY * 0.05f;
            height = Mathf.Clamp(height, 0.1f, 1.3f);
            Vector3 new_height = new Vector3(0, 120 * height,0);
           

            angle += getRadianAngle(mouseX);
            Vector3 desired_pos = target.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * r + new_height;
           
            transform.position = Vector3.Lerp(transform.position, desired_pos, 0.1f);
           
            transform.LookAt(target);

            offset = transform.position - target.position;
            //return;
        }

        Vector3 new_pos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, new_pos, 0.1f);
        //transform.LookAt(target);


    
        
    }

    float getRadianAngle(float h){
        // return h + 1f * Mathf.PI;
        if (h<0){
            h += 2;
        }
        return h * 1f * Mathf.PI;
    }
    
}
