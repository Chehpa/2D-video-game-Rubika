using UnityEngine;

/// Ramasse un objet d'inventaire identifié par une string (ex: "Glass", "HairPin").
/// Fonctionne en "auto-pick" quand le joueur entre dans le trigger, ou via appel manuel.
[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour
{
    [Header("Item")]
    [Tooltip("ID de l'item (ex: Glass, HairPin)")]
    public string itemId = "Glass";

    [Tooltip("Détruire le GameObject après la prise")]
    public bool autoDestroyOnPick = true;

    [Header("Détection")]
    [Tooltip("Tag du joueur (laisser vide pour accepter tout)")]
    public string playerTag = "Player";

    Collider2D _col;

    void Reset()
    {
        _col = GetComponent<Collider2D>();
        if (_col != null) _col.isTrigger = true;
    }

    void Awake()
    {
        if (_col == null) _col = GetComponent<Collider2D>();
        if (_col != null) _col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag)) return;
        TryPickup(other.gameObject);
    }

    /// Permet un ramassage manuel (ex: appelé par un script d'interaction au clavier).
    public bool TryPickup(GameObject picker)
    {
        if (string.IsNullOrEmpty(itemId)) return false;

        // Cherche un PlayerInventory sur le joueur ou son parent
        var inv = FindInventoryOn(picker) ?? FindInventoryInScene();
        if (inv == null)
        {
            Debug.LogWarning("[PickupItem] Aucun PlayerInventory trouvé.");
            return false;
        }

        inv.Add(itemId);
        Debug.Log($"[PickupItem] Pris: {itemId}");

        if (autoDestroyOnPick) Destroy(gameObject);
        else if (_col != null) _col.enabled = false;

        return true;
    }

    // ---------- Helpers ----------
    PlayerInventory FindInventoryOn(GameObject go)
    {
        foreach (var c in go.GetComponentsInParent<Component>(true))
            if (c is PlayerInventory p) return p;
        return null;
    }

    PlayerInventory FindInventoryInScene()
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindFirstObjectByType<PlayerInventory>(FindObjectsInactive.Include);
#else
        return Object.FindObjectOfType<PlayerInventory>();
#endif
    }
}
