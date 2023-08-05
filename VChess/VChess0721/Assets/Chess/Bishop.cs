using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chess
{
    public Bishop(ChessID cid_, Vector2Int pos_)
    {
        init(cid_, pos_);
    }

    protected override bool isValidPos(Vector2Int pos_)
    {
        // Show Effect according to validity
        if (!cid.player)
        {
            return pos_.x >= 0 && pos_.y >= 5 && pos_.x < 9 && pos_.y < 10;
        }
        return pos_.x >= 0 && pos_.y >= 0 && pos_.x < 9 && pos_.y < 5;
    }

    public override bool isValidMove(Vector2Int pos_, Grid[,] board)
    {
        // Cannot Move Dead Chess
        if (!isAlive())
        {
            return false;
        }
        // Cannot Move Chess to Invalid Positions
        if (!isValidPos(pos_))
        {
            return false;
        }
        Vector2Int move = pos_ - pos;
        // Valid Movement
        if (Mathf.Abs(move.x) == 2 && Mathf.Abs(move.y) == 2)
        {
            Vector2Int midway = pos + (move / 2);
            if (!board[midway.x, midway.y].occupied)
            {
                return isnotOccupied(pos_, board);
            }
            else return false;
        }
        // Invalid Movement
        else return false;
    }
}
