
using UnityEngine;
using UC;
using System.Collections;

namespace WSKit
{

    [System.Serializable]
    public class GameAction_EquipItem : GameAction
    {
        [SerializeField] private Hypertag   slot;
        [SerializeField] private Item       item;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            if (item == null)
            {
                Debug.LogWarning("No item specified to add.");
            }
            var equipmentManager = GameObject.FindFirstObjectByType<Equipment>();
            if (equipmentManager == null)
            {
                Debug.LogWarning($"Can't find equipment manager.");
            }
            equipmentManager.Equip(slot, item);

            yield return null;
        }
    }
}
