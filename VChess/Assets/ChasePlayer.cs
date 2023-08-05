using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mapbox.Unity.Map;

// This class probably should be renamed
public class ChasePlayer : MonoBehaviour
{
    private GameObject player;
    public int deductMonet = 10;
    public float speed = 6f;
    // Update is called once per frame
    void Start(){
        if (SceneManager.GetActiveScene().name == "exploration_scene")
        {
            player = GameObject.Find("player_target_view");
        }
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "exploration_scene")
        {
            if (CheckCatchUp())
            {
                Debug.Log("monkey catch up");
                Destroy(gameObject);
                SpawnMonkey.has_monkey = false;
                return;
            }
            Debug.Log("move");
            // chase player
            var step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,player.transform.position,step);
            transform.LookAt(player.transform);
        }

        
    }

    bool CheckCatchUp()
    {
        float dis = Vector3.Distance(
            transform.position,
            player.transform.position
            );
        Debug.Log("distance");
        Debug.Log(dis);
        if (dis < 1f)
        {
            DisplayCurrency.currency -= deductMonet;
            return true;
        }
        return false;
    }
}
