using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType { King, Guard, Bishop, Knight, Cannon, Rook, Pawn}


public abstract class Chess
{
    protected ChessID cid;
    protected bool alive;
    protected Vector2Int pos;
    protected ChessType type;

    protected void init(ChessID cid_, Vector2Int pos_)
    {
        cid = cid_;
        alive = true;
        pos = pos_;
    }

    protected virtual bool isValidPos(Vector2Int pos_)
    {
        // Show Effect according to validity
        return pos_.x >= 0 && pos_.y >= 0 && pos_.x < 9 && pos_.y < 10;
    }

    public virtual bool isValidMove(Vector2Int pos_, Grid[,] board)
    {
        return isValidPos(pos_);
    }

    public bool isnotOccupied(Vector2Int pos_, Grid[,] board)
    {
        // Grid Occupied
        if (board[pos_.x, pos_.y].occupied)
        {
            // Cannot Move to grids occupied by allies
            if (board[pos_.x, pos_.y].cid.player == cid.player)
            {
                return false;
            }
            // Kill Enemy
            else return true;
        }
        // Grid not Occupied
        else return true;
    }

    public List<Vector2Int> getValidMoves(Grid[,] board)
    {
        List <Vector2Int> moves = new List<Vector2Int>();
        foreach (int x in Enumerable.Range(0, 9))
        {
            foreach (int y in Enumerable.Range(0, 10))
            {
                Vector2Int pos_ = new Vector2Int(x, y);
                if (isValidMove(pos_, board))
                {
                    moves.Add(pos_);
                }
            }
        }
        return moves;
    }

    public ChessID getChessId()
    {
        return cid;
    }

    public ChessType getType()
    {
        return type;
    }

    public bool move(Vector2Int pos_, Grid[,] board)
    {
        // Check Move Validity
        if (!isValidMove(pos_, board)) // IMPORTANT Modified: pos_ -> transformPos(pos_)
        {
            Debug.Log("InvalidMove!!!");
            return false;
        }
        // Move
        pos = pos_;
        return true;
    }

    public bool isAlive()
    {
        return alive;
    }

    public void kill()
    {
        alive = false;
    }

    public Vector2Int getPos()
    {
        return pos;
    }
}
