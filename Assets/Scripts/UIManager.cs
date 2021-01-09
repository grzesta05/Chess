﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool consoleOpen = false;
    [SerializeField]
    private Board board;
    private bool disableForcedColorMoves = false;
    private bool disableBoardFlip = false; 
    //public float timeToBoardPush = 5f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(!consoleOpen)
            {
                ReadSettings();
            }
            consoleOpen = !consoleOpen;
        }

        //if(timeToBoardPush > 0f)
        //{
        //    timeToBoardPush -= Time.deltaTime;
        //} 
        //else
        //{
        //    PushSettingsToBoard();
        //    timeToBoardPush = 5f;
        //}
    }

    private void OnGUI()
    {
        //ReadSettings();

        if(consoleOpen)
        {
            GUI.Box(new Rect(10, 10, 200, 90), "Debug Menu");
            #region Color
            GUIStyle green = new GUIStyle();
            GUIStyle red = new GUIStyle();
            green.normal.background = MakeTex(/*2, 2,*/ Color.green);
            red.normal.background = MakeTex(/*2, 2,*/ Color.red);
            #endregion
            //disableForcedColorMoves = GUI.Toggle(new Rect(12, 30, 200, 20), disableForcedColorMoves, "Disable forced color moves");
            //disableBoardFlip = GUI.Toggle(new Rect(12, 60, 200, 20), disableBoardFlip, "Disable board flip");
            if (GUI.Button(new Rect(12, 30, 160, 20), "Forced color moves"))
            {
                disableForcedColorMoves = !disableForcedColorMoves;
                PushSettingsToBoard();
            }
            if (!board.disableForcedColorMoves)
            {
                GUI.Box(new Rect(185, 30, 20, 20), " ", green);
            }
            else
            {
                GUI.Box(new Rect(185, 30, 20, 20), " ", red);
            }

            if (GUI.Button(new Rect(12, 55, 160, 20), "Board flip"))
            {
                disableBoardFlip = !disableBoardFlip;
                PushSettingsToBoard();
            }
            if (!board.disableTurnBoard)
            {
                GUI.Box(new Rect(185, 55, 20, 20), " ", green);
            }
            else
            {
                GUI.Box(new Rect(185, 55, 20, 20), " ", red);
            }
        }
    }

    private void PushSettingsToBoard()
    {
        board.disableForcedColorMoves = this.disableForcedColorMoves;
        board.disableTurnBoard = this.disableBoardFlip;
        PlayerPrefs.SetInt("disableBoardFlip", System.Convert.ToInt32(disableBoardFlip));
        PlayerPrefs.SetInt("disableForcedColorMoves", System.Convert.ToInt32(disableForcedColorMoves));
    }

    private void ReadSettings()
    {
        this.disableBoardFlip = PlayerPrefs.GetInt("disableBoardFlip", 0) == 1 ? true : false;
        this.disableForcedColorMoves = PlayerPrefs.GetInt("disableForcedColorMoves", 0) == 1 ? true : false;
    }

    private Texture2D MakeTex(Color col)
    {
        Color[] px = new Color[4];
        for(int i = 0; i < 4; i++)
        {
            px[i] = col;
        }
        Texture2D result = new Texture2D(2, 2);
        result.SetPixels(px);
        result.Apply();
        return result;
    }
}
