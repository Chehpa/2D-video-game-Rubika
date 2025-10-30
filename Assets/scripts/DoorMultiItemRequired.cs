using UnityEngine;

public class DoorMultiItemRequired : MonoBehaviour
{
    [Header("Items requis (tous)")]
    public string[] requiredItems;       // ex: key_red, key_blue, key_green

    [Header("Références")]
    public GameObject doorVisual;        // ce qui bloque (sprite+collider)

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null)
        {
            Debug.LogWarning("[Door] Joueur sans PlayerInventory.");
            return;
        }

        // vérifier toutes les clés
        if (HasAllRequired(inv))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("[Door] manque au moins 1 clé.");
        }
    }

    private bool HasAllRequired(PlayerInventory inv)
    {
        if (requiredItems == null || requiredItems.Length == 0)
            return true; // si t'as rien mis, on ouvre

        for (int i = 0; i < requiredItems.Length; i++)
        {
            string key = requiredItems[i];
            if (string.IsNullOrEmpty(key)) continue;

            // on utilise TON inventaire et son alias
            if (!inv.Has(key))
            {
                return false;
            }
        }

        return true;
    }

    private void OpenDoor()
    {
        isOpen = true;

        if (doorVisual != null)
        {
            doorVisual.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("[Door] porte ouverte ✅ (toutes les clés ok)");
    }
}
