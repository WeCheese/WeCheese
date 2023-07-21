using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chess
{
    public Knight(ChessID cid_, Vector2Int pos_)
    {
        init(cid_, pos_);
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
        if (Mathf.Abs(move.x) == 2 && Mathf.Abs(move.y) == 1)
        {
            Vector2Int midway = pos + (new Vector2Int(move.x, 0) / 2);
            if (!board[midway.x, midway.y].occupied)
            {
                return isnotOccupied(pos_, board);
            }
            else return false;
        }
        else if (Mathf.Abs(move.x) == 1 && Mathf.Abs(move.y) == 2)
        {
            Vector2Int midway = pos + (new Vector2Int(0, move.y) / 2);
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
