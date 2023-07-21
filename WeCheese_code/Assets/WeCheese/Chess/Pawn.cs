using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Pawn : Chess
{
    public Pawn(ChessID cid_, Vector2Int pos_)
    {
        init(cid_, pos_);
    }

    protected override bool isValidPos(Vector2Int pos_)
    {
        // Show Effect according to validity
        return pos_.x >= 0 && pos_.y >= 3 && pos_.x < 9 && pos_.y < 10;
    }

    public override bool isValidMove(Vector2Int pos_, Grid[,] board)
    {
        Debug.Log("isValidMove Reach");
        Debug.LogFormat("fromX:{0}, fromY:{1}", pos.x, pos.y);
        Debug.LogFormat("toX:{0}, toY:{1}",pos_.x, pos_.y);
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
        // If Beyond the River
        if (pos.y >= 5)
        {
            // Valid Movement
            if (move == Vector2Int.up || move == Vector2Int.left || move == Vector2Int.right)
            {
                return isnotOccupied(pos_, board);
            }
            // Invalid Movement
            else return false;
        }
        // If on the Self Side
        else 
        {
            // Valid Movement
            if (move == Vector2Int.up)
            {
                return isnotOccupied(pos_, board);
            }
            // Invalid Movement
            else return false;
        }
    }
}
