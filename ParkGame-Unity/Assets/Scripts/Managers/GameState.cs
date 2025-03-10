/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG Managers
* FILE NAME       : GameState.cs
* DESCRIPTION     : The game state options
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/04     Akram Taghavi-Burris        Created class
* 2025/02/10    ""                           Added Restart state
*
/******************************************************************/

public enum GameState
{
    Idle,
    MainMenu,
    Playing,
    Paused,
    Restart,
    GameOver,
    QuitGame
}