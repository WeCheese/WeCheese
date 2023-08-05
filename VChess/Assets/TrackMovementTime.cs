using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class TrackMovementTime : MonoBehaviour
{
    public static float distance;

    // Has something around in MAP
    public static bool hasTreeAround = false;
    public static bool hasSquirrelAround = false;

    public static int around_tree_info_index;     // from map

    public static Vector2d player_lat_long;
    public AbstractMap map;

    private Vector3 previousPosition = new Vector3(0, 0, 0);    


    void Update()
    {
        distance += Vector3.Distance(transform.position, previousPosition);
        previousPosition = transform.position;
        CheckNearbyTree();
        player_lat_long = map.WorldToGeoPosition(transform.position);
    }

    void CheckNearbyTree()
    {
        hasTreeAround = false;

        int i = 0;
        float curr_min = 99999f;

        foreach (TreeInfo ti in TreeInfo.all_trees)
        {
            if (Vector3.Distance(transform.position, map.GeoToWorldPosition(ti.lat_long)) < curr_min)
            {
                curr_min = Vector3.Distance(transform.position, map.GeoToWorldPosition(ti.lat_long));
                around_tree_info_index = i;
                hasSquirrelAround = ti.has_squirrel;
            }
            hasTreeAround = true;
            i++;
        }
        //Debug.Log("final:");
        //Debug.Log(TreeInfo.all_trees[around_tree_info_index].has_squirrel);
    }


    void Start()
    {
        //// Each time returning from intera to explor, we check whether a new tree should be added to map
        //if (plantedNewTreeType >= 0)
        //{
        //    Debug.Log("new tree is planted in interact, so we need to add it to list");
        //    _ = new TreeInfo(map.WorldToGeoPosition(transform.position), plantedNewTreeType);
        //}
    }

    void spawnNewTreeAtCurrentPos()
    {
        //Debug.Log("new tree is planted on map");
        //GameObject new_object = Instantiate(newPlantedTreeModel);
        //new_object.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //new_object.GetComponent<tree_grow>().realworldposition =
        //    WorldToGeoPosition(); // Record tree's coordinates

        //Debug.Log("current:");
        //Debug.Log(transform.position);

        //DontDestroyOnLoad(new_object);
    }

}


