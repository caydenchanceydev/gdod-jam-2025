using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class Interactable : MonoBehaviour, IInteractable
{
    #region Data Structures

    [Serializable]
    public enum Effects
    {
        Drink = 0,
        Anxiety = 1,
        Piss = 2
    }

    [Serializable]
    public class EffectCost
    {
        public Effects effect;
        public float requirement;
    }

    [Serializable]
    public class InteractableSettings
    {
        public string interactableText;

        public bool isPersonInteraction;
        public int numPeople = 1;
        
        public bool oneTimeUse;
        public bool turnOffAfterUse;
        
        public List<EffectCost> requiredEffects;
        public List<EffectCost> givenEffects;
        
        public AudioSource audioToPlay;
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    [Title("Settings")]

    [SerializeField]
    private InteractableSettings settings;
    
    //----------------------PREVIEW---------------------

    [Title("Preview")] 
    
    [SerializeField, ReadOnly] 
    private bool hasBeenActivated;
    
    #endregion
    #region Unity Methods
    #endregion
    #region Core Methods

    public void Interact()
    {
        if (settings.oneTimeUse && hasBeenActivated)
            return;

        if (!CheckRequirements())
            return;
        
        Debug.Log("Interacted with " + gameObject.name);
        
        if(settings.isPersonInteraction)
            GDOD25_GameManager.Instance.TalkedToPerson(settings.numPeople);

        if (settings.audioToPlay != null)
        {
            settings.audioToPlay.Play();
        }

        ApplyEffects();

        if (settings.oneTimeUse && settings.turnOffAfterUse)
        {
            hasBeenActivated = true;
            gameObject.SetActive(false);
        }
    }

    public void ShowText(string keycode, bool show)
    {
        string textToShow = $"{settings.interactableText}\n[{keycode}] ";
        InteractableUIManager.Instance.ShowText(textToShow, CheckRequirements(), show);
    }

    #endregion
    #region Helpers

    private bool CheckRequirements()
    {
        foreach (EffectCost ec in settings.requiredEffects)
        {
            switch (ec.effect)
            {
                case Effects.Drink:
                    if (BarManager.Instance.drink < ec.requirement)
                        return false;
                    break;
                case Effects.Anxiety:
                    if (BarManager.Instance.anxiety > ec.requirement)
                        return false;
                    break;
                case Effects.Piss:
                    if (BarManager.Instance.bladder < ec.requirement)
                        return false;
                    break;
                default:
                    break;
            }
        }

        return true;
    }

    private void ApplyEffects()
    {
        foreach (EffectCost ec in settings.givenEffects)
        {
            switch (ec.effect)
            {
                case Effects.Drink:
                    BarManager.Instance.ModifyDrink(ec.requirement);
                    break;
                case Effects.Anxiety:
                    BarManager.Instance.ModifyAnxiety(ec.requirement);
                    break;
                case Effects.Piss:
                    BarManager.Instance.ModifyBladder(ec.requirement);
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
    #region Debug
    #endregion
}
