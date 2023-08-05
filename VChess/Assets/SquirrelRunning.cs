using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelRunning : MonoBehaviour
{
    public GameObject tree;
    void Start()
    {
        
    }

    public Vector3 movement_relative_to_object;

    void Update ()
    {
        //if (tree != null) {
        //    transform.RotateAround(tree.transform.position, Vector3.up, 20 * Time.deltaTime);
        //}
        
        // transform.Translate(movement_relative_to_object * Time.deltaTime);
    }

    void OnCollisionEnter(Collision otherObj) {
        if (otherObj.gameObject.tag == "Acorn") {
            Destroy(gameObject);
        }
    }
}
