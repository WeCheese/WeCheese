using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class BattleMonkey : MonoBehaviour
{
    //public GameObject monkey;
    public GameObject acorn_prefab;
    private Transform cursor;

    private GameObject monkey = null;
    private List<GameObject> acorn_list = new ();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursor = GameObject.Find("ar_cursor").transform;
        if (GameObject.FindGameObjectsWithTag("Monkey").Length > 0)
        {
            monkey = GameObject.FindGameObjectsWithTag("Monkey")[0];
        } else {
            return;
        }
        
        CheckDestroy();
        AcornFly();
    }

   

    private void CheckDestroy()
    {
        foreach (GameObject acorn in acorn_list)
        {
            float dis = Vector3.Distance(
                acorn.transform.position,
                monkey.transform.position
                );
            if (dis < 1f)
            {
                Destroy(monkey);
                SpawnMonkey.has_monkey = false;
            }
        }
    }

    private void AcornFly()
    {
        foreach (GameObject acorn in acorn_list)
        {
            acorn.transform.Translate(
                (cursor.position - Camera.main.transform.position) * Time.deltaTime
                );
        }
    }

    public void ThrowAcorn()
    {
        if (ButtonAcorn.acorn_number <= 0)
            return;

        GameObject acorn = Instantiate(acorn_prefab);
        acorn.transform.localScale *= 0.1f;
        acorn_list.Add(acorn);
        ButtonAcorn.acorn_number--;
        
        // First put it at camera's location
        acorn.transform.SetPositionAndRotation(
            Camera.main.transform.position,
            Quaternion.identity
            );
    }
}
