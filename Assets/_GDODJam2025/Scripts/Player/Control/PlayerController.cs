//Created by: Cayden Chancey

using System;

using UnityEngine;

using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    #region Data Structures

    [Serializable]
    public class PlayerControllerSettings
    {
        [FoldoutGroup("Movement")]
        public Transform orientation;
        [FoldoutGroup("Movement")]
        public Rigidbody rb;
        [FoldoutGroup("Movement")]
        public float speed;
        [FoldoutGroup("Movement")]
        public float groundDampening;

        [FoldoutGroup("Ground Check")] 
        public float playerHeight;
        [FoldoutGroup("Ground Check")] 
        public float groundCheckDistance;
        [FoldoutGroup("Ground Check")]
        public LayerMask groundLayer;
    }

    #endregion
    #region Variables
    
    //--------------------SETTINGS--------------------
    
    [Title("Settings")]

    [SerializeField] 
    private PlayerControllerSettings settings;
    
    //--------------------PREVIEW--------------------
    
    [Title("Preview")]
    
    [SerializeField, ReadOnly]
    private bool isGrounded;

    [SerializeField, ReadOnly] 
    private float horizontalInput;

    [SerializeField, ReadOnly] 
    private float verticalInput;

    [SerializeField, ReadOnly] 
    private Vector3 moveDirection;
    
    //--------------------SINGLETON----------------------

    [Title("Singleton")]
    
    [SerializeField, ReadOnly]
    private bool dontDestroyOnLoad;
    
    [ReadOnly]
    public static PlayerController Instance { get; private set; }
    
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
        if(settings.rb == null)
            settings.rb = GetComponent<Rigidbody>();
        
        settings.rb.freezeRotation = true;
    }

    void Update()
    {
        GroundedCheck();
        
        InputCollection();
        
        SpeedCheck();
    }

    void FixedUpdate()
    {
        FPSMovement();
    }

    #endregion
    #region Core Methods

    private void InputCollection()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void GroundedCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, (float)(settings.playerHeight * 0.5) + settings.groundCheckDistance, settings.groundLayer, QueryTriggerInteraction.Ignore);
    
        settings.rb.linearDamping = isGrounded ? settings.groundDampening : 0;
    }

    private void FPSMovement()
    {
        moveDirection = settings.orientation.forward * verticalInput + settings.orientation.right * horizontalInput;
        settings.rb.AddForce(moveDirection.normalized * (settings.speed * 10f), ForceMode.Force);
    }

    private void SpeedCheck()
    {
        Vector3 flatVelocity = new Vector3(settings.rb.linearVelocity.x, 0f, settings.rb.linearVelocity.z);

        if (flatVelocity.magnitude > settings.speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * settings.speed;
            settings.rb.linearVelocity = new Vector3(limitedVelocity.x, settings.rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    #endregion
    #region Helpers

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    #endregion
    #region Debug
    #endregion

    
}
