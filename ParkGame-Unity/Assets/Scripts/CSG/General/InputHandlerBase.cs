/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG General
* FILE NAME       : InputHandlerBase.cs
* DESCRIPTION     : Base class for handling input dynamically
*
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/12    Akram Taghavi-Burris      Created classs
*
*
/******************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

namespace CSG.General{
    public abstract class InputHandlerBase : MonoBehaviour
    {
        public InputActionAsset ActionInputAsset; // Reference your GameInput_Action asset
        
       // public string ActionMapName;  // Name of the action map to dynamically choose
        private InputActionMap _actionsMap;
        private void OnEnable()
        {
            // Check if ActionInputAsset is assigned
            if (ActionInputAsset == null)
            {
                Debug.LogError("ActionInputAsset is not assigned.");
                return;
            }

            // Get the first action map from the InputActionAsset
            _actionsMap = ActionInputAsset.actionMaps.Count > 0 ? ActionInputAsset.actionMaps[0] : null;

            if (_actionsMap == null)
            {
                Debug.LogError("No action maps found in the ActionInputAsset.");
                return;
            }

            // Loop through all actions in the selected map and enable them
            foreach (var action in _actionsMap.actions)
            {
                action.Enable();
                // Register the dynamic callback for each action
                action.performed += ctx => OnActionPerformed(ctx);
            }
        }

        private void OnDisable()
        {
            // Loop through all actions and unregister the callback
            foreach (var action in _actionsMap.actions)
            {
                action.performed -= ctx => OnActionPerformed(ctx);
                action.Disable();
            }
        }

       // Generic method to handle action performance based on action name
        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            string actionName = context.action.name;
            
            // Dynamically call method based on action name
            var method = this.GetType().GetMethod($"On{actionName}", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method != null)
            {
                // Check if the method has parameters
                var parameters = method.GetParameters();

                if (parameters.Length == 0)
                {
                    // Method doesn't require parameters, invoke without arguments
                    method.Invoke(this, null);
                }
                else
                {
                    // If method requires parameters, pass the callback context
                    method.Invoke(this, new object[] { context });
                }
            }
            else
            {
                Debug.LogWarning($"No method found for action: {actionName}");
            }
        }
    }//end InputHandlerBase.cs
}//end namespace