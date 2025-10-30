using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UnlockColliderByInventory : MonoBehaviour
{
    public ItemId requiredItem = ItemId.HairPin;
    public FlagId setOnUnlock = FlagId.F_DoorOpen;
    public GameObject[] enableOnUnlock;
    public GameObject[] disableOnUnlock;
    public bool consumeItem = false;

    void Reset() { var c = GetComponent<Collider2D>(); c.isTrigger = true; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Blindages anti-NullRef
        if (GameStateHost.I == null)
        {
            Debug.LogWarning("[UnlockColliderByInventory] GameStateHost absent, ignore.");
            return;
        }

        if (!GameStateHost.I.HasItem(requiredItem))
        {
            Debug.Log("[Portal] Door: it's locked... need a hair pin.");
            return;
        }

        if (consumeItem) GameStateHost.I.RemoveItem(requiredItem);
        GameStateHost.I.SetFlag(setOnUnlock);

        if (enableOnUnlock != null)
            foreach (var go in enableOnUnlock) if (go) go.SetActive(true);

        if (disableOnUnlock != null)
            foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
    }
}
