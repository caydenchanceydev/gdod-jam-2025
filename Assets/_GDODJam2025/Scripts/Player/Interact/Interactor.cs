using System;

using UnityEngine;

using Sirenix.OdinInspector;

public interface IInteractable
{
    public void ShowText(string keycode, bool show);
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class InteractorSettings
    {
        public KeyCode interactKey;
        public Transform interactSource;
        public float interactRange;
        public LayerMask interactLayer;
    }

    #endregion
    #region Variables
    
    //--------------------SETTINGS--------------------
    
    [Title("Settings")]

    [SerializeField] 
    private InteractorSettings settings;
    
    //--------------------PREVIEW--------------------
    
    [Title("Preview")]
    private IInteractable lastSeenInteractable;
    
    #endregion
    #region Unity Methods

    private void Update()
    {
        CheckForInteractables();
    }

    #endregion
    #region Core Methods

    private void CheckForInteractables()
    {
        Ray ray = new Ray(settings.interactSource.position, settings.interactSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, settings.interactRange, settings.interactLayer, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                lastSeenInteractable = interactable;
                
                interactable.ShowText(settings.interactKey.ToString(), true);
                
                if (Input.GetKeyDown(settings.interactKey))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            if(lastSeenInteractable == null)
                return;
                    
            lastSeenInteractable.ShowText(settings.interactKey.ToString(), false);
            lastSeenInteractable = null;
        }
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
