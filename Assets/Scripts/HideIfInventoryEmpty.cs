using UC;
using UnityEngine;

public class HideIfInventoryEmpty : MonoBehaviour
{
    CanvasGroup         canvasGroup;
    Inventory           inventory;
    InventoryDisplay    inventoryDisplay;
    bool                inventoryOpen = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = FindFirstObjectByType<Inventory>();
        if (inventory)
        {
            inventory.onChange += Inventory_onChange;
            inventoryDisplay = FindFirstObjectByType<InventoryDisplay>();
            if (inventoryDisplay) inventoryDisplay.onInventoryToggle += InventoryDisplay_onInventoryToggle;
        }

        UpdateInventory();
    }

    private void InventoryDisplay_onInventoryToggle(bool open)
    {
        if (open)
            canvasGroup.FadeOut(0.15f);
        else
            canvasGroup.FadeIn(0.15f);
    }

    private void Inventory_onChange(bool add, Item item, int slot)
    {
        UpdateInventory();
    }

    private 

    // Update is called once per frame
    void UpdateInventory()
    {
        if ((inventory) && (canvasGroup))
        {
            if (inventory.HasItems())
            {
                canvasGroup.FadeIn(0.25f);
            }
            else
            {
                canvasGroup.FadeOut(0.25f);
            }
        }
    }
}
