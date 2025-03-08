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
        public string mainMenuSceneName;
        
        public int totalInteractionsNeeded;
        public TextMeshProUGUI counterText;
        
        public CanvasGroup endScreenCanvasGroup;
        public float timeToFadeInEndScreen;

        public GameObject winScreen;
        public RectTransform winTextTransform1;
        public RectTransform winTextTransform2;
        public float win_delayBetweenScreenFadeAndTextScale;
        public float winDelay;
        public float winDelayToRestart;

        public GameObject lossScreen;
        public TextMeshProUGUI lossText;
        
        [FormerlySerializedAs("delayBetweenScreenFadeAndTextScale")] 
        public float loss_delayBetweenScreenFadeAndTextScale;
        
        public float lossTextScaleTime;
        public float timeAtMaxBladderToLose;
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
        
        settings.winScreen.SetActive(true);
        
        settings.endScreenCanvasGroup.DOFade(1f, settings.timeToFadeInEndScreen);
        
        yield return new WaitForSecondsRealtime(settings.winDelayToRestart);
        
        RestartGame();
    }

    private IEnumerator LoseGame()
    {
        yield return new WaitForSecondsRealtime(settings.lossDelay);
        
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
                StartCoroutine(LoseGame());
            }
        }
        else
        {
            bladderMaxStartTime = 0f;
        }
    }

    #endregion
    #region Debug

    [Button("Lose Game")]
    private void Debug_LoseGame()
    {
        gameInProgress = false;
        StartCoroutine(LoseGame());
    }
    
    [Button("Win Game")]
    private void Debug_WinGame()
    {
        gameInProgress = false;
        StartCoroutine(WinGame());
    }

    #endregion
}
