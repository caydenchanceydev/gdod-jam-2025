using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

public class HoldKeyToActivateVFX : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class HoldKeyToActivateVFXSettings
    {
        public List<ParticleSystem> vfx;
        public KeyCode interactKey;
        public float timeToDetermineHold;
    }

    #endregion
    #region Variables
    
    //--------------------SETTINGS--------------------
    
    [Title("Settings")]

    [SerializeField] 
    private HoldKeyToActivateVFXSettings settings;
    
    //--------------------PREVIEW--------------------

    [Title("Preview")] 
    
    [SerializeField, ReadOnly]
    private bool isVFXActive;

    [SerializeField, ReadOnly]
    private float keyPressStart;
    
    //--------------------QUERIES--------------------
    
    [Title("Queries")]
    
    public bool IsVFXActive => isVFXActive;
    
    //--------------------EVENTS--------------------
    
    [Title("Events")]
    
    public UnityEvent OnVFXActivate;
    
    public UnityEvent OnVFXDeactivate;
    
    #endregion
    #region Unity Methods

    private void Update()
    {
        if (Input.GetKeyDown(settings.interactKey))
        {
            keyPressStart = Time.time;
        }
        else 
        if (Input.GetKey(settings.interactKey))
        {
            if (Time.time - keyPressStart <= settings.timeToDetermineHold)
                return;

            if (isVFXActive)
                return;
            
            SetVFXActive(true);
        }
        else 
        if (Input.GetKeyUp(settings.interactKey))
        {
            SetVFXActive(false);
        }
    }

    #endregion
    #region Core Methods

    private void SetVFXActive(bool active)
    {
        if (active)
        {
            foreach (ParticleSystem fx in settings.vfx)
                fx.Play();
        }
        else
        {
            foreach (ParticleSystem fx in settings.vfx)
                fx.Stop();
        }

        isVFXActive = active;
        
        if(active)
            OnVFXActivate?.Invoke();
        else
            OnVFXDeactivate?.Invoke();
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
