using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyMonkeySpawn : MonoBehaviour
{
    public GameObject monkey;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SpawnMonkey.has_monkey){
            Debug.Log("show monkey in interaction mode");
            GameObject new_monkey = Instantiate(monkey);


            Transform cursor = GameObject.Find("ar_cursor").transform;
            new_monkey.transform.SetPositionAndRotation(
                cursor.position + new Vector3(0.4f, 0, 18.5f), 
                Quaternion.identity);
            new_monkey.transform.localScale *= 0.4f;
            new_monkey.transform.LookAt(Camera.main.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
