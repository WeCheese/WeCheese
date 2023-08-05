using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class PlaceBoard : MonoBehaviour
{
    public Transform cursor;
    public GameObject board_object;
    public AudioClip spawn_sound_effect;

    public static bool board_placed = false;
    
    void Update()
    {

        //Display2.text = string.Format("chess : {0} {1} {2}",
        //    chess.transform.position.x, chess.transform.position.y, chess.transform.position.z);
        //Display3.text = string.Format("chess : {0} {1} {2}",
        //    chess1.transform.position.x, chess1.transform.position.y, chess1.transform.position.z);
        //Display4.text = string.Format("chess : {0} {1} {2}",
        //    chess2.transform.position.x, chess2.transform.position.y, chess2.transform.position.z);
    }

    public void PlaceBoardAtCursor()
    {
        board_placed = true;
        board_object.transform.SetPositionAndRotation(cursor.position, cursor.rotation);
        board_object.transform.Translate(Vector3.down * 0.003f, Space.Self);

        AudioSource.PlayClipAtPoint(spawn_sound_effect, Camera.main.transform.position);
    }

    public void RotateClockwise()
    {
        board_object.transform.Rotate(0f, 10f, 0f, Space.Self);
    }

    public void RotateUnClockwise()
    {
        board_object.transform.Rotate(0f, -10f, 0f, Space.Self);
    }


    //public void DisplayLocation()
    //{
    //    Display.text = string.Format("cursor position: {0} {1} {2}", 
    //        cursor.position.x, cursor.position.y, cursor.position.z);
    //}

    //public void PrintHelloWorldFromPython()
    //{
    //    Vector3 vec = MyPythonScript.Run(new Vector3(2.0f, 2.0f, 2.0f));
    //    Display.text = string.Format("$x value: {0}", vec.x);

    //}
}
