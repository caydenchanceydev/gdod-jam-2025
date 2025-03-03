using System;

using UnityEngine;

using Sirenix.OdinInspector;

public class Interactable : MonoBehaviour, IInteractable
{
    #region Data Structures

    [Serializable]
    public class InteractableSettings
    {
        public string interactableText;
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    [Title("Settings")]

    [SerializeField]
    private InteractableSettings settings;
    
    #endregion
    #region Unity Methods
    #endregion
    #region Core Methods

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public void ShowText(string keycode, bool show)
    {
        string textToShow = $"{settings.interactableText}\n[{keycode}] ";
        InteractableUIManager.Instance.ShowText(textToShow, show);
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
