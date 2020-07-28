using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pieces
{
    Null,
    WhitePawn,
    WhiteRook,
    WhiteBishop,
    WhiteKnight,
    WhiteQueen,
    WhiteKing,
    BlackPawn,
    BlackRook,
    BlackBishop,
    BlackKnight,
    BlackQueen,
    BlackKing
}

public enum Color
{
    White, Black
}

public class BoardManager : MonoBehaviour
{
    private Pieces[] board = new Pieces[64];
    private GameManager gm;

    private bool castlingAvailableBlack = true;
    private bool castlingAvailableWhite = true;

    // Start is called before the first frame update
    private void Start()
    {
        setupBoard();
        gm = FindObjectOfType<GameManager>();
    }

    private void setupBoard()
    {
        for (int i = 8; i < 16; ++i)
        {
            board[i] = Pieces.WhitePawn;
        }
        for (int i = 48; i < 56; ++i)
        {
            board[i] = Pieces.BlackPawn;
        }

        board[0] = Pieces.WhiteRook;
        board[1] = Pieces.WhiteKnight;
        board[2] = Pieces.WhiteBishop;
        board[3] = Pieces.WhiteQueen;
        board[4] = Pieces.WhiteKing;
        board[5] = Pieces.WhiteBishop;
        board[6] = Pieces.WhiteKnight;
        board[7] = Pieces.WhiteRook;

        for (int i = 16; i < 48; ++i)
        {
            board[i] = Pieces.Null;
        }

        board[56] = Pieces.BlackRook;
        board[57] = Pieces.BlackKnight;
        board[58] = Pieces.BlackBishop;
        board[59] = Pieces.BlackQueen;
        board[60] = Pieces.BlackKing;
        board[61] = Pieces.BlackBishop;
        board[62] = Pieces.BlackKnight;
        board[63] = Pieces.BlackRook;
    }

    public bool Move(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces p = Get(src_x, src_y);
        if ((gm.WhiteTurn() && IsWhite(p)) || (!gm.WhiteTurn() && IsBlack(p)))
        {
            if (p == Pieces.BlackBishop || p == Pieces.WhiteBishop)
            {
                bool res = BishopMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));
                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }

