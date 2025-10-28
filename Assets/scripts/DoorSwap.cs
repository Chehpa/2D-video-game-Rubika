using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorSwap : MonoBehaviour
{
    [Header("Lock")]
    [Tooltip("ID requis dans l'inventaire (ex: HairPin)")]
    public string requiredItemId = "HairPin";
    [Tooltip("Consommer l'objet à l'ouverture ?")]
    public bool consumeItem = false;

    [Header("Visuals / Collisions")]
    [Tooltip("Visuel porte fermée (optionnel)")]
    public GameObject closedGO;
    [Tooltip("Visuel porte ouverte (optionnel)")]
    public GameObject openGO;
    [Tooltip("Collider plein qui bloque le passage quand c'est fermé")]
    public Collider2D blockingCollider;

    [Header("UI")]
    public string needText = "Door: it's locked… need a hair pin.";

    bool _isOpen;

    void Reset()
    {
        // Le collider porteur de ce script sert de trigger d'activation
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;

        // Essaie de deviner un collider bloquant sur ce GO si rien n'est réglé
        if (blockingCollider == null)
        {
            // Si on a deux colliders: le 1er trigger (ce script), le 2e blocant
            var cols = GetComponents<Collider2D>();
            if (cols.Length > 1)
            {
                foreach (var c in cols)
                    if (!c.isTrigger) { blockingCollider = c; break; }
            }
        }

        Refresh();
    }

    void OnValidate() { Refresh(); }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponentInParent<PlayerInventory>() ?? FindObjectOfType<PlayerInventory>();
        if (inv == null)
        {
            Debug.LogWarning("[DoorSwap] Aucun PlayerInventory trouvé.");
            return;
        }

        if (inv.HasItem(requiredItemId))
        {
            Open(inv);
        }
        else
        {
            Debug.Log($"[Door] {needText}");
        }
    }

    public void Open(PlayerInventory inv = null)
    {
        if (_isOpen) return;
        _isOpen = true;

        if (consumeItem && inv != null)
            inv.RemoveItem(requiredItemId);

        if (blockingCollider) blockingCollider.enabled = false;
        if (closedGO) closedGO.SetActive(false);
        if (openGO) openGO.SetActive(true);
    }

    public void Close()
    {
        _isOpen = false;
        if (blockingCollider) blockingCollider.enabled = true;
        if (closedGO) closedGO.SetActive(true);
        if (openGO) closedGO.SetActive(true);
        if (openGO) openGO.SetActive(false);
    }

    void Refresh()
    {
        // Met l'état visuel cohérent dans l’éditeur
        if (_isOpen)
        {
            if (blockingCollider) blockingCollider.enabled = false;
            if (closedGO) closedGO.SetActive(false);
            if (openGO) openGO.SetActive(true);
        }
        else
        {
            if (blockingCollider) blockingCollider.enabled = true;
            if (closedGO) closedGO.SetActive(true);
            if (openGO) openGO.SetActive(false);
        }
    }
}
