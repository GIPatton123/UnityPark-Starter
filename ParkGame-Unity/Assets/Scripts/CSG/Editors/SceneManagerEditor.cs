/*******************************************************************
* COPYRIGHT       : Year
* PROJECT         : CSG.Editors
* FILE NAME       : SceneManagerEditor.cs
* DESCRIPTION     : Custom editor for Scene Manager, sets scene reference mode
* (i.e. string name or scene asset reference)
*
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/10     Akram Taghavi-Burris        Created class
*
*
/******************************************************************/


using UnityEngine;
using UnityEditor;
using CSG.Managers;
namespace CSG.Editors
{
    [CustomEditor(typeof(SceneManager))]  // Ensure SceneManager is your custom class, not Unity's built-in one
    public class SceneManagerEditor : Editor
    {
        //Scene Manager section modes
        private enum SceneReferenceMode { None, StringReference, AssetReference}
        private SceneReferenceMode _selectedMode;
        
        //Reference to SceneManager
        private SceneManager _sceneManager;
        
        //Reference to SceneAssetReference
        private SceneAssetReference _sceneAssetReference;
        

        private SerializedProperty _mainMenu;
        private SerializedProperty _pauseMenu;
        private SerializedProperty _optionsMenu;
        private SerializedProperty _hudScene;
        private SerializedProperty _gameOverScene;
        private SerializedProperty _gameLevels;
        private SerializedProperty _isPersistentProperty;
       

        private void OnEnable()
        {
            // Load saved mode selection (default to Basic)
            _selectedMode = (SceneReferenceMode)EditorPrefs.GetInt("SceneReferenceMode", 0);
            
            // Get serialized properties
            _mainMenu = serializedObject.FindProperty("MainMenu");
            _pauseMenu = serializedObject.FindProperty("PauseMenu");
            _optionsMenu = serializedObject.FindProperty("OptionsMenu");
            _hudScene = serializedObject.FindProperty("HudScene");
            _gameOverScene = serializedObject.FindProperty("GameOverScene");
            _gameLevels = serializedObject.FindProperty("GameLevels");  
            
        }//end OnEnable()
        
        

        public override void OnInspectorGUI()
        {
            // Sync the properties with the inspector
            serializedObject.Update(); 
            
            // Display inherited field _isPersistent)
            EditorGUILayout.LabelField("SINGLETON STATUS", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_isPersistent"));
            
            //Get the SceneManager target
            _sceneManager = (SceneManager)target;

            //Try to get the SceneAssetReference; component and output to _sceneReference value
            _sceneManager.TryGetComponent(out _sceneAssetReference);
            
            // Display description of the modes
            EditorGUILayout.HelpBox("Add all scenes to the Build Profile Scenes Lists before testing.  ", 
                MessageType.Info);
            
            // Dropdown for selecting mode
            EditorGUILayout.LabelField("SCENE REFERENCE MODE", EditorStyles.boldLabel);
            // Display description of the modes
            EditorGUILayout.HelpBox("- String Reference: Reference scenes by name (requires exact spelling).\n" +
                                    "- Asset Reference: Use SceneAssetReference component reference scenes by scene assest.", 
                MessageType.Info);

            SceneReferenceMode newMode = (SceneReferenceMode)EditorGUILayout.EnumPopup("Select Mode: ", _selectedMode);

            // Update mode if changed
            if (newMode != _selectedMode)
            {
                _selectedMode = newMode;
                EditorPrefs.SetInt("SceneReferenceMode", (int)_selectedMode);
            }

            CheckMode();
            
            //Save the changes
            serializedObject.ApplyModifiedProperties();
            
        } //end OnInspectorGUI()


        private void CheckMode()
        {
            switch (_selectedMode)
            {
                case SceneReferenceMode.None: 
                    RemoveSceneAssetReference();
                    break;
                case SceneReferenceMode.StringReference:
                    ShowStringReference();
                    break;
                case SceneReferenceMode.AssetReference:
                    ShowAssetReference();
                    break;
                default:
                    _selectedMode = SceneReferenceMode.None;
                    break;
                
            }//end switch(_selectedMode)
        }//end CheckMode()
        
        
        private void ShowStringReference()
        {

        // String Reference
            EditorGUILayout.LabelField("ScenceStringReference", EditorStyles.boldLabel);
            
            _sceneManager.MainMenu = EditorGUILayout.TextField("Main Menu", _sceneManager.MainMenu);
            _sceneManager.PauseMenu = EditorGUILayout.TextField("Pause Menu", _sceneManager.PauseMenu);
            _sceneManager.OptionsMenu = EditorGUILayout.TextField("Options Menu", _sceneManager.OptionsMenu);
            _sceneManager.HudScene = EditorGUILayout.TextField("Hud Scene", _sceneManager.HudScene);
            _sceneManager.GameOverScene = EditorGUILayout.TextField("Game Over Scene", _sceneManager.GameOverScene);

            // Properly display serialized list
            EditorGUILayout.PropertyField(_gameLevels, new GUIContent("Game Levels"), true);

            RemoveSceneAssetReference();
            
            
        }//end ShowStringReference()

        
        private void ShowAssetReference()
        {
            
            //Add SceneAssetReference if null 
            if (_sceneAssetReference == null)
            {
               _sceneAssetReference = _sceneManager.gameObject.AddComponent<SceneAssetReference>();
                EditorUtility.SetDirty(_sceneManager);
            }
            
            EditorGUILayout.HelpBox("Asset-Based Mode requires the SceneReference component.", MessageType.Info);
            
        }//end ShowAssetReference()

        private void RemoveSceneAssetReference()
        {
            // Remove SceneAssetReference if it exists
            if (_sceneAssetReference != null)
            {
                DestroyImmediate(_sceneAssetReference); // Use DestroyImmediate for editor-time removal
                EditorUtility.SetDirty(_sceneManager); // Mark the SceneManager as dirty to apply changes
            }
        }//end RemoveSceneAssetReference()


    }//end SceneManagerEditor.cs
}//end namespace
