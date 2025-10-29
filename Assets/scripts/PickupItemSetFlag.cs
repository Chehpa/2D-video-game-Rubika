using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItemSetFlag : MonoBehaviour
{
    [Header("Pickup")]
    public string itemId = "Glass";
    public bool addToInventory = true;
    public bool autoDestroyOnPick = true;

    [Header("Flag à poser")]
    public string flagToSet = "F_HandsFree";

    Collider2D _col;

    void Reset()
    {
        _col = GetComponent<Collider2D>();
        if (_col != null) _col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        TryPickup(other.gameObject);
    }

    public void TryPickup(GameObject picker)
    {
        if (addToInventory && !string.IsNullOrEmpty(itemId))
        {
            var inv = FindInventoryOn(picker) ?? FindInventoryInScene();
            if (inv != null) inv.AddItem(itemId);
        }

        if (!string.IsNullOrEmpty(flagToSet)) GameState.Set(flagToSet);

        if (autoDestroyOnPick) Destroy(gameObject);
        else if (_col != null) _col.enabled = false;
    }

    PlayerInventory FindInventoryOn(GameObject go)
    {
        foreach (var c in go.GetComponentsInParent<Component>(true))
            if (c is PlayerInventory p) return p;
        return null;
    }
    PlayerInventory FindInventoryInScene()
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindFirstObjectByType<PlayerInventory>();
#else
        return Object.FindObjectOfType<PlayerInventory>();
#endif
    }
}
