using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using TMPro;

public struct ChessID
{
    public bool player;
    public int id;

    public ChessID(bool player_, int id_)
    {
        player = player_;
        id = id_;
    }
}

public struct Grid
{
    public bool occupied;
    public ChessID cid;

    // Struct pre-zeroed by default
    public Grid(bool occupied_, bool player_, int id_)
    {
        occupied = occupied_;
        cid = new ChessID(player_, id_);
    }
}


public class ChessController : MonoBehaviour
{
    private Grid[,] board = new Grid[9, 10];
    private bool player = true;
    private bool turn = false;
    private Dictionary<ChessID, Chess> chessDict;
    private List<Dictionary<string, object>> chessHist = new List<Dictionary<string, object>>();
    private int histPtr;

    public FirebaseManager fm;
    public MoveChess mc;
    
    public TextMeshProUGUI Displaya;

    public void init(bool player_, bool resume = false)
    {
        Debug.Log("Before init brd");
        initBoard();
        Debug.Log("After init brd");
        player = player_;
        turn = player_;
        mc.is_first = player_;
        histPtr = 0;
        Debug.Log("Before registering updatemovecallback");
        fm.UpdateMoveCallbackRegisterAsync(oppoMove);
        Debug.Log("Finish init");
    }

    public void getHist(List<Dictionary<string, object>> chessHist_)
    {
        chessHist = chessHist_;
    }

    public void resumeGame()
    {
        while (histPtr < chessHist.Count)
        {
            Debug.Log("CC actually resuming game");
            // Read From, To, Update User
            Vector2Int from = Int_to_Vector(Convert.ToInt32(chessHist[histPtr]["from"]));
            Vector2Int to = Int_to_Vector(Convert.ToInt32(chessHist[histPtr]["to"]));
            bool mover = Convert.ToBoolean(chessHist[histPtr]["update_user"]);
            // Move
            Chess chess = getChess(from);
            chess.move(to, board);
            // Check Killing
            if (board[to.x, to.y].occupied)
            {
                chessDict[board[to.x, to.y].cid].kill();
            }
            // Update Board
            board[from.x, from.y].occupied = false;
            board[to.x, to.y].occupied = true;
            board[to.x, to.y].cid = chess.getChessId();
            Debug.Log(" resume chess.move Succeed!!!");
            Displaya.text = " resume qq Receive Other Move 2";
            if (mover)
            {
                turn = false;
            }
            else
            {
                turn = true;
            }

            histPtr++;
        }
        // Render the Chess Board
        for (int i = 0; i < 16; i++)
        {
            Chess chess = chessDict[new ChessID(player, i)];
            if (chess.isAlive())
            {
                mc.all_my_chess[i].transform.position = mc.all_points[Vector_to_Int(transformPos(chess.getPos()))].transform.position;
            }
            else
            {
                mc.ChessDie(true, i);
            }
        }
        for (int i = 0; i < 16; i++)
        {
            Chess chess = chessDict[new ChessID(!player, i)];
            if (chess.isAlive())
            {
                mc.all_enemy_chess[i].transform.position = mc.all_points[Vector_to_Int(transformPos(chess.getPos()))].transform.position;
            }
            else
            {
                mc.ChessDie(false, i);
            }
        }
    }

    public void reviewStep()
    {
        if (histPtr >= chessHist.Count)
        {
            return;
        }
        // Read From, To, Update User
        Vector2Int from = Int_to_Vector(Convert.ToInt32(chessHist[histPtr]["from"]));
        Vector2Int to = Int_to_Vector(Convert.ToInt32(chessHist[histPtr]["to"]));
        bool mover = Convert.ToBoolean(chessHist[histPtr]["update_user"]);
        // Move
        Chess chess = getChess(from);
        chess.move(to, board);
        if (mover)
        {
            mc.all_my_chess[chess.getChessId().id].transform.position = mc.all_points[Vector_to_Int(transformPos(chess.getPos()))].transform.position;
            // Render on ChessBoard
            // Check Killing
            if (board[to.x, to.y].occupied)
            {
                chessDict[board[to.x, to.y].cid].kill();
                mc.ChessDie(false, board[to.x, to.y].cid.id);
            }
        }
        else
        {
            mc.all_enemy_chess[chess.getChessId().id].transform.position = mc.all_points[Vector_to_Int(transformPos(chess.getPos()))].transform.position;
            // Render on ChessBoard
            // Check Killing
            if (board[to.x, to.y].occupied)
            {
                chessDict[board[to.x, to.y].cid].kill();
                mc.ChessDie(true, board[to.x, to.y].cid.id);
            }
        }
        // Update Board
        board[from.x, from.y].occupied = false;
        board[to.x, to.y].occupied = true;
        board[to.x, to.y].cid = chess.getChessId();
        Debug.Log("chess.move Succeed!!!");
        Displaya.text = "qq Receive Other Move 2";
        histPtr++;
    }

    

