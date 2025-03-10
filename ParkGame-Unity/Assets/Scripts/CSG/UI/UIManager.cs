/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG Managers
* FILE NAME       : UIManager.cs
* DESCRIPTION     : UI Manager for managing user interface updates
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/20     Akram Taghavi-Burris        Created class
* 2025/02/22     ""                          Forced UI update after value change
*
/******************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CSG.General;
using CSG.Managers;

namespace CSG.UI
{
    public class UIManager : Singleton<UIManager>
    {
        
        //General Game Info
        public String GameTitle { get; set; }
        public String GameDescription { get; set; }
        public String GameCredits { get; set; }
        

        //GameManager Reference
        private GameManager GM;
        
        // UI Element Dictionaries by type
        private Dictionary<string, List<VisualElement>> _buttonDictionary = new();
        private Dictionary<string, List<VisualElement>> _textLabelsDictionary = new();
        
        //Dictionary of values 
        private Dictionary<string, string> _textValuesDictionary = new();

        // Awake is called once at instantiation
        protected override void Awake()
        {
            //call the base awake
            base.Awake();
            
            //Reference the GameManager
            GM = GameManager.Instance;

        }//end Awake()

        void Start()
        {
            GetGameInfo();
        }//end Start()
        
        
        
        // Get the general game info from the GameManager
        private void GetGameInfo()
        {
            GM = GameManager.Instance;
            GameTitle = GM.GameTitle;
            GameDescription = GM.GameDescription;
            GameCredits = GM.GameCredits;
       
            Debug.Log(GameTitle + " - " + GameDescription + " - " + GameCredits);
        }//end GetGameInfo()
        
        
        /// <summary>
        /// Registers UI Element to be updated when its ID's value changes.
        /// </summary>
        /// <param name="elementName">UI element ID is the name of the element in the UI Builder</param>
        /// <param name="elementType">UI element type </param>
        /// <param name="elementText"> Default text value of UI element </param>
        /// <typeparam name="T">T is the type of UI element we are registering (e.g. button, label) </typeparam>
        public void RegisterUIElement<T>(string elementName, T elementType, string elementText) where T : VisualElement
        {
            // Determine which dictionary to use based on type
            var uiElementDictionary = GetDictionaryType(typeof(T));

            // Ensure the dictionary is valid
            if (uiElementDictionary == null)
            {
                Debug.LogError($"UIManager: Unsupported element type {typeof(T)} for registration.");
                return;
            }//end dictionary validation

            
            // Create a new list of element type
            if (!uiElementDictionary.ContainsKey(elementName))
            {
                uiElementDictionary[elementName] = new List<VisualElement>();
                
            }//end new list creation

            // Add the element name and associated type,  if it is not already registered
            if (!uiElementDictionary[elementName].Contains(elementType))
            {
                uiElementDictionary[elementName].Add(elementType);
            }
            else
            {
                Debug.LogWarning($"UIManager: UI element '{elementName}' is already registered.");
                
            }//end add element to dicitionary
            
            
            // If there is no text value
            if (!_textValuesDictionary.ContainsKey(elementName))
            {
                // set the default text
                _textValuesDictionary[elementName] = elementText;
                
            }//end if no text value
  
            // Update the UI text value to default text 
            UpdateUI<T>(elementName, _textValuesDictionary[elementName]);
            
        }//end RegisterUIElement()

        
        // Determines the appropriate dictionary based on the element type
        private Dictionary<string, List<VisualElement>> GetDictionaryType(Type elementType)
        {
            // Check if the type is Button
            if (elementType == typeof(Button))
                return _buttonDictionary; // Dictionary for buttons

            // Check if the type is Label
            if (elementType == typeof(Label))
                return _textLabelsDictionary; // Dictionary for labels

            // If type is unsupported
            Debug.LogWarning($"UIManager: No dictionary found for element type {elementType}");
            return null;
            
        }//end GetDictionaryType()

        
        /// <summary>
        /// Update the UI Element based on the type of element 
        /// </summary>
        /// <param name="elementID">UI element ID is the name of the element in the UI Builder</param>
        /// <param name="newValue">The new "text" value to display. </param>
        /// <typeparam name="T">T is the type of UI element we are registering (e.g. button, label) </typeparam>
        public void UpdateUI<T>(string elementID, string newValue) where T : VisualElement
        {
           
            var uiElementDictionary = GetDictionaryType(typeof(T));

            // Ensure the dictionary exists and contains the element ID
            if (uiElementDictionary != null && uiElementDictionary.ContainsKey(elementID))
            {
               
                foreach (var element in uiElementDictionary[elementID])
                {
                    switch (element)
                    {
                        case Label labelElement:
                            labelElement.text = newValue; // Update Label text
                            Debug.LogWarning($"{elementID} is now equal to {newValue}.");
                            break;

                        case Button buttonElement:
                            buttonElement.text = newValue; // Update Button text
                            break;

                        // Extend for other UI types (e.g., TextField, Slider, etc.)
                        case TextField textFieldElement:
                            textFieldElement.value = newValue; // Update TextField value
                            break;

                        default:
                            Debug.LogWarning($"UIManager: Unsupported UI element type {element.GetType()} for ID '{elementID}'");
                            break;
                        
                    }//end switch(element)
                }//end foreach 
            }
            else
            {
                Debug.LogWarning($"UIManager: No elements of type {typeof(T)} found for ID '{elementID}'");
                
            }//end If dictionary exists 
            
        }//end UpdateUI 

        
        //Retrieves the stored value for a given key.
        public string GetTextValue(string key)
        {
            return _textValuesDictionary.ContainsKey(key) ? _textValuesDictionary[key] : string.Empty;
            
        }//end GetTextValue()
        
        
        //Unregister UI elements when removed or scenes changes
        public void UnregisterUIElement<T>(string elementName, T elementType) where T : VisualElement
        {
            // Determine which dictionary to use based on type
            var uiElementDictionary = GetDictionaryType(typeof(T));

            // Ensure the dictionary is valid
            if (uiElementDictionary == null)
            {
                Debug.LogError($"UIManager: Unsupported element type {typeof(T)} for unregistration.");
                return;
            }//end dictionary validation

            // Check if the element exists in the dictionary
            if (uiElementDictionary.ContainsKey(elementName))
            {
                // Remove the element from the list
                if (uiElementDictionary[elementName].Contains(elementType))
                {
                    uiElementDictionary[elementName].Remove(elementType);
                    Debug.Log($"UIManager: UI element '{elementName}' unregistered successfully.");
                }
                else
                {
                    Debug.LogWarning($"UIManager: Element '{elementName}' of type {typeof(T)} not found in dictionary.");
                }//end remove from list
            }
            else
            {
                Debug.LogWarning($"UIManager: Element name '{elementName}' not found in dictionary.");
                
            }//end check for element in dictionary

            // Remove associated text value from _textValuesDictionary if it exists
            if (_textValuesDictionary.ContainsKey(elementName))
            {
                _textValuesDictionary.Remove(elementName);
                Debug.Log($"UIManager: Text value for '{elementName}' unregistered successfully.");
                
            }//end if(_textValuesDictionary.ContainsKey(elementName))
            
        }//end UnregisterUIElemen()
        
    } //end UIManager.cs
}//end namespace