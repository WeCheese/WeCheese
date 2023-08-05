using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public King(ChessID cid_, Vector2Int pos_)
    {
        init(cid_, pos_);
    }

    protected override bool isValidPos(Vector2Int pos_)
    {
        // Show Effect according to validity
        if (!cid.player)
        {
            return pos_.x >= 3 && pos_.y >= 7 && pos_.x <= 5 && pos_.y < 10;
        }
        return pos_.x >= 3 && pos_.y >= 0 && pos_.x <= 5 && pos_.y < 3;
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
        if (move == Vector2Int.up || move == Vector2Int.down || move == Vector2Int.left || move == Vector2Int.right)
        {
            return isnotOccupied(pos_, board);
        }
        // Invalid Movement
        else return false;
    }
}
