using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonkey : MonoBehaviour
{
    public GameObject prefab_monkey;
    public static bool has_monkey = false;

    void Update()
    {
        if (has_monkey){
            return;
        }
        float randomNumber = Random.Range(0f, 10.0f);
        if (randomNumber <= 0.001f)
        {
            Debug.Log("a monkey is spawned in map");
            // spawn a monkey
            GameObject new_monkey = Instantiate(prefab_monkey);
            new_monkey.transform.SetPositionAndRotation(
                transform.position + RandomPosition(),
                Quaternion.identity);
            new_monkey.transform.LookAt(transform);
            has_monkey = true;
        }
    }

    Vector3 RandomPosition(){
        float r = 20f;
        float x = Random.Range(0f, r);
        float z = Mathf.Sqrt(Mathf.Pow(r,2) - Mathf.Pow(x,2));
        return new Vector3(x,0,z);
    }
}
