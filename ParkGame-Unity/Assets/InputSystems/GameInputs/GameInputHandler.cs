/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : One Wild Night
* FILE NAME       : GameInputHandler.cs
* DESCRIPTION     : Handles global game-related input actions, such as pausing, exiting the game.
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/12    Akram Taghavi-Burris      Created classs
* 
*
/******************************************************************/

using System.Collections;
using System.Collections.Generic;
using CSG.General;
using CSG.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputHandler : InputHandlerBase
{

    //Called on pause action, that toggles the pause menu
    void OnPause()
    {
        Debug.Log("OnPause");
        GameManager.Instance.TogglePause();

    }//end OnPause()

    /// <summary>
    /// OnQuit (escape) will call the OnPause, to ensure that escape was not accidentally called
    /// will load pause menu which has the option to quit or resume
    /// </summary>
    void OnQuit()
    {
        Debug.Log("OnQuit");
        OnPause();
        
    }//end OnQuit()

}//end GameInputHandler.cs