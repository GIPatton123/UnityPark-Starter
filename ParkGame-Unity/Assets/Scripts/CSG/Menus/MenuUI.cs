/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : MenuUI
* FILE NAME       : MainMenuUI.cs
* DESCRIPTION     : Registers UI elements for main menu to UI Manager
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/20     Akram Taghavi-Burris        Created class
*
*
/******************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CSG.UI;

namespace CSG.Menus{
    public class MenuUI : UIElementHandler
    {
    
        // Dictionary reference of general game (text) values
        private Dictionary<string, string> gameInfoUIElements = new();
        
            
        //Register UI with UIManager when enabled
        protected override void OnEnable()
            {
                base.OnEnable();
                
                // Populate the dictionary with the corresponding game info values
                gameInfoUIElements = new Dictionary<string, string>
                {
                    { "Title", UIM.GameTitle },
                    { "Description", UIM.GameDescription },
                    { "Credits", UIM.GameCredits }
                };
                
              Debug.Log(UIM.GameTitle);  
              SetTextFromDictionary(_labelElementsList, gameInfoUIElements);

            }//end OnEnable()

           
    }//end MenuUI.cs
} //end namespace