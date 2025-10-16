using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour, IInteractable
{
    public ItemType itemType;
    public string Prompt => $"Take {itemType} [E]";

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact(PlayerInventory inv)
    {
        inv.Add(itemType);
        Destroy(gameObject);
    }
}
