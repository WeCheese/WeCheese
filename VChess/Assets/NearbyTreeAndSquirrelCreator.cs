using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyTreeAndSquirrelCreator : MonoBehaviour
{
    public static bool hasSpawnedNearbyTree;
    public List<GameObject> prefabs;

    public GameObject squirrel_prefab;
    public GameObject acorn_prefab;
    public Transform cursor;

    // Tree and sq spawned when entering the interact mode
    private GameObject tree = null;
    private GameObject squirrel = null;
    private List<GameObject> acorn_list = new ();

    void Start()
    {
        hasSpawnedNearbyTree = false;
    }

    void Update()
    {
        if (!hasSpawnedNearbyTree)
        {
            if (GameObject.Find("ar_cursor") != null)
            {
                hasSpawnedNearbyTree = true;
                if (TrackMovementTime.hasTreeAround)
                {
                    Debug.Log("tree in interact");
                    TreeInfo ti = TreeInfo.all_trees[TrackMovementTime.around_tree_info_index];
                    tree = Instantiate(prefabs[ti.tree_type]);
                    Transform cursor = GameObject.Find("ar_cursor").transform;
                    tree.transform.SetPositionAndRotation(cursor.position, cursor.rotation);
                    tree.transform.localScale = ti.growth * TreeInfo.full_size * 0.1f;  // Make it smaller in interaction
                }

                if (TrackMovementTime.hasSquirrelAround)
                {
                    Debug.Log("sq in interact");
                    squirrel = Instantiate(squirrel_prefab);
                    Transform cursor = GameObject.Find("ar_cursor").transform;
                    squirrel.transform.SetPositionAndRotation(
                        cursor.position + new Vector3(0.5f, 0, 0.5f), 
                        Quaternion.identity);
                    squirrel.transform.localScale *= 0.1f;
                }
            }
        }

        CheckSquirrelDestroy();
        SquirrelRun();
        AcornFly();
    }
    private void SquirrelRun()
    {
        if ((tree != null) && (squirrel != null))
        {
            squirrel.transform.RotateAround(
                tree.transform.position, Vector3.up, 20 * Time.deltaTime);
        }
    }

    private void CheckSquirrelDestroy()
    {
        foreach (GameObject acorn in acorn_list)
        {
            float dis = Vector3.Distance(
                acorn.transform.position,
                squirrel.transform.position
                );
            if (dis < 1f)
            {
                Destroy(squirrel);
                TreeInfo.all_trees[TrackMovementTime.around_tree_info_index].
                    has_squirrel = false;
                TrackMovementTime.hasSquirrelAround = false;
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
