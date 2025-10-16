using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HandsRope : MonoBehaviour, IInteractable
{
    public string Prompt => "Cut the rope [E]";
    private bool cut;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact(PlayerInventory inv)
    {
        if (cut) return;

        if (inv.Has(ItemType.GlassShard))
        {
            cut = true;
            Debug.Log("[Rope] Hands freed.");
            Destroy(gameObject); // placeholder: tu mettras une anim plus tard
        }
        else
        {
            Debug.Log("[Rope] You need something sharp.");
        }
    }
}
