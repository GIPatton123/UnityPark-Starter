/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG Managers
* FILE NAME       : SceneManager.cs
* DESCRIPTION     : Scene Manager for managing scene transitions, loads and unloads.
*
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/07     Akram Taghavi-Burris        Created class
*
*
/******************************************************************/

using System.Collections;
using System.Collections.Generic;
using CSG.General;
using CSG.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

//Alias to Unity's SceneManager
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace CSG.Managers
{

    //Implement the singleton pattern
    public class SceneManager : Singleton<SceneManager> 
    {
        //Use SceneReference Manager
        public bool UseSceneAssets;
        
        // Properties to scene names
        public string MainMenu;
        public string PauseMenu;
        public string OptionsMenu;
        public string HudScene;
        public string GameOverScene;
        public List<string> GameLevels;
        
        // Private variables for managing scenes
        private string _currentScene;
        private string _gameLevelToLoad;
        private int _gameLevelIndex = -1; // Set to -1 initially, as no level is loaded yet.
        private List<string> _loadedScenes = new List<string>();
        
     

        
        /// <summary>
        /// Get a game level, either by name or by index.
        /// </summary>
        ///<param name="gameLevelName">The name of game level to get, null value allowed for loading sequentially.</param>
        public void GetGameLevel(string gameLevelName = null)
        {
            Debug.Log($"Initial level index: {_gameLevelIndex}");
            Debug.Log($"Total game levels: {GameLevels.Count}");
            
            //if the game level name is null set to he first level in the list
            if (gameLevelName == null)
            {

                // Increment the index to load the next level
                _gameLevelIndex++;
                
                Debug.Log($"Incremented level index: {_gameLevelIndex}");

                // If we're past the last level, loop back to the first level (or handle as needed)
                if (_gameLevelIndex >= GameLevels.Count)
                {
                    Debug.LogWarning("No more game levels");
                }
            }
            //If a game level name is set, check if it exists in the list
            else if (GameLevels.Contains(gameLevelName))
            {
                // Set the current level index to the passed level
                _gameLevelIndex = GameLevels.IndexOf(gameLevelName); 
            }
            else
            {
                // If the level name is not in the List, log a warning and default to the first level
                Debug.LogWarning($"Game level '{gameLevelName}' not found. Defaulting to the first level.");
                
                _gameLevelIndex = 0; // Default to the first level in the list
            }//end if/else (gameLevelName)
            
            
            //set game level to load to current game level index
            _gameLevelToLoad = GameLevels[_gameLevelIndex];

            // Log for debugging
            Debug.Log("Level to load: " + _gameLevelToLoad);

            //Request scene change to game level
            OnSceneChangeRequest(_gameLevelToLoad);

        }//end GetGameLevel()

        
        ///<summary>
        /// Method to request a scene change asynchronously.
        ///</summary>
        ///<param name="sceneName">The name of the scene to load.</param>
        ///<param name="isAdditive">If true, the scene will be loaded in additive mode.</param>
        public void OnSceneChangeRequest(string sceneName, bool isAdditive = false)
        {
            //Check if scene is not already loaded
            if (_loadedScenes.Contains(sceneName))
            {
                Debug.LogWarning($"{sceneName} is already loaded");
                return;
            }//end if(_loadedScenes.Contains(sceneName)

            //Set load mode based on isAdditive value
            LoadSceneMode loadMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

            // Start async scene loading
            StartCoroutine(LoadSceneAsync(sceneName, loadMode));
            
            Debug.Log($"{sceneName} is loaded");

        } //end OnSceneChangeRequest()

        
        ///<summary>
        /// Coroutine to load the scene asynchronously.
        ///</summary>
        ///<param name="sceneName">The name of the scene to load.</param>
        ///<param name="loadMode">The mode in which to load the scene, either Single or Additive.</param>
        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode)
        {
            //Reference to the scene being loaded
            AsyncOperation asyncLoad = UnitySceneManager.LoadSceneAsync(sceneName, loadMode);

            //Check to ensure there is no null reference
            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene '{sceneName}'. Make sure the scene is added in Build Settings.");
                
                yield break;
            }//end if(asyncLoad == null)

            // If loading in Single mode set scene as current scene
            if (loadMode == LoadSceneMode.Single)
            {
                _currentScene = sceneName;
            }//end if(loadMode == LoadSceneMode.Single)

            // Wait until scene is fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }//end while(!asyncLoad.isDone)

            // Add scene to HashSet after it successfully loads
            _loadedScenes.Add(sceneName);
            Debug.Log($"Scene '{sceneName}' successfully loaded.");
            
        }//end LoadSceneAsync()
        
        ///<summary>
        /// Unload a scene and remove it from the loaded scenes list and unloads asynchronously.
        ///</summary>
        ///<param name="sceneName">The name of the scene to unload.</param>

        public void UnloadScene(string sceneName)
        {
            // Check if the scene is currently loaded
            if (_loadedScenes.Contains(sceneName))
            {
                // Unload the scene asynchronously
                StartCoroutine(UnloadSceneAsync(sceneName));
            }
            else
            {
                Debug.LogWarning($"Scene '{sceneName}' is not currently loaded.");
                
            }//end if(_loadedScenes.Contains(sceneName))
            
        }//end UnloadScene()
        

        ///<summary>
        /// Coroutine to unload the scene asynchronously.
        ///</summary>
        ///<param name="sceneName">The name of the scene to unload.</param>
        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            //Reference to the scene being unloaded
            AsyncOperation asyncUnload = UnitySceneManager.UnloadSceneAsync(sceneName);

            //Check to ensure there is no null reference
            if (asyncUnload == null)
            {
                Debug.LogError($"Failed to unload scene '{sceneName}'.");
                yield break;
            }//end if(asyncUnload == null)

            // Wait until the scene is fully unloaded
            while (!asyncUnload.isDone)
            {
                yield return null;
            }//end while(!asyncUnload.isDone)

            // Remove from the loaded scenes list after successfully unloading
            _loadedScenes.Remove(sceneName);
            Debug.Log($"Scene '{sceneName}' successfully unloaded.");
            
        }//end UnloadSceneAsync()
        
        //Unload all scenes and reset game levels 
        public void UnloadAllScenes()
        {
            // Clear the list of scenes
            _loadedScenes.Clear();
            
            //Reset the GameLevel Index
            _gameLevelIndex = -1;

            Debug.Log("All scenes unloaded.");

        }//end UnloadAllScenes()

    } //end SceneManager.cs
}//end namespace