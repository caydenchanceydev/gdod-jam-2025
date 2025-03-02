//Created by: Cayden Chancey

using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    #region Data Structures
    #endregion
    #region Variables
    #endregion
    #region Unity Methods
    #endregion
    #region Core Methods

    public void StartGameClicked()
    {
        GDOD25_GameManager.Instance.StartGame();
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
