using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;

public class FoodSpotOnTap : MonoBehaviour
{
    public int price = 10;
    public float cal = 1.1f;
    public float threshold = 200f;
    public AbstractMap map;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit, 300f))
            {
                // Detect tap with tag
                if (raycastHit.collider.CompareTag("FoodSpot"))
                {
                    Debug.Log("FoodSpot clicked");
                    if (CloseEnough())
                    {
                        // player gets widder
                        // TODO: why camera would rotate?
                        Transform player_target = GameObject.Find("LocationBasedGame").transform.Find("player_target_view");
                        //player_target.localScale = Vector3.Scale(player_target.localScale, new Vector3(cal,1,cal));
                        player_target.localScale += new Vector3(cal, 0, cal);
                        // money --
                        DisplayCurrency.currency -= price;
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
