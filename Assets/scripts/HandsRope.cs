using UnityEngine;

/// Débloque les mains du joueur si son inventaire contient l'item requis (ex: "Glass").
/// Coupe la corde: désactive le collider, applique des effets visuels et pose le flag "F_HandsFree".
[RequireComponent(typeof(Collider2D))]
public class HandsRope : MonoBehaviour
{
    [Header("Condition")]
    [Tooltip("ID de l'item requis (ex: Glass)")]
    public string requiredItemId = "Glass";

    [Tooltip("Consommer l'item à l'ouverture")]
    public bool consumeItem = true;

    [Header("Effets à l'ouverture")]
    [Tooltip("Si vide, prend le Collider2D présent sur ce GameObject")]
    public Collider2D colliderToDisable;
    public GameObject[] disableOnUnlock;
    public GameObject[] enableOnUnlock;

    [Header("Flag posé")]
    public string flagToSet = "F_HandsFree";

    [Header("Détection")]
    [Tooltip("Tag du joueur (laisser vide pour accepter tout)")]
    public string playerTag = "Player";

    bool _done;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        colliderToDisable = col;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_done) return;
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag)) return;

        TryUnlock(other.gameObject);
    }

    /// Appel manuel possible si besoin (ex: depuis un autre script)
    public bool TryUnlock(GameObject actor)
    {
        if (_done) return false;

        var inv = FindInventoryOn(actor) ?? FindInventoryInScene();
        if (inv == null)
        {
            Debug.LogWarning("[HandsRope] Aucun PlayerInventory trouvé.");
            return false;
        }

        if (!inv.Has(requiredItemId)) return false;

        if (consumeItem) inv.Remove(requiredItemId);

        if (colliderToDisable == null) colliderToDisable = GetComponent<Collider2D>();
        if (colliderToDisable != null) colliderToDisable.enabled = false;

        foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
        foreach (var go in enableOnUnlock) if (go) go.SetActive(true);

        if (!string.IsNullOrEmpty(flagToSet)) GameState.Set(flagToSet);

        _done = true;
        Debug.Log($"[HandsRope] Débloqué avec «{requiredItemId}». Flag posé: {flagToSet}");
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
