using UC;
using UnityEngine;

namespace WSKit
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Hypertag   handSlot;
        [SerializeField] Transform  handSlotTransform;

        
        Equipment           equipmentManager;
        InventoryDisplay    inventoryDisplay;
        CursorManager       cursorManager;
        FPSController       fpsController;
        GameObject          equippedItem;
        Item                lastItemEquipped;

        void Start()
        {
            inventoryDisplay = FindFirstObjectByType<InventoryDisplay>();
            if (inventoryDisplay)
            {
                inventoryDisplay.onInventoryToggle += InventoryDisplay_onInventoryToggle;
                inventoryDisplay.onInventorySelect += InventoryDisplay_onInventorySelect;
            }
            equipmentManager = FindFirstObjectByType<Equipment>();
            if (equipmentManager)
            {
                equipmentManager.onChange += EquipmentManager_onChange;
            }
            fpsController = GetComponent<FPSController>();
            cursorManager = FindFirstObjectByType<CursorManager>();
        }

        private void InventoryDisplay_onInventorySelect(Item item)
        {
            if (item != null)
            {
                equipmentManager.Equip(handSlot, item);
                inventoryDisplay.Close();
            }
            else
            {
                equipmentManager.Unequip(handSlot);
            }
        }

        private void EquipmentManager_onChange(bool equip, Hypertag slot, Item item)
        {
            if (slot != handSlot) return;

            if ((item) && (equip) && (lastItemEquipped != item))
            {
                RemoveEquippedItem();

                if (item.scenePrefab != null)
                {
                    equippedItem = Instantiate(item.scenePrefab, handSlotTransform);
                }

                lastItemEquipped = item;
            }
            else
            {
                if (lastItemEquipped != null) 
                {
                    RemoveEquippedItem();
                }
            }
        }

        void RemoveEquippedItem()
        {
            if (equippedItem != null) 
            {
                equippedItem.Delete();
                equippedItem = null;
                lastItemEquipped = null;
            }
        }

        private void InventoryDisplay_onInventoryToggle(bool open)
        {
            fpsController.enabled = !open;
            Cursor.lockState = (open) ? (CursorLockMode.None) : (CursorLockMode.Locked);
            Cursor.visible = open;
            cursorManager.SetCursor(!open);
        }
    }
}