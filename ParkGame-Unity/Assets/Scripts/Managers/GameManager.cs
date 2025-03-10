/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG Managers
* FILE NAME       : GameManager.cs
* DESCRIPTION     : Game Manager for managing game essentials.
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/04     Akram Taghavi-Burris        Created class
* 2025/02/10    ""                           Added Restart case
*
/******************************************************************/

using System;
using CSG.General;
using CSG.UI;
using UnityEngine;

namespace CSG.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        
        [Header("General Game Settings")]
        public string GameTitle = "Untitled Game";
        public string GameDescription = "Short Game Description";
        public string GameCredits = "By Your Name";

        
        [Header("Game State")]
        //Current state of game 
        public GameState CurrentState = GameState.Idle;


        [Header("Managers")]
        public SceneManager SM;
        public UIManager UIM;


        // Start is called before the first frame update
        void Start()
        {
            // Get the instance to managers
            SM = SceneManager.Instance;
            UIM = UIManager.Instance;

            ChangeState(GameState.Idle);

        } //end Start()
        

        //QuitGame will exit the game or exit play mode in the editor
        void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        } //end QuitGame()
        

        //Manage the behaviors for game state changes 
        void ManageGameState()
        {
            switch (CurrentState)
            {
                case GameState.Idle:
                    Debug.Log("Idle");
                    ChangeState(GameState.MainMenu);
                    break;

                case GameState.MainMenu:
                    Debug.Log("MainMenu");
                    SM.OnSceneChangeRequest(SM.MainMenu);
                    
                    break;

                case GameState.Playing:
                    Debug.Log("Playing Game");
                    break;

                case GameState.Paused:
                    Debug.Log("Paused");
                    break;
                
                case GameState.Restart:
                    Debug.Log("Restart Game");
                    SM.UnloadAllScenes();
                    StartLevel();
                    break;

                case GameState.GameOver:
                    Debug.Log("Game Over");
                    SM.OnSceneChangeRequest(SM.GameOverScene);
                    break;

                case GameState.QuitGame:
                    Debug.Log("Quit Game");
                    QuitGame();
                    break;

                default:
                    Debug.LogWarning("Unknown GameState! Defaulting to Idle.");
                    CurrentState = GameState.Idle;
                    break;

            } //end switch (CurrentState)

        } //end ManageGameState


        //Start (load) the next game level
        public void StartLevel(string levelName = null)
        {
            SM.GetGameLevel(levelName);
            ChangeState(GameState.Playing);
        } //end StartLevel()
        
        
        //Toggles pause behaviors on and off 
        public void TogglePause()
        {
            //Do not load Pause Menu On Main Menu
            if (CurrentState == GameState.MainMenu){return;}
            
            //Check if current state is Pause
            if (CurrentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
                Time.timeScale = 1;
                SM.UnloadScene(SM.PauseMenu);
            }
            else //change to pause state
            {
                ChangeState(GameState.Paused);
                Time.timeScale = 0;
                SM.OnSceneChangeRequest(SM.PauseMenu, true);
                
            }//end if(CurrentState == GameState.Paused)
            
        }//end TogglePause()
        

        //<summary>
        //Changes the state of the game
        //</summary>
        //<param name="newState">Pass the game of the new game state as a parameter.</param>
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            ManageGameState();
        } //end ChangeState
        
        
        

    } //end GameManager.cs
}//end namespace