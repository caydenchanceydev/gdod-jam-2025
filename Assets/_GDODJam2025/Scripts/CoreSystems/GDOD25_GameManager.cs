//Created by: Cayden Chancey

using System;
using UnityEngine;

public class GDOD25_GameManager : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class GameManagerSettings
    {
        
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    //[Title("Settings")]

    [SerializeField]
    private GameManagerSettings settings;
    
    //--------------------SINGLETON----------------------

    //[ReadOnly]
    public static GDOD25_GameManager Instance { get; private set; }

    private bool dontDestroyOnLoad;
    
    #endregion
    #region Unity Methods
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    
    #endregion
    #region Core Methods

    public void StartGame()
    {
        
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
