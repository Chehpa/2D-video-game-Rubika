using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour
{
    public string itemId = "key_red";
    public bool destroyOnPickup = true;

    private void Reset()
    {
        // sécurité: si tu oublies le trigger, on le coche
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // on ne parle qu'au player
        if (!other.CompareTag("Player"))
            return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null && !string.IsNullOrEmpty(itemId))
        {
            inv.Add(itemId);
            Debug.Log("[Pickup] ADDED to inventory: " + itemId);
        }

        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
