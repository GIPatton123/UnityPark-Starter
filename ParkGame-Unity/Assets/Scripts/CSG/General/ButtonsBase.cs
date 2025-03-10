/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG.General 
* FILE NAME       : ButtonBase.cs
* DESCRIPTION     : Base class for managing menu buttons in a UI Document.
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
using UnityEngine.UIElements;

namespace CSG.General
{
    public abstract class ButtonsBase : MonoBehaviour
    {
        // Reference to the UI Document attached to this GameObject
        protected UIDocument _uiDoc;
        
        // Dictionary to store button references, mapped by their names
        protected Dictionary<string, Button> _buttonsDictionary = new();

        /// <summary>
        /// Awake is called once at instantiation
        /// Protected virtual allows for the child class to overide this
        /// </summary>
        protected void Awake()
        {
            // Get the UIDocument component attached to this GameObject
            _uiDoc = GetComponent<UIDocument>();

            // Get all buttons from the UI document
            GetButtons();
        }//end Awake()

        /// <summary>
        /// Searches for buttons in the UI document using the provided names,
        /// stores them in the dictionary, and registers click event listeners.
        /// </summary>
        private void GetButtons()
        {
            // Get all buttons from the root visual element
            List<Button> buttons = _uiDoc.rootVisualElement.Query<Button>().ToList();

            if (buttons.Count == 0)
            {
                Debug.LogWarning("No buttons found in the UI document.");
            }//end if(buttons.Count == 0)

            // Loop through all buttons and register them
            foreach (var button in buttons)
            {
                // Use the button in the loop, no need to query again by name
                string name = button.name; // Assuming each button has a unique name
                
                // Store button in the dictionary
                _buttonsDictionary[name] = button;

                // Register click event listener
                button.RegisterCallback<ClickEvent>(evt => OnClickButton(name));
                
            }//end foreach(var button in buttons)
        }//end GetButtons()

        /// <summary>
        /// Abstract method for handling button clicks.
        /// Each derived class must define behavior for each button.
        /// </summary>
        /// <param name="buttonName">The name of the button that was clicked</param>
        protected abstract void OnClickButton(string buttonName);
        
    }//end ButtonsBase.cs
}//end namespce