                return res;
            }
            else if (p == Pieces.BlackRook || p == Pieces.WhiteRook)
            {
                bool res = RookMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));

                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }
                return res;
            }
            else if (p == Pieces.BlackKing || p == Pieces.WhiteKing)
            {
                bool res = KingMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));
                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }
                return res;
            }
            else if (p == Pieces.BlackQueen || p == Pieces.WhiteQueen)
            {
                bool res = QueenMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));
                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }
                return res;
            }
            else if (p == Pieces.BlackPawn || p == Pieces.WhitePawn)
            {
                bool res = PawnMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));
                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }
                return res;
            }
            else if (p == Pieces.BlackKnight || p == Pieces.WhiteKnight)
            {
                bool res = KnightMove(src_x, src_y, dest_x, dest_y, out eaten);
                if (res)
                {
                    if (CheckForChecks(GetOtherColor(p)))
                    {
                        if (CheckMate(GetOtherColor(p)))
                        {
                            gm.EndGameCheckmate(GetColor(p));
                        }
                    }
                    else
                    {
                        if (StaleMate(GetOtherColor(p)))
                        {
                            gm.EndGameDraw(GetColor(p), "Stalemate");
                        }
                    }
                    gm.SwitchTurn();
                }
                return res;
            }
            else
            {
                eaten = false;
                return false;
            }
        }
        else
        {
            eaten = false;
            return false;
        }
    }

    private Pieces Get(int x, int y)
    {
        //ISSUE HERE
        if (x < 0 || x > 7 || y < 0 || y > 7)
            throw new System.IndexOutOfRangeException();
        return board[8 * y + x];
    }

    private void Set(int x, int y, Pieces val)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7)
            throw new System.IndexOutOfRangeException();
        board[8 * y + x] = val;
    }

    private int Min(int x1, int x2)
    {
        if (x1 < x2)
            return x1;
        else
            return x2;
    }

    private int Max(int x1, int x2)
    {
        if (x1 > x2)
            return x1;
        else
            return x2;
    }

    private bool IsWhite(Pieces p)
    {
        return (int)p > 0 && (int)p < 7;
    }

    private bool IsBlack(Pieces p)
    {
        return (int)p > 6 && (int)p < 13;
    }

    private bool CheckForChecks(Color color)
    {
        int king_x = -1;
        int king_y = -1;
        Pieces King = color == Color.White ? Pieces.WhiteKing : Pieces.BlackKing;

        for (int x = 0; x < 8 && king_x < 0; ++x)
        {
            for (int y = 0; y < 8; ++y)
            {
                if (Get(x, y) == King)
                {
                    king_x = x;
                    king_y = y;
                    break;
                }
            }
        }

        for (int x = 0; x < 8; ++x)
        {
            for (int y = 0; y < 8; ++y)
            {
                if (GetColor(Get(x, y)) != color)
                {
                    if (IsValidMove(x, y, king_x, king_y))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    private Color GetColor(Pieces p)
    {
        return IsWhite(p) ? Color.White : Color.Black;
    }

    private Color GetOtherColor(Pieces p)
    {
        return IsWhite(p) ? Color.Black : Color.White;
    }

    private bool StaleMate(Color color)
    {
        return CheckMate(color);
    }

    private bool CastlingAvailable(Color color)
    {
        if (color == Color.Black)
            return castlingAvailableBlack;
        else
            return castlingAvailableWhite;
    }

    private bool CheckMate(Color color)
    {
        for (int src_x = 0; src_x < 8; ++src_x)
        {
            for (int src_y = 0; src_y < 8; ++src_y)
            {
                if (GetColor(Get(src_x, src_y)) == color)
                {
                    for (int dest_x = 0; dest_x < 8; ++dest_x)
                    {
                        for (int dest_y = 0; dest_y < 8; ++dest_y)
                        {
                            if (IsValidMove(src_x, src_y, dest_x, dest_y))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    private bool CheckEndPosition(int src_x, int src_y, int dest_x, int dest_y, Pieces this_piece)
    {
        if (Get(dest_x, dest_y) != Pieces.Null)
        {
            //possible refactoring --> if(IsWhite(this) && IsWhite(enemy) || IsBlack(this) && IsBlack(enemy)) return false, else...
            if (IsWhite(this_piece))
            {
                if (IsWhite(Get(dest_x, dest_y)))
                    return false;
                else
                {
                    Pieces enemy = Get(dest_x, dest_y);
                    Set(dest_x, dest_y, this_piece);
                    Set(src_x, src_y, Pieces.Null);

                    if (CheckForChecks(Color.White))
                    {
                        Set(dest_x, dest_y, enemy);
                        Set(src_x, src_y, this_piece);
                        return false;
                    }
                    else
                    {
                        Set(dest_x, dest_y, enemy);
                        Set(src_x, src_y, this_piece);
                        return true;
                    }
                }
            }
            else
            {
                if (IsBlack(Get(dest_x, dest_y)))
                    return false;
                else
                {
                    Pieces enemy = Get(dest_x, dest_y);
                    Set(dest_x, dest_y, this_piece);
                    Set(src_x, src_y, Pieces.Null);

                    if (CheckForChecks(Color.Black))
                    {
                        Set(dest_x, dest_y, enemy);
                        Set(src_x, src_y, this_piece);
                        return false;
                    }
                    else
                    {
                        Set(dest_x, dest_y, enemy);
                        Set(src_x, src_y, this_piece);
                        return true;
                    }
                }
            }
        }
        else
        {
            Color myColor = GetColor(this_piece);

            Set(dest_x, dest_y, this_piece);
            Set(src_x, src_y, Pieces.Null);

            if (CheckForChecks(myColor))
            {
                Set(dest_x, dest_y, Pieces.Null);
                Set(src_x, src_y, this_piece);
                return false;
            }
            else
            {
                Set(dest_x, dest_y, Pieces.Null);
                Set(src_x, src_y, this_piece);
                return true;
            }
        }
    }

    private bool FirstMove(int y, Color color)
    {
        if (color == Color.White)
        {
            return y == 1;
        }
        else
        {
            return y == 6;
        }
    }

    private bool IsValidMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        if (src_x < 0 || src_x > 7 || src_y < 0 || src_y > 7 || dest_x < 0 || dest_x > 7 || dest_y < 0 || dest_y > 7)
            return false;
        Pieces p = Get(src_x, src_y);
        if (p == Pieces.BlackBishop || p == Pieces.WhiteBishop)
            return ValidBishopMove(src_x, src_y, dest_x, dest_y);
        else if (p == Pieces.BlackRook || p == Pieces.WhiteRook)
            return ValidRookMove(src_x, src_y, dest_x, dest_y);
        else if (p == Pieces.BlackKing || p == Pieces.WhiteKing)
        {
            if (src_x == 4 && src_y == 0  && GetColor(Get(src_x, src_y)) == Color.White && CastlingAvailable(Color.White))
            {
                if(dest_x == 6 && dest_y == 0)
                {
                    Debug.Log("Here");

                    return ValidKingMove(4, 0, 5, 0) && ValidKingMove(5, 0, 6, 0);

                }
                else if(dest_x == 2 && dest_y == 0)
                {
                    return ValidKingMove(4, 0, 3, 0) && ValidKingMove(3, 0, 2, 0);
                }

                return ValidKingMove(src_x, src_y, dest_x, dest_y);
            }
            else if (src_x == 4 && src_y == 7 && GetColor(Get(src_x, src_y)) == Color.Black && CastlingAvailable(Color.Black)) 
            {
                if (dest_x == 6 && dest_y == 7)
                {
                    return ValidKingMove(4, 7, 5, 7) && ValidKingMove(5, 7, 6, 7);

                }
                else if (dest_x == 2 && dest_y == 7)
                {
                    return ValidKingMove(4, 7, 3, 7) && ValidKingMove(3, 7, 2, 7);
                }
                return ValidKingMove(src_x, src_y, dest_x, dest_y);
            }
            else
            {
                return ValidKingMove(src_x, src_y, dest_x, dest_y);
            }
        }
        else if (p == Pieces.BlackQueen || p == Pieces.WhiteQueen)
            return ValidQueenMove(src_x, src_y, dest_x, dest_y);
        else if (p == Pieces.BlackPawn || p == Pieces.WhitePawn)
            return ValidPawnMove(src_x, src_y, dest_x, dest_y);
        else if (p == Pieces.BlackKnight || p == Pieces.WhiteKnight)
            return ValidKnightMove(src_x, src_y, dest_x, dest_y);
        else
            return false;
    }

    private bool PawnMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackPawn && this_piece != Pieces.WhitePawn)
        {
            eaten = false;
            return false;
        }
        else
        {
            if (ValidPawnMove(src_x, src_y, dest_x, dest_y))
            {
                if (Get(dest_x, dest_y) != Pieces.Null)
                    eaten = true;
                else
                    eaten = false;
                Set(dest_x, dest_y, this_piece);
                Set(src_x, src_y, Pieces.Null);
                return true;
            }
            else
            {
                eaten = false;
                return false;
            }
        }
    }

    private bool ValidPawnMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        Pieces this_piece = Get(src_x, src_y);

        if (IsWhite(this_piece))
        {
            if (FirstMove(src_y, Color.White) && src_x == dest_x && src_y != dest_y && dest_y - src_y <= 2 && Get(dest_x, dest_y) == Pieces.Null)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else if (Get(dest_x, dest_y) != Pieces.Null && Mathf.Abs(dest_x - src_x) == 1 && dest_y - src_y == 1)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else if (src_x == dest_x && dest_y - src_y == 1 && Get(dest_x, dest_y) == Pieces.Null)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (FirstMove(src_y, Color.Black) && src_x == dest_x && src_y != dest_y && src_y - dest_y <= 2 && Get(dest_x, dest_y) == Pieces.Null)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else if (Get(dest_x, dest_y) != Pieces.Null && Mathf.Abs(dest_x - src_x) == 1 && src_y - dest_y == 1)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else if (src_x == dest_x && src_y - dest_y == 1 && Get(dest_x, dest_y) == Pieces.Null)
            {
                return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
            }
            else
            {
                return false;
            }
        }
    }

    private bool BishopMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackBishop && this_piece != Pieces.WhiteBishop)
        {
            eaten = false;
            return false;
        }
        else
        {
            if (ValidBishopMove(src_x, src_y, dest_x, dest_y))
            {
                if (Get(dest_x, dest_y) != Pieces.Null)
                    eaten = true;
                else
                    eaten = false;
                Set(dest_x, dest_y, this_piece);
                Set(src_x, src_y, Pieces.Null);
                return true;
            }
            else
            {
                eaten = false;
                return false;
            }
        }
    }

    private bool ValidBishopMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        Pieces this_piece = Get(src_x, src_y);

        int x_diff = Math.Sign(dest_x - src_x);
        int y_diff = Math.Sign(dest_y - src_y);

        if (Mathf.Abs(src_x - dest_x) == Mathf.Abs(src_y - dest_y) && !(src_x == dest_x && src_y == dest_y))
        {
            for (int i = 1; i < Mathf.Abs(dest_x - src_x); i++)
            {
                if (Get(src_x + i * x_diff, src_y + i * y_diff) != Pieces.Null)
                    return false;
            }
        }
        else
        {
            return false;
        }

        return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
    }

    private bool RookMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackRook && this_piece != Pieces.WhiteRook)
        {
            eaten = false;
            return false;
        }
        else
        {
            if (ValidRookMove(src_x, src_y, dest_x, dest_y))
            {
                if (Get(dest_x, dest_y) != Pieces.Null)
                    eaten = true;
                else
                    eaten = false;
                Set(dest_x, dest_y, this_piece);
                Set(src_x, src_y, Pieces.Null);

                return true;
            }
            else
            {
                eaten = false;
                return false;
            }
        }
    }

    private bool ValidRookMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        Pieces this_piece = Get(src_x, src_y);

        int x_diff = Math.Sign(dest_x - src_x);
        int y_diff = Math.Sign(dest_y - src_y);

        if (x_diff == 0 ^ y_diff == 0)
        {
            for (int i = 1; i < Max(Mathf.Abs(dest_x - src_x), Mathf.Abs(dest_y - src_y)); i++)
            {
                if (Get(src_x + i * x_diff, src_y + i * y_diff) != Pieces.Null)
                    return false;
            }
        }
        else
        {
            return false;
        }

        return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
    }

    private bool ValidKnightMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        Pieces this_piece = Get(src_x, src_y);

        if (src_x == dest_x && src_y == dest_y)
            return false;

        if (!(Mathf.Abs(src_x - dest_x)
                + Mathf.Abs(src_y - dest_y) == 3
                && Mathf.Abs(src_x - dest_x) != 0
                && Mathf.Abs(src_y - dest_y) != 0))
            return false;

        return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
    }

    private bool KnightMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackKnight && this_piece != Pieces.WhiteKnight)
        {
            eaten = false;
            return false;
        }

        if (ValidKnightMove(src_x, src_y, dest_x, dest_y))
        {
            if (Get(dest_x, dest_y) != Pieces.Null)
                eaten = true;
            else
                eaten = false;
            Set(dest_x, dest_y, this_piece);
            Set(src_x, src_y, Pieces.Null);
            return true;
        }
        else
        {
            eaten = false;
            return false;
        }
    }

    private bool ValidQueenMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        if (ValidRookMove(src_x, src_y, dest_x, dest_y) || ValidBishopMove(src_x, src_y, dest_x, dest_y))
            return CheckEndPosition(src_x, src_y, dest_x, dest_y, Get(src_x, src_y));
        else
            return false;
    }

    private bool QueenMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackQueen && this_piece != Pieces.WhiteQueen)
        {
            eaten = false;
            return false;
        }

        if (ValidQueenMove(src_x, src_y, dest_x, dest_y))
        {
            if (Get(dest_x, dest_y) != Pieces.Null)
                eaten = true;
            else
                eaten = false;
            Set(dest_x, dest_y, this_piece);
            Set(src_x, src_y, Pieces.Null);
            return true;
        }
        else
        {
            eaten = false;
            return false;
        }
    }

    private bool ValidKingMove(int src_x, int src_y, int dest_x, int dest_y)
    {
        Pieces this_piece = Get(src_x, src_y);

        if (src_x == dest_x && src_y == dest_y)
            return false;

      
        if (!(Mathf.Abs(src_x - dest_x) <= 1 && Mathf.Abs(src_y - dest_y) <= 1))
            return false;

        return CheckEndPosition(src_x, src_y, dest_x, dest_y, this_piece);
    }

    private bool KingMove(int src_x, int src_y, int dest_x, int dest_y, out bool eaten)
    {
        Pieces this_piece = Get(src_x, src_y);
        if (this_piece != Pieces.BlackKing && this_piece != Pieces.WhiteKing)
        {
            eaten = false;
            return false;
        }

        
        if (ValidKingMove(src_x, src_y, dest_x, dest_y))
        {
            if (Get(dest_x, dest_y) != Pieces.Null)
                eaten = true;
            else
                eaten = false;
            Set(dest_x, dest_y, this_piece);
            Set(src_x, src_y, Pieces.Null);
            return true;
        }
        else
        {
            eaten = false;
            return false;
        }
    }
}