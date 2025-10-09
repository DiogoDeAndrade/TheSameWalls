using UC;
using UnityEngine;

public class HideIfInventoryEmpty : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Inventory   inventory;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = FindFirstObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((inventory) && (canvasGroup))
        {
            if (inventory.HasItems())
            {
                canvasGroup.FadeIn(0.5f);
            }
            else
            {
                canvasGroup.FadeOut(0.5f);
            }
        }
    }
}
