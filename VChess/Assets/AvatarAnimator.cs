using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimator : MonoBehaviour
{
    Animator anim;
    Vector3 previous_position = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        previous_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.position - previous_position;
        float speed = movement.magnitude;
        // if (Input.GetKey(KeyCode.Space))
        //     speed=0f;

        anim.SetFloat("speed",speed);
        anim.speed = Mathf.Max(speed * 0.5f, 1f);
        previous_position=transform.position;
    }
}
