using UnityEngine;

/// Déverrouille quand le joueur entre dans le trigger ET possède l'item requis (ID string),
/// consomme l'item si demandé, active/désactive des objets et pose un flag de GameState.
[RequireComponent(typeof(Collider2D))]
public class UnlockColliderByInventoryAndSetFlag : MonoBehaviour
{
    [Header("Condition")]
    [Tooltip("ID de l'item requis (ex: Glass)")]
    public string requiredItemId = "Glass";

    [Tooltip("Force l'unlock même si l'inventaire n'est pas trouvé (debug)")]
    public bool debugAlwaysHasItem = false;

    [Header("Effets au déverrouillage")]
    [Tooltip("S'il est vide, on prendra le Collider2D de ce GameObject")]
    public Collider2D colliderToDisable;
    public GameObject[] disableOnUnlock;
    public GameObject[] enableOnUnlock;
    public bool consumeItem = true;

    [Header("Flag à poser")]
    [Tooltip("Ex: F_HandsFree")]
    public string flagToSet = "F_HandsFree";

    [Header("Détection")]
    [Tooltip("Tag du joueur (laisser vide pour accepter tout)")]
    public string playerTag = "Player";

    private bool _done;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        colliderToDisable = col;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_done) return;
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag)) return;

        if (!HasRequiredItem(other)) return;

        DoUnlock();
    }

    bool HasRequiredItem(Collider2D other)
    {
        if (debugAlwaysHasItem) return true;

        var inv = FindInventoryOn(other.gameObject) ?? FindInventoryInScene();
        if (inv == null)
        {
            Debug.LogWarning("[Unlock] Aucun PlayerInventory dans la scène/hiérarchie.");
            return false;
        }

        if (!inv.Has(requiredItemId)) return false;
        if (consumeItem) inv.Remove(requiredItemId);
        return true;
    }

    void DoUnlock()
    {
        if (colliderToDisable == null) colliderToDisable = GetComponent<Collider2D>();
        if (colliderToDisable != null) colliderToDisable.enabled = false;

        if (disableOnUnlock != null)
            foreach (var go in disableOnUnlock) if (go) go.SetActive(false);

        if (enableOnUnlock != null)
            foreach (var go in enableOnUnlock) if (go) go.SetActive(true);

        if (!string.IsNullOrEmpty(flagToSet)) GameState.Set(flagToSet);

        _done = true;
        Debug.Log($"[Unlock] OK avec '{requiredItemId}'. Flag posé: {flagToSet}");
    }

    // ---------- Helpers pour retrouver l'inventaire ----------

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
