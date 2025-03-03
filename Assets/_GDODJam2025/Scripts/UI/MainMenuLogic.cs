//Created by: Cayden Chancey

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Sirenix.OdinInspector;

using Tymski;

public class MainMenuLogic : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class GameManagerSettings
    {
        public SceneReference gameplayScene;
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    [Title("Settings")]

    [SerializeField]
    private GameManagerSettings settings;
    
    #endregion
    #region Unity Methods
    #endregion
    #region Core Methods

    public void StartGameClicked()
    {
        SceneManager.LoadScene(settings.gameplayScene);
    }
    
    public void OptionsClicked()
    {
    }
    
    public void CreditsClicked()
    {
    }
    
    public void ExitGameClicked()
    {
        Application.Quit();
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
