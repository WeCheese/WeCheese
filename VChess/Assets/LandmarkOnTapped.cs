using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;

public class LandmarkOnTapped : MonoBehaviour
{
    public GameObject panel;
    public float threshold = 200f;
    public AbstractMap map;
    public int locked_seed_type;  // Enter this in editor
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit, 300f))
            {
                // Detect tap with tag
                if (raycastHit.collider.CompareTag("Landmark"))
                {
                    Debug.Log("Landmark clicked");
                    if (CloseEnough())
                    {
                        panel.SetActive(!panel.gameObject.activeSelf);
                        
                        // Unlock the corresponding seed
                        ButtonSpawnTreeInteract.
                            all_seed_unlock_status[locked_seed_type] = true;
                    }
                }
            }
        }
    }

    
    bool CloseEnough()
    {
        Transform player_target = GameObject.Find("LocationBasedGame").transform.Find("PlayerTarget");

        float distance = Vector3.Distance(
            player_target.position,
            transform.position
        );


        return (distance < threshold);
    }


}
