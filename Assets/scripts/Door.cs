using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour, IInteractable
{
    [Header("Lock")]
    public bool requireHairPin = true;

    public string Prompt => requireHairPin ? "Pick the lock [E]" : "Open door [E]";
    private bool isOpen;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact(PlayerInventory inv)
    {
        if (isOpen) return;

        if (!requireHairPin || inv.Has(ItemType.HairPin))
        {
            isOpen = true;
            Destroy(gameObject); // v1 placeholder (anim plus tard)
        }
        else
        {
            Debug.Log("[Door] It's locked… need something thin.");
        }
    }
}
