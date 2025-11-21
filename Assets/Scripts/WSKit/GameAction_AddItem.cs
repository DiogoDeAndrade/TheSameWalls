using UnityEngine;
using UC;
using System.Collections;
using UC.Interaction;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Add Item")]
    public class GameAction_AddItem : GameAction
    {
        [SerializeField] private Item   item;
        [SerializeField] private int    quantity = 1;

        public override IEnumerator Execute(GameObject referenceObject)
        {
            if (item == null)
            {
                Debug.LogWarning("No item specified to add.");
            }
            var inventory = GameObject.FindFirstObjectByType<Inventory>();
            if (inventory == null)
            {
                Debug.LogWarning($"Can't find inventory.");
            }
            inventory?.Add(item, quantity);

            yield return null;
        }
    }
}
