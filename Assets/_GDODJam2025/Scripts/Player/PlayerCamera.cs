using System;

using UnityEngine;

using Sirenix.OdinInspector;

public class PlayerCamera : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class PlayerCameraSettings
    {
        [FoldoutGroup("Look")]
        public Transform orientation;
        [FoldoutGroup("Look")]
        public float xSensitivity;
        [FoldoutGroup("Look")]
        public float ySensitivity;
    }

    #endregion
    #region Variables
    
    //--------------------SETTINGS--------------------
    
    [Title("Settings")]

    [SerializeField] 
    private PlayerCameraSettings settings;
    
    //--------------------PREVIEW--------------------

    [Title("Preview")] 
    
    [SerializeField, ReadOnly] 
    private float xRotation;

    [SerializeField, ReadOnly] 
    private float yRotation;
    
    //--------------------SINGLETON----------------------

    [Title("Singleton")]
    
    [SerializeField, ReadOnly]
    private bool dontDestroyOnLoad;
    
    [ReadOnly]
    public static PlayerCamera Instance { get; private set; }
    
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
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
    }
    
    #endregion
    #region Core Methods

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * settings.xSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * settings.ySensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        settings.orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        PlayerController.Instance.SetRotation(Quaternion.Euler(0f, yRotation, 0f));
    }

    #endregion
    #region Helpers
    #endregion
    #region Debug
    #endregion
}
