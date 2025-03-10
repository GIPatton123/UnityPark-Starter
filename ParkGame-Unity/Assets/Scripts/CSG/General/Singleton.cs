/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : CSG general libraries
* FILE NAME       : Singleton.cs
* DESCRIPTION     : Signleton base class that any MonoBehaviour can inherit from
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2025/02/04     Akram Taghavi-Burris        Created class
* 
*
/******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSG.General {

    
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Static instance hold the reference to the Singleton
    private static T _instance;
    
    //Public property to access Singleton instance
    public static T Instance {get{return _instance;}}
    
    [SerializeField]
    //Is the object persistent between scenes
    private bool _isPersistent = true;
    
    
    // Awake is called once at instantiation
    protected virtual void Awake()
    {
        //Check for singleton duplication
        ChekForSingelton();
    }//end Awake()

    void ChekForSingelton()
    {
        //If instance of singleton is empty
        if (_instance == null)
        {
            //make this object the instance 
            _instance = this as T;

            CheckForPresistance();
        }
        //Else if this object is not the instance, destroy this object
        else if (_instance != this)
        {
            Destroy(this);
        }
        
        //Log the current instance for debugging purposes
        Debug.Log(_instance);
        
    }//end CheckForSingleton()


    void CheckForPresistance()
    {
        if (_isPersistent)
        {
            //if a parent object exists
            if (transform.parent != null)
            {
                //detach object from parent
                transform.SetParent(null);
                Debug.Log($"detach {gameObject.name}");
            }
            
            //Keep object peresistent between scenes
           DontDestroyOnLoad(_instance);
           
        }//end if (isPersistent)
        
    }//end CheckForPresistance()
    
    

}//end Singleton.cs

}//end namespace
