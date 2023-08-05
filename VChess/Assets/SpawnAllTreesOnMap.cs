using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class TreeInfo
{
    public static List<TreeInfo> all_trees = new List<TreeInfo>();
    public static Vector3 full_size = new(600f, 600f, 600f);
    public static float full_dis = 100f;            // The distance when the tree should grow fully

    public Vector2d lat_long;
    public float growth = 0.0f;
    public float starting_dis = 0.0f;
    public int tree_type = -1;
    public bool has_generated_currency = false;
    public bool has_squirrel = false;

    public TreeInfo(Vector2d _lat_long, int _tree_type, float _starting_dis)
    {
        lat_long = _lat_long;
        tree_type = _tree_type;
        starting_dis = _starting_dis;

        all_trees.Add(this);
    }

    // TODO: not sure we need the destructor
}


public class SpawnAllTreesOnMap : MonoBehaviour
{
    public List<GameObject> prefabs;
    public List<GameObject> tree_instances = new List<GameObject>();
    public AbstractMap map;
    public AudioClip fully_grown_sound_effect;
    public GameObject prefab_squirrel;  // Drag it in editor

    void Start()
    {
        // Spawn all the tree once here, adjust the position later in Update
        foreach(TreeInfo ti in TreeInfo.all_trees)
        {
            GameObject new_tree = GameObject.Instantiate(prefabs[ti.tree_type]);

            tree_instances.Add(new_tree);
        }
    }

    void Update()
    {
        int i = 0;
        foreach (TreeInfo ti in TreeInfo.all_trees)
        {
            GameObject tree = tree_instances[i];
            int tree_money = ti.has_squirrel ? 
                (ti.tree_type + 1) : (ti.tree_type + 1) * 10;   // We define money as this
            tree.transform.position = map.GeoToWorldPosition(ti.lat_long);
            tree.transform.position += new Vector3(0, 3, 0);   // To raise the tree a bit

            if (ti.growth <= 1)
            {
                ti.growth = (TrackMovementTime.distance - ti.starting_dis) / TreeInfo.full_dis < 0.2f ? 
                    0.2f : 
                    (TrackMovementTime.distance - ti.starting_dis) / TreeInfo.full_dis;
                tree.transform.localScale = ti.growth * TreeInfo.full_size;
            }

            if ((ti.growth > 1) && !ti.has_generated_currency)
            {
                ti.has_generated_currency = true;
                DisplayCurrency.currency += tree_money;
                AudioSource.PlayClipAtPoint(fully_grown_sound_effect, Camera.main.transform.position);
            }

            RandomSpawnSquirrel(i);

            i++;
        }

    }

    void RandomSpawnSquirrel(int i)
    {
        if (TreeInfo.all_trees[i].has_squirrel)    
            return;

        float randomNumber = Random.Range(0f, 10.0f);
        if (randomNumber <= 0.01f)
        {
            Debug.Log("a sq is spawned in map");
            // spawn a squirrel
            GameObject new_squirrel = Instantiate(prefab_squirrel);
            new_squirrel.transform.SetPositionAndRotation(
                tree_instances[i].transform.position + new Vector3(2, 0, 2),
                Quaternion.identity);
            TreeInfo.all_trees[i].has_squirrel = true;
        }
    }

}
