using UnityEngine;
using UC;

public class ActManager : MonoBehaviour
{
    [SerializeField] private WSKit.EventType    startEvent;
    [SerializeField] private WSKit.EventType    updateEvent;
    [SerializeField] private Hypertag[]     activeTokens;

    private float _timeFromStart = 0.0f;

    public static ActManager Instance;
    public float timeFromStart => _timeFromStart;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _timeFromStart = 0.0f;

        GameManager.UpdateFonts();

        var tokenManager = GetComponent<TokenManager>();
        foreach (var t in activeTokens)
            tokenManager?.Add(t);

        var inventory = FindFirstObjectByType<Inventory>(); 
        if (inventory)
        {
            var inventoryDisplay = FindFirstObjectByType<InventoryDisplay>();
            if (inventoryDisplay)
            {
                inventoryDisplay.SetInventory(inventory);
            }
        }

        WSKit.OnEvent.TriggerEvent(startEvent);
    }

    private void Update()
    {
        _timeFromStart += Time.deltaTime;
        WSKit.OnEvent.TriggerEvent(updateEvent);
        GameManager.UpdateFonts();
    }    
}

