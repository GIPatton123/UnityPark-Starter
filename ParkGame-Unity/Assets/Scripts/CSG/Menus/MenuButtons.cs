/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG.Menus
* FILE NAME       : MainMenuButtons.cs
* DESCRIPTION     : Behaviours or main menu buttons
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/09    Akram Taghavi-Burris      Created classs
* 
*
/******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSG.General;
using CSG.Managers;

namespace CSG.Menus {
    public class MenuButtons : ButtonsBase
    {
    
        /// <summary>
        /// Defines behavior when a button is clicked.
        /// </summary>
        /// <param name="buttonName">The name of the clicked button</param>
        protected override void OnClickButton(string buttonName)
        {
            switch (buttonName)
            {
                case "PlayButton":
                    Debug.Log("Starting game...");
                    GameManager.Instance.StartLevel();
                    break;
                
                case "ResumeButton":
                    Debug.Log("resume game...");
                    GameManager.Instance.TogglePause();
                    break;
                
                case "RestartButton":
                    Debug.Log("restart game...");
                    GameManager.Instance.ChangeState(GameState.Restart);
                    break;
                    
                case "QuitButton":
                    Debug.Log("Exiting game...");
                    GameManager.Instance.ChangeState(GameState.QuitGame);
                    break;
                
                default:
                    Debug.Log($"Unhandled button: {buttonName}");
                    break;
            }
        }//end OnClickButton()

    }//end MainMenuButtons.cs
}//end namespace