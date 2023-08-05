using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBoard : MonoBehaviour
{
    public Transform cursor;
    public GameObject board_object;
    public AudioClip spawn_sound_effect;

    void Start()
    {
    }

    void Update()
    {
    }

    public void PlaceBoardAtCursor()
    {
        board_object.transform.SetPositionAndRotation(cursor.position, cursor.rotation);

        // AudioSource.PlayClipAtPoint(spawn_sound_effect, Camera.main.transform.position);
    }
}