    private int Vector_to_Int(Vector2Int vector)
    {
        return 9 * vector.y + vector.x;
    }

    private Vector2Int Int_to_Vector(int i)
    {
        int y = i / 9;
        int x = i - 9 * y;
        return new Vector2Int(x, y);
    }

    public bool ARMove(int id, int to_)
    {
        Vector2Int from = chessDict[new ChessID(player, id)].getPos();
        Vector2Int to = transformPos(Int_to_Vector(to_));
        if (move(from, to))
        {
            // Send Signal to Back End
            bool flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    fm.MoveChess(Vector_to_Int(from), Vector_to_Int(to));
                }
                catch (Exception e)
                {
                    flag = true;
                }
            }
            return true;
        }
        return false;
    }

    public bool move(Vector2Int from, Vector2Int to)
    {
        // Check Turn
        if (!turn)
        {
            Displaya.text = "NOT YOUR TURN!";
            return false;
        }
        if (from == to)
        {
            Displaya.text = "CANNOT MOVE TO ORIGINAL POSITION!";
            return false;
        }
        // Check Validity of From
        if (!board[from.x, from.y].occupied || board[from.x, from.y].cid.player != player)
        {
            Displaya.text = "CANNOT MOVE EMPTY GRID / OTHER'S CHESS!";
            return false;
        }
        // Move Chess
        Chess chess = getChess(from);
        if (chess.move(to, board))
        {
            // Check Killing
            if (board[to.x, to.y].occupied && board[to.x, to.y].cid.player != player)
            {
                ChessID cid = board[to.x, to.y].cid;
                chessDict[cid].kill();
                mc.ChessDie(false, cid.id);
            }
            // Update Board
            board[from.x, from.y].occupied = false;
            board[to.x, to.y].occupied = true;
            board[to.x, to.y].cid = chess.getChessId();
            // End Turn
            turn = false;
            return true;
        }
        else
        {
            Displaya.text = "INVALID MOVE!";
            return false;
        }
    }

    public bool oppoMove(int from_, int to_)
    {
        Debug.Log("oppoMove REACH!!!");
        Displaya.text = "qq Receive Other Move 1";
        Vector2Int from = Int_to_Vector(from_);
        Vector2Int to = Int_to_Vector(to_);
        // Check Turn
        if (turn)
        {
            Debug.Log("NOT YOUR TURN!");
            Displaya.text = "NOT YOUR TURN!";
            return false;
        }
        if (from == to)
        {
            Debug.Log("from = to!");
            Displaya.text = "from = to!";
            return false;
        }
        // Check Validity of From
        if (!board[from.x, from.y].occupied || board[from.x, from.y].cid.player == player)
        {
            Debug.Log("CANNOT MOVE EMPTY GRID / OTHER'S CHESS!");
            Debug.Log(!board[from.x, from.y].occupied);
            Debug.Log(board[from.x, from.y].cid.player == player);
            Displaya.text = "CANNOT MOVE EMPTY GRID / OTHER'S CHESS!";
            /*return false;*/
        }
        // Move Chess
        // This output is right
        Debug.LogFormat("getChessfrom:{0}, {1}", from.x, from.y);
        Debug.LogFormat("getChessto:{0}, {1}", to.x, to.y);
        Debug.LogFormat("getChessPlayer:{0}", player); // player should be false
        Debug.LogFormat("test:{0}, {1}", to.x, to.y);
        Chess chess = getChess(from);
        if (chess.move(to, board))
        {
            // Check Killing
            if (board[to.x, to.y].occupied && board[to.x, to.y].cid.player == player)
            {
                ChessID cid = board[to.x, to.y].cid;
                chessDict[cid].kill();
                mc.ChessDie(true, cid.id);
            }
            // Update Board
            board[from.x, from.y].occupied = false;
            board[to.x, to.y].occupied = true;
            board[to.x, to.y].cid = chess.getChessId();
            Debug.Log("chess.move Succeed!!!");
            Displaya.text = "qq Receive Other Move 2";
            // Render Move on AR Board
            mc.ShowOpponentMove(chess.getChessId().id, Vector_to_Int(transformPos(to)));
            // Begin Turn
            turn = true;
            return true;
        }
        else
        {
            Displaya.text = "qq Fail Receive Other Move 2";
            return false;
        }
    }

    public Chess getChess(Vector2Int pos)
    {
        // Reference not set to an instance of object?
        // This output is right
        Debug.LogFormat("posX:{0}", pos.x);
        Debug.LogFormat("posY:{0}", pos.y);
        // Modified
        ChessID targetID = board[pos.x, pos.y].cid;
        Debug.LogFormat("player:{0}", targetID.player);
        Debug.LogFormat("chessID:{0}", targetID.id);
        if (chessDict == null)
        {
            Debug.Log("chessDict NULL!!!");
            throw new ArgumentNullException(nameof(chessDict));
        }
        if (ReferenceEquals(targetID, null))
        {
            throw new ArgumentNullException(nameof(targetID));
        }
        if (chessDict.TryGetValue(targetID, out Chess targetChess))
        {
            Debug.Log("getChess");
            // This output is wrong
            Debug.LogFormat("TargetChess x:{0}, y:{1}", targetChess.getPos().x, targetChess.getPos().y);
            Debug.LogFormat("TargetChess player:{0}", targetChess.getChessId().player);
            return targetChess;
        }
        Debug.Log("getChess Fail");
        // return default chess if not get chess
        return new King(new ChessID(player, 0), new Vector2Int(4, 0));

        //
        // return chessDict[board[pos.x, pos.y].cid];
    }

    public Vector2Int transformPos(Vector2Int pos)
    {
        if (player)
        {
            return pos;
        }
        else return new Vector2Int(8, 9) - pos;
    }

    public Vector2Int rotate(Vector2Int pos)
    {
        return new Vector2Int(8, 9) - pos;
    }

    public bool getTurn()
    {
        return turn;
    }

    public bool win(bool player)
    {
        if (!chessDict[new ChessID(!player, 0)].isAlive())
        {
            return true;
        }
        else return false;
    }

    public bool gameOver()
    {
        if (!chessDict[new ChessID(true, 0)].isAlive() || !chessDict[new ChessID(false, 0)].isAlive())
        {
            return true;
        }
        else return false;
    }

    private void initBoard()
    {
        chessDict = new Dictionary<ChessID, Chess> {
            // This Player
            // King
            { new ChessID(true, 0), new King(new ChessID(true, 0), new Vector2Int(4, 0))},
            // Guard
            { new ChessID(true, 1), new Guard(new ChessID(true, 1), new Vector2Int(3, 0))},
            { new ChessID(true, 2), new Guard(new ChessID(true, 2), new Vector2Int(5, 0))},
            // Bishop
            { new ChessID(true, 3), new Bishop(new ChessID(true, 3), new Vector2Int(2, 0))},
            { new ChessID(true, 4), new Bishop(new ChessID(true, 4), new Vector2Int(6, 0))},
            // Knight
            { new ChessID(true, 5), new Knight(new ChessID(true, 5), new Vector2Int(1, 0))},
            { new ChessID(true, 6), new Knight(new ChessID(true, 6), new Vector2Int(7, 0))},
            // Cannon
            { new ChessID(true, 7), new Cannon(new ChessID(true, 7), new Vector2Int(1, 2))},
            { new ChessID(true, 8), new Cannon(new ChessID(true, 8), new Vector2Int(7, 2))},
            // Rook
            { new ChessID(true, 9), new Rook(new ChessID(true, 9), new Vector2Int(0, 0))},
            { new ChessID(true, 10), new Rook(new ChessID(true, 10), new Vector2Int(8, 0))},
            // Pawn
            { new ChessID(true, 11), new Pawn(new ChessID(true, 11), new Vector2Int(0, 3))},
            { new ChessID(true, 12), new Pawn(new ChessID(true, 12), new Vector2Int(2, 3))},
            { new ChessID(true, 13), new Pawn(new ChessID(true, 13), new Vector2Int(4, 3))},
            { new ChessID(true, 14), new Pawn(new ChessID(true, 14), new Vector2Int(6, 3))},
            { new ChessID(true, 15), new Pawn(new ChessID(true, 15), new Vector2Int(8, 3))},
            // Other Player
            // King
            { new ChessID(false, 0), new King(new ChessID(false, 0), rotate(new Vector2Int(4, 0)))},
            // Guard
            { new ChessID(false, 1), new Guard(new ChessID(false, 1), rotate(new Vector2Int(3, 0)))},
            { new ChessID(false, 2), new Guard(new ChessID(false, 2), rotate(new Vector2Int(5, 0)))},
            // Bishop
            { new ChessID(false, 3), new Bishop(new ChessID(false, 3), rotate(new Vector2Int(2, 0)))},
            { new ChessID(false, 4), new Bishop(new ChessID(false, 4), rotate(new Vector2Int(6, 0)))},
            // Knight
            { new ChessID(false, 5), new Knight(new ChessID(false, 5), rotate(new Vector2Int(1, 0)))},
            { new ChessID(false, 6), new Knight(new ChessID(false, 6), rotate(new Vector2Int(7, 0)))},
            // Cannon
            { new ChessID(false, 7), new Cannon(new ChessID(false, 7), rotate(new Vector2Int(1, 2)))},
            { new ChessID(false, 8), new Cannon(new ChessID(false, 8), rotate(new Vector2Int(7, 2)))},
            // Rook
            { new ChessID(false, 9), new Rook(new ChessID(false, 9), rotate(new Vector2Int(0, 0)))},
            { new ChessID(false, 10), new Rook(new ChessID(false, 10), rotate(new Vector2Int(8, 0)))},
            // Pawn
            { new ChessID(false, 11), new Pawn(new ChessID(false, 11), rotate(new Vector2Int(0, 3)))},
            { new ChessID(false, 12), new Pawn(new ChessID(false, 12), rotate(new Vector2Int(2, 3)))},
            { new ChessID(false, 13), new Pawn(new ChessID(false, 13), rotate(new Vector2Int(4, 3)))},
            { new ChessID(false, 14), new Pawn(new ChessID(false, 14), rotate(new Vector2Int(6, 3)))},
            { new ChessID(false, 15), new Pawn(new ChessID(false, 15), rotate(new Vector2Int(8, 3)))},
        };
        // Player 1
        // King
        board[4, 0] = new Grid(true, true, 0);
        // Guard
        board[3, 0] = new Grid(true, true, 1);
        board[5, 0] = new Grid(true, true, 2);
        // Bishop
        board[2, 0] = new Grid(true, true, 3);
        board[6, 0] = new Grid(true, true, 4);
        // Knight
        board[1, 0] = new Grid(true, true, 5);
        board[7, 0] = new Grid(true, true, 6);
        // Cannon
        board[1, 2] = new Grid(true, true, 7);
        board[7, 2] = new Grid(true, true, 8);
        // Rook
        board[0, 0] = new Grid(true, true, 9);
        board[8, 0] = new Grid(true, true, 10);
        // Pawn
        board[0, 3] = new Grid(true, true, 11);
        board[2, 3] = new Grid(true, true, 12);
        board[4, 3] = new Grid(true, true, 13);
        board[6, 3] = new Grid(true, true, 14);
        board[8, 3] = new Grid(true, true, 15);
        // Player 2
        // King
        board[4, 9] = new Grid(true, false, 0);
        // Guard
        board[5, 9] = new Grid(true, false, 1);
        board[3, 9] = new Grid(true, false, 2);
        // Bishop
        board[6, 9] = new Grid(true, false, 3);
        board[2, 9] = new Grid(true, false, 4);
        // Knight
        board[7, 9] = new Grid(true, false, 5);
        board[1, 9] = new Grid(true, false, 6);
        // Cannon
        board[7, 7] = new Grid(true, false, 7);
        board[1, 7] = new Grid(true, false, 8);
        // Rook
        board[8, 9] = new Grid(true, false, 9);
        board[0, 9] = new Grid(true, false, 10);
        // Pawn
        board[8, 6] = new Grid(true, false, 11);
        board[6, 6] = new Grid(true, false, 12);
        board[4, 6] = new Grid(true, false, 13);
        board[2, 6] = new Grid(true, false, 14);
        board[0, 6] = new Grid(true, false, 15);
    }
}
