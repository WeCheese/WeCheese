using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Mapbox.Unity.Map;

public class SwitchModeOnPress : MonoBehaviour
{
    public static bool hasSpawnedNearbyTree; // In interact mode
    // public GameObject squirrel;

    public void SwitchMode()
    {
        if (SceneManager.GetActiveScene().name == "exploration_scene")
        {
            // Go to interact
            // Set default, since we want to know later whether new tree is planted
            SceneManager.LoadScene("interaction_scene");
            GetComponentInChildren<TextMeshProUGUI>().text = "Explore";
        }
        else
        {
            // Go to map
            SceneManager.LoadScene("exploration_scene");
            GetComponentInChildren<TextMeshProUGUI>().text = "Interact";
            hasSpawnedNearbyTree = false; // When go back to map, initialize it
        }
    }


}
