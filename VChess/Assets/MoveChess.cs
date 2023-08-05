using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MoveChess : MonoBehaviour
{
    public Transform cursor;
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

    public TextMeshProUGUI Display2;
    void Start()
    {
        
    }

    void Update()
    {
        // Check whether game starts
        if (game_start && !ccCreated)
        {
            Debug.Log("MoveChess before cc.init");
            ccCreated = true;
            cc.init(fm.IsStartPlayer());
            // cc.init(false);
        }
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
            all_enemy_chess[opponent_chess_index].transform.Translate(0.3f * distance_vec * Time.deltaTime);  
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

    public void SelectChess()
    {
        //GameObject selectedChess = all_my_chess[0]; // By default, use the [0] chess
        if (!chess_selected)  
        { 
            selected_chess_index = 0;
            int index = 0;
            foreach (GameObject chess in all_my_chess)
            {
                Vector3 directionToTarget = chess.transform.position - cursor.position;
                if (directionToTarget.sqrMagnitude < dis_threshold_sq)
                {
                    selected_chess_index = index;
                    chess_selected = true;
                    // Make this chess bigger
                    all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.cyan;
                    return;
                }
                index++;
            }
        }
    }

    public void SelectDestination()
    {
        if (chess_selected) // Only after chess selected -> select destination
        {
            selected_point_index = 0;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = cursor.position;
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
                all_my_chess[selected_chess_index].GetComponent<MeshRenderer>().material.color = Color.white;                
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
}
