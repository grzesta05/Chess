﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //2D array of squares, actually representing a board (8x8)
    public Square[,] squares = new Square[8, 8];

    public Transform[] children;

    private void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                squares[i, j] = children[((i) * 8) + j].GetComponent<Square>();
            }
        }

        //for (int i = 0; i < 8; i++)
        //{
        //    for (int j = 0; j < 8; j++)
        //    {
        //        Debug.Log(string.Format("{0} {1}", children[((i) * 8) + j].GetComponent<Square>().column, children[((i) * 8) + j].GetComponent<Square>().row));
        //    }
        //    Debug.Log("ROW");
        //}
    }
}