﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }
    //0 - black, 1 - white
    public bool white;

    public GameObject board;

    public PieceType type;

    bool selected = false;
    public Square currentSquare;
    List<Square> legalSquares = new List<Square>();
    List<Square> temp = new List<Square>();

    private void Start()
    {
        foreach(Transform sq in board.GetComponent<Board>().children)
        {
            //If a piece is close to a square - if it's on the square
            if((sq.position - this.transform.position).sqrMagnitude < 1f)
            {
                currentSquare = sq.gameObject.GetComponent<Square>();
                currentSquare.currentPiece = this;
                continue;
            }
        }
    }

    private void Update()
    {
        currentSquare.occupied = true;
    }

    private void OnMouseDown()
    {
        selected = true;

        //Actual calculating legal moves
        switch (type)
        {
            case PieceType.King:
                //For all squares
                foreach (Transform sq in board.GetComponent<Board>().children)
                {
                    //This magnitude gives every square around (standard king's move)
                    if ((sq.position - this.transform.position).sqrMagnitude <= 250f)
                    {
                        if (sq.GetComponent<Square>().occupied)
                        {
                            if (sq.GetComponent<Square>().currentPiece.white != this.white)
                            {
                                legalSquares.Add(sq.gameObject.GetComponent<Square>());
                            }

                            continue;
                        }

                        legalSquares.Add(sq.gameObject.GetComponent<Square>());
                    }
                }

                break;

            case PieceType.Rook:
                foreach (Transform sq in board.GetComponent<Board>().children)
                {
                    //This magnitude gives squares top, bottom, left and right (relative to current)
                    if ((sq.position - this.transform.position).sqrMagnitude <= 150f && !sq.GetComponent<Square>().occupied)
                    {
                        legalSquares.Add(sq.gameObject.GetComponent<Square>());
                    }
                }

                foreach(Square sq in legalSquares)
                {
                    Debug.Log("psiuu");
                    //If legal move is on the edge, it's the last in this direction anyway, so sort it out here
                    if(sq.column != 0 && sq.column != 7 && sq.row != 0 && sq.row != 7)
                    {
                        //Four possibilities: this square is top, left, bottom or right relatively to here
                        if(sq.row == currentSquare.row)
                        {
                            if(sq.column > currentSquare.column)
                            {
                                //Case right
                                int i = 1;
                                //Math.Abs is translation from chess notation (squares being counted from bottom-left) to normal notation (from top-left)
                                //Basically while is iterating through legal moves (while condition indicates legal)
                                while (sq.column + i < 8)
                                {
                                    if(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i].occupied)
                                    {
                                        if (board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i].currentPiece.white != this.white)
                                        {
                                            temp.Add(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i]);
                                        }

                                        break;
                                    }
                                    temp.Add(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i]);
                                    i++;
                                }
                            }
                            else
                            {
                                //Case left
                                int i = -1;
                                while (sq.column + i >= 0 && !board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i].occupied)
                                {
                                    temp.Add(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7), sq.column + i]);
                                    i--;
                                }
                            }
                        }
                        else
                        {
                            if (sq.row > currentSquare.row)
                            {
                                //Case up
                                int i = 1;
                                while (sq.row - i < 8 && !board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7) - i, sq.column].occupied)
                                {
                                    temp.Add(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7) - i, sq.column]);
                                    i++;
                                }
                            }
                            else
                            {
                                //Case bottom
                                int i = -1;
                                while (sq.row - i >= 0 && !board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7) - i, sq.column].occupied)
                                {
                                    temp.Add(board.GetComponent<Board>().squares[Mathf.Abs(sq.row - 7) - i, sq.column]);
                                    i--;
                                }
                            }
                        }
                    }
                }

                break;

            case PieceType.Pawn:
                int j = 0;
                foreach (Transform sq in board.GetComponent<Board>().children)
                {
                    if (this.white)
                    {
                        //If first move (you can move 2 squares)
                        //If pawn is in second row (can double move), second square !occupied, it's actually the square 2 ahead,                                it's in the same column                                         the square 1 ahead is not occupied
                        //Overall: can move two squares ahead
                        if(this.currentSquare.row == 1 && !sq.GetComponent<Square>().occupied && sq.GetComponent<Square>().row == this.currentSquare.row + 2 && sq.GetComponent<Square>().column == this.currentSquare.column && !board.GetComponent<Board>().children[j + 8].GetComponent<Square>().occupied)
                        {
                            legalSquares.Add(sq.GetComponent<Square>());
                        }
                        //Square's one ahead,                                               it's in the same column,                                        is not occupied
                        if(sq.GetComponent<Square>().row == this.currentSquare.row + 1 && sq.GetComponent<Square>().column == this.currentSquare.column && !sq.GetComponent<Square>().occupied)
                        {
                            legalSquares.Add(sq.GetComponent<Square>());
                        }
                    }
                    else
                    {
                        //Look above basically
                        if(this.currentSquare.row == 6 && !sq.GetComponent<Square>().occupied && sq.GetComponent<Square>().row == this.currentSquare.row - 2 && sq.GetComponent<Square>().column == this.currentSquare.column && !board.GetComponent<Board>().children[j - 8].GetComponent<Square>().occupied)
                        {
                            legalSquares.Add(sq.GetComponent<Square>());
                        }
                        if(sq.GetComponent<Square>().row == this.currentSquare.row - 1 && sq.GetComponent<Square>().column == this.currentSquare.column && !sq.GetComponent<Square>().occupied)
                        {
                            legalSquares.Add(sq.GetComponent<Square>());
                        }
                    }

                    j++;
                }

                break;
        }

        foreach(Square sq in temp)
        {
            legalSquares.Add(sq);
        }

        foreach(Square sq in legalSquares)
        {
            sq.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
    }

    //TODO: gotta update currentPiece and currentSquare on move
}