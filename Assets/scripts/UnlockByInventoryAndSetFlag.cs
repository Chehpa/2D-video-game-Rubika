using System;
using UnityEngine;
using UObj = UnityEngine.Object; // ← alias pour éviter l’ambiguïté sur Object

[RequireComponent(typeof(Collider2D))]
public class UnlockByInventoryAndSetFlag : MonoBehaviour
{
    [Header("Condition")]
    [Tooltip("ID d'item requis (ex: Glass)")]
    public string requiredItemId = "Glass";
    [Tooltip("Forcer l'unlock pour tester sans inventaire")]
    public bool debugAlwaysHasItem = false;

    [Header("Effets au déverrouillage")]
    [Tooltip("Si vide, prend le Collider2D de ce GameObject")]
    public Collider2D colliderToDisable;
    public GameObject[] disableOnUnlock;
    public GameObject[] enableOnUnlock;

    [Header("Flag à poser")]
    public string flagToSet = "F_HandsFree";

    [Header("Détection")]
    [Tooltip("Tag du joueur (laisser vide pour accepter tout)")]
    public string playerTag = "Player";

    private bool done;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        colliderToDisable = col;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (done) return;
        if (!string.IsNullOrEmpty(playerTag) && !other.CompareTag(playerTag)) return;

        if (debugAlwaysHasItem || HasRequiredItem(other.gameObject))
        {
            DoUnlock();
        }
    }

    private bool HasRequiredItem(GameObject playerGO)
    {
        var inv = FindInventoryOn(playerGO) ?? FindInventoryInScene();
        if (inv == null) return false;

        try
        {
            var mi = inv.GetType().GetMethod("HasItem", new[] { typeof(string) });
            if (mi != null)
            {
                var result = mi.Invoke(inv, new object[] { requiredItemId });
                if (result is bool b) return b;
            }
        }
        catch { /* ignore proprement */ }

        return false;
    }

    private object FindInventoryOn(GameObject go)
    {
        foreach (var c in go.GetComponentsInParent<Component>(true))
            if (c.GetType().Name == "PlayerInventory") return c;
        return null;
    }

    private object FindInventoryInScene()
    {
        // Unity 2023+ : API récente sans warning
#if UNITY_2023_1_OR_NEWER
        var list = UObj.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var c in list)
            if (c != null && c.GetType().Name == "PlayerInventory") return c;
        return null;
#else
        // Unity 2021/2022 : fallback (peut être obsolète mais OK)
        var list = UObj.FindObjectsOfType<MonoBehaviour>();
        foreach (var c in list)
            if (c != null && c.GetType().Name == "PlayerInventory") return c;
        return null;
#endif
    }

    private void DoUnlock()
    {
        if (colliderToDisable == null) colliderToDisable = GetComponent<Collider2D>();
        if (colliderToDisable != null) colliderToDisable.enabled = false;

        if (disableOnUnlock != null)
            foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
        if (enableOnUnlock != null)
            foreach (var go in enableOnUnlock) if (go) go.SetActive(true);

        GameState.Set(flagToSet);
        done = true;
        Debug.Log($"[Unlock] OK with '{requiredItemId}'. Flag set: {flagToSet}");
    }
}
