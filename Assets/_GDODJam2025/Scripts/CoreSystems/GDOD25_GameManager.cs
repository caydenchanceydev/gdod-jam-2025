//Created by: Cayden Chancey

using System;
using System.Collections;
using UnityEngine;

using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GDOD25_GameManager : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class GameManagerSettings
    {
        public GameObject hud;
        
        public string mainMenuSceneName;
        
        public int totalInteractionsNeeded;
        public TextMeshProUGUI counterText;
        
        public CanvasGroup endScreenCanvasGroup;
        public float timeToFadeInEndScreen;

        public GameObject winScreen;
        public float winDelay;
        public float winDelayToRestart;

        public GameObject lossScreen;
        public TextMeshProUGUI lossText;
        
        [FormerlySerializedAs("delayBetweenScreenFadeAndTextScale")] 
        public float loss_delayBetweenScreenFadeAndTextScale;
        
        public float lossTextScaleTime;
        public float timeAtMaxBladderToLose;
        public float timeAtMaxDrinkToLose;
        public float lossDelay;
        public float lossDelayToRestart;
    }

    #endregion
    #region Variables
    
    //----------------------SETTINGS---------------------

    [Title("Settings")]

    [SerializeField]
    private GameManagerSettings settings;
    
    //----------------------PREVIEW---------------------

    [Title("PREVIEW")] 
    
    [SerializeField, ReadOnly]
    public bool gameInProgress;
    
    [SerializeField, ReadOnly]
    public int interactionCounterValue;
    
    [SerializeField, ReadOnly]
    public float bladderMaxStartTime;
    
    [SerializeField, ReadOnly]
    public float drinkMaxStartTime;
    
    //--------------------SINGLETON----------------------

    [Title("Singleton")]
    
    [SerializeField]
    private bool dontDestroyOnLoad;
    
    [ReadOnly]
    public static GDOD25_GameManager Instance { get; private set; }
    
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
        
        gameInProgress = true;
    }

    private void Update()
    {
        if (!gameInProgress)
            return;
        
        CheckWinConditions();
        CheckLossConditions();
    }

    #endregion
    #region Core Methods

    public void TalkedToPerson(int numPeople)
    {
        interactionCounterValue += numPeople;
        settings.counterText.text = $"{interactionCounterValue.ToString()} / {settings.totalInteractionsNeeded.ToString()}";
    }

    private IEnumerator WinGame()
    {
        yield return new WaitForSecondsRealtime(settings.winDelay);
        
        settings.hud.gameObject.SetActive(false);
        
        settings.winScreen.SetActive(true);
        
        settings.endScreenCanvasGroup.DOFade(1f, settings.timeToFadeInEndScreen);
        
        yield return new WaitForSecondsRealtime(settings.winDelayToRestart);
        
        RestartGame();
    }

    private IEnumerator LoseGame(bool fromDrink)
    {
        yield return new WaitForSecondsRealtime(settings.lossDelay);
        
        settings.hud.gameObject.SetActive(false);

        if (fromDrink)
            settings.lossText.text = "YOU BLACKED OUT";
        else
            settings.lossText.text = "YOU PISSED YOURSELF";
        
        settings.lossScreen.SetActive(true);
        settings.endScreenCanvasGroup.DOFade(1f, settings.timeToFadeInEndScreen);
        
        yield return new WaitForSecondsRealtime(settings.loss_delayBetweenScreenFadeAndTextScale);
        
        settings.lossText.transform.DOScale(Vector3.one, settings.lossTextScaleTime);
        
        yield return new WaitForSecondsRealtime(settings.lossDelayToRestart);
        
        RestartGame();
    }

    private void RestartGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene(settings.mainMenuSceneName);
    }

    #endregion
    #region Helpers

    private void CheckWinConditions()
    {
        if (interactionCounterValue >= settings.totalInteractionsNeeded)
        {
            gameInProgress = false;
            StartCoroutine(WinGame());
        }
    }
    
    private void CheckLossConditions()
    {
        if (BarManager.Instance.bladder >= 1f)
        {
            bladderMaxStartTime = Time.time;

            if (Time.time - bladderMaxStartTime >= settings.timeAtMaxBladderToLose)
            {
                gameInProgress = false;
                StartCoroutine(LoseGame(false));
            }
        }
        else
        {
            bladderMaxStartTime = 0f;
        }
        
        if (BarManager.Instance.drink >= 1f)
        {
            drinkMaxStartTime = Time.time;

            if (Time.time - drinkMaxStartTime >= settings.timeAtMaxDrinkToLose)
            {
                gameInProgress = false;
                StartCoroutine(LoseGame(true));
            }
        }
        else
        {
            drinkMaxStartTime = 0f;
        }
    }

    #endregion
    #region Debug

    [Button("Lose Game")]
    private void Debug_LoseGame(bool fromDrink)
    {
        gameInProgress = false;
        StartCoroutine(LoseGame(fromDrink));
    }
    
    [Button("Win Game")]
    private void Debug_WinGame()
    {
        gameInProgress = false;
        StartCoroutine(WinGame());
    }

    #endregion
}
