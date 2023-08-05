using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MoveChess : MonoBehaviour
{
    public Transform cursor;
    public Transform new_cursor;
    public GameObject[] all_my_chess;
    public GameObject[] all_enemy_chess;
    public GameObject[] all_points;

    // Control my move
    private bool chess_selected = false;
    private int selected_chess_index = 0;

    private bool point_selected = false;
    private int selected_point_index = 0;

    // Control opponent move
    private int opponent_chess_index = 0;
    private int opponent_destination_index = 0;
    private bool opponent_should_move = false;

    public bool game_start = false;

    public static float dis_threshold_sq = 0.0025f;

    public ChessController cc;
    public FirebaseManager fm;
    private bool ccCreated = false;

    public AudioClip sound_effect;
    public AudioClip eaten_sound_effect;

    private bool in_cool_down = false;
    private float cool_down_timer = 0f;
    private float cool_down_wait_time = 3.0f;

    public bool is_first;
    private Vector3 scaleChange = new Vector3(0.03f, 0f, 0.03f);

    void Update()
    {
        // Check whether game starts
        /*if (game_start && !ccCreated)
        {
            Debug.Log("MoveChess before cc.init");
            ccCreated = true;
            cc.init(fm.IsStartPlayer());
            // cc.init(false);
        }*/
        // Always check if my chess needs to be moved
        if (chess_selected && point_selected)
        {
            Vector3 distance_vec = all_points[selected_point_index].transform.position - all_my_chess[selected_chess_index].transform.position;
            
            // Close enough, stop moving
            if (distance_vec.magnitude < 0.005f)
            {
                chess_selected = false;
                point_selected = false;
                return;
            }
            distance_vec /= distance_vec.magnitude;
            all_my_chess[selected_chess_index].transform.Translate(0.3f * distance_vec * Time.deltaTime, Space.World);
        }
        
        // Always check if a opponent chess needs to be moved
        if (opponent_should_move)
        {
            Vector3 distance_vec = all_points[opponent_destination_index].transform.position - all_enemy_chess[opponent_chess_index].transform.position;
            
            // Close enough, stop moving
            if (distance_vec.magnitude < 0.005f)
            {
                opponent_should_move = false;
                return;
            }
            distance_vec /= distance_vec.magnitude;
            all_enemy_chess[opponent_chess_index].transform.Translate(0.3f * distance_vec * Time.deltaTime, Space.World);  
        }
        
        // Calculate cooldown time
        if (in_cool_down)
        {
            cool_down_timer += Time.deltaTime;
            if (cool_down_timer > cool_down_wait_time)
            {
                cool_down_timer = 0f;
                in_cool_down = false;
            }
        }
    }

    public void testOppoMove()
    {
        opponent_should_move = true;
        opponent_chess_index = 7;
        opponent_destination_index = 9;
    }

    public void testMyMove()
    {
        chess_selected = true;
        point_selected = true;
        selected_chess_index = 0;
        selected_point_index = 0;
    }
    public void ShowOpponentMove(int opponent_chess_index, int opponent_destination_index)
    {
        this.opponent_chess_index = opponent_chess_index;
        this.opponent_destination_index = opponent_destination_index;
        opponent_should_move = true;
    }
    
    public void SelectChessOldCursor()
    {
        Debug.Log("before audio  SelectChessOldCursor");
        SelectChess(cursor.position);
        Debug.Log("before audio chesSelectChessOldCursors");
    }

    public void SelectDestinationOldCursor()
    {
        SelectDestination(cursor.position);
    }

    public void SelectByHand()
    {
        if (!in_cool_down)
        {
            if (!chess_selected)
            {
                SelectChess(new_cursor.position);
            }
            else
            {
                SelectDestination(new_cursor.position);
            }
            in_cool_down = true;
        }
    }
    
    public void SelectChess(Vector3 cursorPos)
    {
        //GameObject selectedChess = all_my_chess[0]; // By default, use the [0] chess
        AudioSource.PlayClipAtPoint(sound_effect, Camera.main.transform.position);
        if (!chess_selected)
        {
            selected_chess_index = 0;
            int index = 0;
            foreach (GameObject chess in all_my_chess)
            {
                Vector3 directionToTarget = chess.transform.position - cursorPos;
                //Vector3 directionToTarget2 = chess.transform.position - new_cursor.position;
                if (directionToTarget.sqrMagnitude < dis_threshold_sq)
                {
                    selected_chess_index = index;
                    chess_selected = true;
                    all_my_chess[selected_chess_index].transform.localScale += scaleChange;
                    //all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.cyan;
                    return;
                }
                index++;
            }
        }
    }

    public void SelectDestination(Vector3 cursorPos)
    {
        AudioSource.PlayClipAtPoint(sound_effect, Camera.main.transform.position);
        if (chess_selected) // Only after chess selected -> select destination
        {
            selected_point_index = 0;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = cursorPos;
            int index = 0;
            foreach (GameObject point in all_points)
            {
                Vector3 directionToTarget = point.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    selected_point_index = index;
                }
                index++;
            }
            // Judge whether this move is valid!
            // Display2.text = string.Format("chess:{0} goto:{1}", selected_chess_index, selected_point_index); 
            
            // Change to this cc version later!!!!!
            if (cc.ARMove(selected_chess_index, selected_point_index))
            {
                point_selected = true;
                // Turn the selected chess color back
                all_my_chess[selected_chess_index].transform.localScale -= scaleChange;
                //all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.white;                
            }
            
            // Delete this later!!!!!! See above^^^^
            //point_selected = true;
            // // Turn the selected chess color back
            //all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.white;
            
            
            //selected_point_index = 0;
            //int index = 0;
            //foreach (GameObject point in all_points)
            //{
            //    Vector3 directionToTarget = point.transform.position - cursor.position;
            //    if (directionToTarget.sqrMagnitude < dis_threshold_sq)
            //    {
            //        selected_point_index = index;
            //        point_selected = true;
            //        // Turn the selected chess color back
            //        all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.white;
            //        all_points[selected_point_index].GetComponent<MeshRenderer>().material.color = Color.red;
            //    }
            //    index++;
            //}
        }
    }

    public void ChessDie(bool this_player, int index)
    {
        //AudioSource.PlayClipAtPoint(eaten_sound_effect, Camera.main.transform.position);
        if (this_player)
        {
            all_my_chess[index].SetActive(false);
        }
        else
        {
            all_enemy_chess[index].SetActive(false);
        }
    }
    
    /*public void */
}
