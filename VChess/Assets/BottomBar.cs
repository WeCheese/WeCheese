using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBar : MonoBehaviour
{
    static BottomBar instance;

    void Start()
    {
        if (instance == null)
        {
            // Here "this" means this Component
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }


}
