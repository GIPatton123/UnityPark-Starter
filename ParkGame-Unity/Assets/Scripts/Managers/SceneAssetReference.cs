/*******************************************************************
* COPYRIGHT       : Year
* PROJECT         : CSG.Managers
* FILE NAME       : SceneAssetReference.cs
* DESCRIPTION     : Stores references between SceneAssets and their string names
*
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/10     Akram Taghavi-Burris        Created class
*
*
/******************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq; //Filters out any null values
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSG.Managers
{

public class SceneAssetReference: MonoBehaviour
{
    //Get reference to scene manager
    private SceneManager SceneManager;

        [Header("Menu Scenes")]
        [Tooltip("Menu Scenes have an additive scene mode")]
        [SerializeField] private SceneAsset _mainMenuScene;
        [SerializeField] private SceneAsset _pauseMenuScene;
        [SerializeField] private SceneAsset _optionsMenuScene;

        // Lists and dictionaries to store menu scenes and their names
        private List<SceneAsset> _menuScenesList = new List<SceneAsset>();
        private Dictionary<SceneAsset, string> _menuSceneDictionary = new Dictionary<SceneAsset, string>();

        // Store scene names for easy access
        private string _mainMenuSceneName;
        public string MainMenuSceneName {get; private set;}
        
        private string _pauseMenuSceneName;
        public string PauseMenuSceneName {get; private set;}
        
        private string _optionsMenuSceneName;
        public string OptionsMenuSceneName {get; private set;}
        
        // Progression Scene references
        [Header("Progression Scenes")]
        [SerializeField] private SceneAsset _hudScene;
        [SerializeField] private SceneAsset _gameOverScene;
        
        // Lists and dictionaries to store progression scenes and their names
        private List<SceneAsset> _progressionScenesList = new List<SceneAsset>();
        private Dictionary<SceneAsset, string> _progressionSceneDictionary= new Dictionary<SceneAsset, string>();
        
        // Store scene names for easy access
        private string _hubSceneName;
        public string HubSceneName {get; private set;}
        
        private string _gameOverSceneName;
        public string GameOverSceneName {get; private set;}

       
        // Game Level Scenes references
        [Header("Game Levels Scenes")]
        [Tooltip("List the levels in load order, unless the order is not important")]
        [SerializeField] private List<SceneAsset> _gameScenesList = new List<SceneAsset>();

        // Dictionary to store game scenes and their names
        private Dictionary<SceneAsset, string> _gameSceneDictionary = new Dictionary<SceneAsset, string>();
        
        // List of game scene names
        private List<string> _gameLevelsSceneNames = new List<string>();
        public List<string> GameLevelsSceneNames {get; private set;}

        // Start is called before the first frame update
        void Start()
        {
            //Get reference to SceneManager
            SceneManager = GetComponent<SceneManager>();
            
            // Retrieve and organize scenes
            GetMenuScenes();
            GetProgressionScenes();
            GetLevelScenes();
            
            // Assign scene names
            SetSceneNames();
            
            // Debug to check scene names
            DebugSceneNames();

        }//end Start()


        // Retrieves all menu scenes, organizes them, and updates the scene names
        private void GetMenuScenes()
        {
            // Adds menu scenes to the list
            AddScenes(_menuScenesList, _mainMenuScene, _pauseMenuScene, _optionsMenuScene);
            
            // Updates the dictionary with scene names
            GetSceneNames(_menuScenesList, _menuSceneDictionary);
            
            // Fetch and assign names from the dictionary
            _menuSceneDictionary.TryGetValue(_mainMenuScene, out _mainMenuSceneName);
            _menuSceneDictionary.TryGetValue(_pauseMenuScene, out _pauseMenuSceneName);
            _menuSceneDictionary.TryGetValue(_optionsMenuScene, out _optionsMenuSceneName);

            // Assign the names to the public properties
            MainMenuSceneName = _mainMenuSceneName; 
            PauseMenuSceneName = _pauseMenuSceneName;
            OptionsMenuSceneName = _optionsMenuSceneName;

        }//end GetMenuScenes
        
        // Retrieves all progression scenes, organizes them, and updates the scene names
        private void GetProgressionScenes()
        {
            // Adds progression scenes to the list
            AddScenes(_progressionScenesList,_hudScene, _gameOverScene);
            
            // Updates the dictionary with scene names
            GetSceneNames(_progressionScenesList, _progressionSceneDictionary);
            
            // Fetch and assign names from the dictionary
            _progressionSceneDictionary.TryGetValue(_hudScene, out _hubSceneName);
            _progressionSceneDictionary.TryGetValue(_gameOverScene, out _gameOverSceneName);
            
            // Assign the names to the public properties
            HubSceneName = _hubSceneName;
            GameOverSceneName = _gameOverSceneName;
      
        }//end GetProgressionScenes()

        // Retrieves all game level scenes and stores their names in a list
        private void GetLevelScenes()
        {
            // Updates the dictionary with game scene names
            GetSceneNames(_gameScenesList, _gameSceneDictionary);
            
            // Loop through each key-value pair in the dictionary
            foreach (var entry in _gameSceneDictionary)
            {
                // Add the scene name (Value) to the list
                _gameLevelsSceneNames.Add(entry.Value); 
            }//end foreach
            
            GameLevelsSceneNames = _gameLevelsSceneNames;
            
        }//end GetLevelScenes()
        
        
        ///<summary>
        /// Add scenes to the scenes list if values are not null
        ///</summary>
        ///<param name = "scenes">The name of the list to add the scene references to</param>
        ///<param name = "scenesToAdd">The scene references to add to array </param>
        // The 'params' keyword allows us to pass multiple arguments directly into a method, must be set to an array
        private void AddScenes(List<SceneAsset> scenes, params SceneAsset[] scenesToAdd)
        {
            // Add the scenesToAdd to the main list if they are not null
            scenes.AddRange(scenesToAdd.Where(scene => scene != null));
        }
        
        ///<summary>
        /// Get all scenes names in the list and add them to the dictionary
        ///</summary>
        ///<param name="sceneList">A list of SceneAsset objects to extract names from and add to the dictionary.</param>
        ///<param name="sceneNames">A dictionary to store the scene references as keys and their corresponding scene names as values.</param>
        private void GetSceneNames(List<SceneAsset> sceneList, Dictionary<SceneAsset, string> sceneNames)
        {
            // Loop through each scene and set its name dynamically
            foreach (var scene in sceneList)
            {
                string sceneName = "";

                #if UNITY_EDITOR
                // In the editor, update the scene name manually
                sceneName = GetSceneName(scene);
                #else
                // At runtime, we can directly use the SceneName property
                sceneName = scene.SceneName;
                #endif

                // Add the scene reference and its corresponding name to the dictionary
                sceneNames[scene] = sceneName;

                Debug.Log("Scene Name: " + sceneName);
            }//end foreach (var scene in sceneList)
            
        }//end GetSceneNames()
        
        
        //Get the Scene name when in the editor    
        #if UNITY_EDITOR
        public string GetSceneName(SceneAsset sceneAsset)
        {
            if (sceneAsset != null)
            {
                // Get the scene path from the asset
                string scenePath = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);

                // Get the scene name from the path
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                return sceneName;
            }

            return string.Empty; // Return an empty string if the sceneAsset is null
        }//end IN UNITY EDITOR GetSceneName()
        #endif


        // Debug output for verifying scene names
        private void DebugSceneNames()
        {
            Debug.Log($"Main Menu is: {_mainMenuSceneName}");
            Debug.Log($"Pause Menu is: {_pauseMenuSceneName}");
            Debug.Log($"Pause Menu is: {_pauseMenuSceneName}");
            Debug.Log($"Hub Scene is: {_hubSceneName}");
            Debug.Log($"Game over is: {_gameOverSceneName}");

            // Log the names of game levels
            for (int i = 0; i < _gameLevelsSceneNames.Count; i++)
            {
                Debug.Log($"Game Scene {i} Name: {_gameLevelsSceneNames[i]}");
            }//end for
            
        }//end DebugSceneNames()
        
        
        // Sets scene names from the SceneReferenceManager and assigns them to public properties.
        private void SetSceneNames()
        {
                //Set scene names
                SceneManager.MainMenu = MainMenuSceneName;
                SceneManager.PauseMenu = PauseMenuSceneName;
                SceneManager.OptionsMenu = OptionsMenuSceneName;
                SceneManager.HudScene = HubSceneName;
                SceneManager.GameOverScene = GameOverSceneName;
                SceneManager.GameLevels = GameLevelsSceneNames;
                
        }//end SetSceneNames()
    
    }//end SceneAssetReference.cs
}//end namespace
