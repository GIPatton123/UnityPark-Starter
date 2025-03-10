/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG UI
* FILE NAME       : UIElementHandler.cs
* DESCRIPTION     : Shared logic for setup, registering, and unregistering UI elements.
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/17     Akram Taghavi-Burris        Created class
*
*
/******************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace  CSG.UI
{
    public abstract class UIElementHandler : MonoBehaviour
    {
        // Reference to UI manager
        protected UIManager UIM;

        // Reference to UI document
        private UIDocument _uiDocument;
        
        // Protected reference to UI document    
        private UIDocument UIDocument => _uiDocument; 
        
        //Get a reference to the Root Visual Element of the UI Document 
        protected VisualElement _root;
        
        [System.Serializable]
        //Create UI Elements data type that holds to string values for name and instance
        public class UIElements
        {
            public string _uiElementName; // UI Element name (ID) 
            public string _uiElementTextValue; // The text value of the UI Element 
            [HideInInspector] public string _uiElementType; // Type of UI element
        }

        // Create Button Element list, each index in the list will have both a name and text value
        [Tooltip("List of all Button UI elements")]
        [SerializeField] protected List<UIElements> _buttonElementsList = new List<UIElements>();
        
        // Create Label Element list, each index in the list will have both a name and text value
        [Tooltip("List of all Label UI elements")]
        [SerializeField] protected List<UIElements> _labelElementsList = new List<UIElements>();
        
        
        // On Awake reference the UI Document
        protected virtual void Awake()
        {            
            // Assign UI Manager
            UIM = UIManager.Instance;
            Debug.Log($"UI Manager {UIM}");
            
            //Get the UI Document component
            if (!TryGetComponent(out _uiDocument))
            {
                Debug.LogError("No Main Menu UI component found!");
                
            }//end if(UIDocument)
            
            // Set the root visual element
            _root = UIDocument.rootVisualElement;

            

        }//end OnAwake()

        // Initialize all UI Elements when enabled
        protected virtual void OnEnable()
        {
            InitializeUIElements<Button>(_buttonElementsList);
            InitializeUIElements<Label>(_labelElementsList);
            
        }// end OnEnable()

        //Disable UI Elements when Disabled
        protected virtual void OnDisable()
        {
            DisableUIElements<Button>(_buttonElementsList);
            DisableUIElements<Label>(_labelElementsList);
            
        }//end OnDisable()

        
        /// <summary>
        /// Initialize UI elements of any type (T) from UI Document
        /// </summary>
        /// <param name="uiElementList"> The List (type) of UI elements to add </param>
        /// <typeparam name="T">The type of elements (e.g. Button, labels, etc.) </typeparam>
        protected virtual void InitializeUIElements<T>(List<UIElements> uiElementList) 
            where T : VisualElement
        {
            // Loop through each ui element in the list
            foreach (var uiElement in uiElementList)
            {
                // Find the type of element by name using root.Q<T>(uiElement.name)
                var elementOfType = _root.Q<T>(uiElement._uiElementName);
                
                // Ensure the element exists before attempting to register it
                if (elementOfType != null)
                {
                    Debug.Log($"{uiElement._uiElementName} was added to the element List");

                    // Register the element with the UI Manager
                    UIM.RegisterUIElement<T>(uiElement._uiElementName, elementOfType, uiElement._uiElementTextValue);
                    
                    // Update the UI element text if needed 
                    UIM.UpdateUI<T>(uiElement._uiElementName, uiElement._uiElementTextValue);
                }
                else
                {
                    Debug.LogWarning($"UI Element '{uiElement._uiElementName}' not found in the document.");
                    
                }//end if(elementOfType != null)
                
            }//end ForEach element
            
        }//end InitializeUIElements()
        
        
        /// <summary>
        /// Disable (unregister) UI elements of any type (T) from UI Document
        /// </summary>
        /// <param name="uiElementList"> The List (type) UI elements to remove</param>
        /// <typeparam name="T">The type of elements (e.g. Button, labels, etc.) </typeparam>
        protected virtual void DisableUIElements<T>(List<UIElements> uiElementList)
            where T : VisualElement
        {
            // Loop through each UI element in the list
            foreach (var uiElement in uiElementList)
            {
                // Find the element by name using root.Q<T>(uiElement.name)
                var elementOfType = _root.Q<T>(uiElement._uiElementName);

                // Ensure the element exists before attempting to unregister it
                if (elementOfType != null)
                {
                    Debug.Log($"{uiElement._uiElementName} is being unregistered from the UI Manager");

                    // Unregister the element with the UI Manager
                    UIM.UnregisterUIElement<T>(uiElement._uiElementName, elementOfType);
                }
                else
                {
                    Debug.LogWarning($"UI Element '{uiElement._uiElementName}' not found in the document for unregistration.");
                    
                }//end if(elementOfType != null)
                
            }//end ForEach
            
        }//end DisableUIElements()
        
        
        // Set the new text from the dictionary
        protected void SetTextFromDictionary(List<UIElements> elementsList, Dictionary<string, string> infoDictionary)
        {
                
            // Loop through each ui element and check the game info elements exit 
            foreach (var element in infoDictionary)
            {
                string elementName = element.Key;
                string elementValue = element.Value;

                // Check if the element exists in the list
                bool elementExists = elementsList.Exists(e => e._uiElementName == elementName);

                // If the element exist, update its text value 
                if (elementExists)
                {
                    // Update the UI element text with the corresponding value
                    UIM.UpdateUI<Label>(elementName, elementValue);
                    Debug.Log($"{elementName} is equal to {elementValue}");
                        
                }//end if (!elementExists)
                    
            }//end ForEach ui element in list
                
        }//end SetTextFromDictionary
        

    }//end UIElementHandler.cs
        
}//end CSG.UI