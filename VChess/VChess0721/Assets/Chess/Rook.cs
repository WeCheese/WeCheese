using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chess
{
    public Rook(ChessID cid_, Vector2Int pos_)
    {
        init(cid_, pos_);
        Debug.LogFormat("curChessPos:{0},{1}", pos.x,pos.y);
    }

    public override bool isValidMove(Vector2Int pos_, Grid[,] board)
    {
        Debug.Log("isValidMove Reach");
        Debug.LogFormat("fromX:{0}, fromY:{1}", pos.x, pos.y);
        Debug.LogFormat("toX:{0}, toY:{1}",pos_.x, pos_.y);
        // Cannot Move Dead Chess
        if (!isAlive())
        {
            Debug.Log("NotAlive");
            return false;
        }
        // Cannot Move Chess to Invalid Positions
        if (!isValidPos(pos_))
        {
            Debug.Log("Move to Invalid pos");
            return false;
        }
        Vector2Int move = pos_ - pos;
        // Valid Movement
        if (move.x == 0)
        {
            // Move Upwards
            if (move.y > 0)
            {
                Debug.Log("MoveUp Reach");
                for (int i = pos.y + 1; i < pos_.y; i++)
                {
                    Vector2Int midway = new Vector2Int(pos.x, i);
                    if (board[midway.x, midway.y].occupied)
                    {
                        Debug.Log("Midway Occupied(up)");
                        return false;
                    }
                }
            }
            // Move Downwards
            else if (move.y < 0)
            {
                Debug.Log("MoveDown Reach");
                for (int i = pos.y - 1; i > pos_.y; i--)
                {
                    Vector2Int midway = new Vector2Int(pos.x, i);
                    if (board[midway.x, midway.y].occupied)
                    {
                        Debug.Log("Midway Occupied(down)");
                        return false;
                    }
                }
            }
            else return false;
        }
        else if (move.y == 0)
        {
            // Move Rightwards
            if (move.x > 0)
            {
                Debug.Log("MoveRight Reach");
                for (int i = pos.x + 1; i < pos_.x; i++)
                {
                    Vector2Int midway = new Vector2Int(i, pos.y);
                    if (board[midway.x, midway.y].occupied)
                    {
                        Debug.Log("Midway Occupied(right)");
                        return false;
                    }
                }
            }
            // Move Leftwards
            else if (move.x < 0)
            {
                Debug.Log("MoveLeft Reach");
                for (int i = pos.x - 1; i > pos_.x; i--)
                {
                    Vector2Int midway = new Vector2Int(i, pos.y);
                    if (board[midway.x, midway.y].occupied)
                    {
                        Debug.Log("Midway Occupied(left)");
                        return false;
                    }
                }
            }
            else return false;
        }
        // Invalid Movement
        else
        {
            Debug.Log("Invalid Movement of Rook");
            return false;
        }
        // Check Target Position
        Debug.Log("Check target pos occupied");
        return isnotOccupied(pos_, board);
    }
}
