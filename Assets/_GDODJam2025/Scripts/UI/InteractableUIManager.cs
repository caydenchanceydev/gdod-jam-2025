using System;

using UnityEngine;

using TMPro;

using Sirenix.OdinInspector;

public class InteractableUIManager : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class InteractableUIManagerSettings
    {
        public GameObject interactTextObj;
        public TextMeshProUGUI interactText;

        public Color regularColor;
        public Color nonInteractColor;
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    [Title("Settings")]

    [SerializeField]
    private InteractableUIManagerSettings settings;
    
    //--------------------SINGLETON----------------------

    [Title("Singleton")]
    
    [SerializeField, ReadOnly]
    private bool dontDestroyOnLoad;
    
    [ReadOnly]
    public static InteractableUIManager Instance { get; private set; }
    
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

    public void ShowText(string text, bool interactable, bool active)
    {
        if(active)
            settings.interactText.text = text;
        
        settings.interactText.color = interactable ? settings.regularColor : settings.nonInteractColor;
        
        settings.interactTextObj.SetActive(active);
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
