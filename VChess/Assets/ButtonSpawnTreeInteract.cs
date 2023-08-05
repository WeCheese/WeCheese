using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonSpawnTreeInteract : MonoBehaviour
{
    //int[] all_seed_number_initial = {  }
    public static List<int> all_seed_number = new List<int>() { 
        10, 10, 10, 10, 10, 10, 10, 10, 10, 10
    };
    public static List<bool> all_seed_unlock_status = new List<bool>()
    {
        true, true, true, true, true, true, true, false, false, false
    };

    public GameObject gameobject_to_spawn;
    public int tree_type;                   // Fill this in editor
    public AudioClip spawn_sound_effect;

    private Transform cursor;
    private bool isInteract;

    void Start()
    {
        if (GameObject.Find("ar_cursor") == null)
        {
            // We are in exploration mode -> do nothing
            isInteract = false;
        }
        else
        {
            // We are in interact mode
            isInteract = true;
            cursor = GameObject.Find("ar_cursor").transform;
        }
    }

    public void SpawnGameobjectAtCursor()
    {
        if ((all_seed_number[tree_type] > 0) 
            && isInteract 
            && all_seed_unlock_status[tree_type])
        {
            GameObject new_object = Instantiate(gameobject_to_spawn);
            new_object.transform.SetPositionAndRotation(cursor.position, cursor.rotation);
            new_object.transform.localScale = new (12f, 12f, 12f);  // which = 60 * 0.2
            AudioSource.PlayClipAtPoint(spawn_sound_effect, Camera.main.transform.position);

            // Add the new tree to list
            _ = new TreeInfo(TrackMovementTime.player_lat_long, 
                tree_type, TrackMovementTime.distance);

            all_seed_number[tree_type]--;
        }

    }

    private void Update()
    {
        if (GameObject.Find("ar_cursor") == null)
        {
            // We are in exploration mode
            isInteract = false;
        }
        else
        {
            // We are in interact mode
            isInteract = true;
            cursor = GameObject.Find("ar_cursor").transform;
        }

        GetComponentInChildren<TextMeshProUGUI>().text =
            all_seed_unlock_status[tree_type] ?
            string.Format("{0}", all_seed_number[tree_type]) :
            "Locked";
    }

}
