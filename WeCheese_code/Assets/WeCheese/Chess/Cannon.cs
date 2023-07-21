using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Chess
{
    public Cannon(ChessID cid_, Vector2Int pos_)
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
        // Move to Empty Position
        if (!board[pos_.x, pos_.y].occupied)
        {
            if (move.x == 0)
            {
                // Move Upwards
                if (move.y > 0)
                {
                    for (int i = pos.y + 1; i < pos_.y; i++)
                    {
                        Vector2Int midway = new Vector2Int(pos.x, i);
                        if (board[midway.x, midway.y].occupied)
                        {
                            return false;
                        }
                    }
                    return isnotOccupied(pos_, board);
                }
                // Move Downwards
                else if (move.y < 0)
                {
                    for (int i = pos.y - 1; i > pos_.y; i--)
                    {
                        Vector2Int midway = new Vector2Int(pos.x, i);
                        if (board[midway.x, midway.y].occupied)
                        {
                            return false;
                        }
                    }
                    return isnotOccupied(pos_, board);
                }
                else return false;
            }
            else if (move.y == 0)
            {
                // Move Rightwards
                if (move.x > 0)
                {
                    for (int i = pos.x + 1; i < pos_.x; i++)
                    {
                        Vector2Int midway = new Vector2Int(i, pos.y);
                        if (board[midway.x, midway.y].occupied)
                        {
                            return false;
                        }
                    }
                    return isnotOccupied(pos_, board);
                }
                // Move Leftwards
                else if (move.x < 0)
                {
                    for (int i = pos.x - 1; i > pos_.x; i--)
                    {
                        Vector2Int midway = new Vector2Int(i, pos.y);
                        if (board[midway.x, midway.y].occupied)
                        {
                            return false;
                        }
                    }
                    return isnotOccupied(pos_, board);
                }
                else return false;
            }
            // Invalid Movement
            else return false;
        }
        // Move to Enemy Position
        else if (board[pos_.x, pos_.y].occupied && board[pos_.x, pos_.y].cid.player != cid.player)
        {
            int count = 0;
            if (move.x == 0)
            {
                // Move Upwards
                if (move.y > 0)
                {
                    for (int i = pos.y + 1; i < pos_.y; i++)
                    {
                        Vector2Int midway = new Vector2Int(pos.x, i);
                        if (board[midway.x, midway.y].occupied)
                        {
                            count++;
                        }
                    }
                }
                else if (move.y < 0)
                {
                    for (int i = pos.y - 1; i > pos_.y; i--)
                    {
                        Vector2Int midway = new Vector2Int(pos.x, i);
                        if (board[midway.x, midway.y].occupied)
                        {
                            count++;
                        }
                    }
                }
                else return false;
            }
            else if (move.y == 0)
            {
                if (move.x > 0)
                {
                    for (int i = pos.x + 1; i < pos_.x; i++)
                    {
                        Vector2Int midway = new Vector2Int(i, pos.y);
                        if (board[midway.x, midway.y].occupied)
                        {
                            count++;
                        }
                    }
                }
                else if (move.x < 0)
                {
                    for (int i = pos.x - 1; i > pos_.x; i--)
                    {
                        Vector2Int midway = new Vector2Int(i, pos.y);
                        if (board[midway.x, midway.y].occupied)
                        {
                            count++;
                        }
                    }
                }
                else return false;
            }
            // Invalid Movement
            else return false;
            // Check Chess Number Midway
            if (count == 1)
            {
                return isnotOccupied(pos_, board);
            }
            else return false;
        }
        else return false;
    }
}
