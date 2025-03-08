using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

public class BarManager : MonoBehaviour
{
    [Title("UI")]
    public Slider drinkSlider;
    public Slider bladderSlider;
    public Slider anxietySlider;
    
    [Title("Start Values")]
    [Range(0, 1)] 
    public float drinkStart;
    [Range(0, 1)] 
    public float bladderStart;
    [Range(0, 1)] 
    public float anxietyStart;
    
    [Title("Current Values")]
    [Range(0, 1), ReadOnly] 
    public float drink;
    [Range(0, 1), ReadOnly] 
    public float bladder;
    [Range(0, 1), ReadOnly] 
    public float anxiety;
    
    [Title("Tick Settings")]
    public float tickRate;
    
    [Title("Passive Drain Rates")]
    public float drinkDrainRate;  // Passive Drink decrease per tick

    [Title("Threshold Ranges")] 
    [Range(0, 1)]
    public float drinkMinBladder;
    [Range(0, 1)]
    public float drinkMaxBladder;
    [Range(0, 1)] 
    public float drinkMinAnxiety;
    [Range(0, 1)]
    public float drinkMaxAnxiety;
    [Range(0, 1)] 
    public float bladderMinAnxiety;
    [Range(0, 1)]
    public float bladderMaxAnxiety;
    
    [Title("Influence Multipliers")]
    public float drinkToBladder;
    public float drinkToAnxiety;
    public float bladderToAnxiety;
    
    //--------------------SINGLETON----------------------

    [Title("Singleton")]
    
    [SerializeField]
    private bool dontDestroyOnLoad;
    
    [ReadOnly]
    public static BarManager Instance { get; private set; }
    
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
        SetStartValues();
        
        InvokeRepeating(nameof(UpdateStatus), tickRate, tickRate);
    }

    private void SetStartValues()
    {
        drink = drinkStart;
        bladder = bladderStart;
        anxiety = anxietyStart;
        
        drinkSlider.value = drink;
        bladderSlider.value = bladder;
        anxietySlider.value = anxiety;
    }

    private void UpdateStatus()
    {
        // Passive drink drain
        drink -= drinkDrainRate;
        
        // Drink influences bladder
        if (drink >= drinkMinBladder && drink <= drinkMaxBladder)
        {
            bladder += drink * drinkToBladder;
        }

        // Drink influences anxiety
        if (drink >= drinkMinAnxiety && drink <= drinkMaxAnxiety)
        {
            anxiety += drink * drinkToAnxiety;
        }

        // Bladder influences anxiety
        if (bladder >= bladderMinAnxiety && bladder <= bladderMaxAnxiety)
        {
            anxiety += bladder * bladderToAnxiety;
        }
        
        drink = Mathf.Clamp01(drink);
        bladder = Mathf.Clamp01(bladder);
        anxiety = Mathf.Clamp01(anxiety);
        
        drinkSlider.value = drink;
        bladderSlider.value = bladder;
        anxietySlider.value = anxiety;
    }
    
    [Title("Debug Interaction")]
    
    [Button("Modify Drink")]
    public void ModifyDrink(float amount)
    {
        drink = Mathf.Clamp01(drink + amount);
    }

    [Button("Modify Anxiety")]
    public void ModifyAnxiety(float amount)
    {
        anxiety = Mathf.Clamp01(anxiety + amount);
    }
    
    [Button("Modify Bladder")]
    public void ModifyBladder(float amount)
    {
        bladder = Mathf.Clamp01(bladder + amount);
    }
}
