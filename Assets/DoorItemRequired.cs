using UnityEngine;

public class DoorItemRequired : MonoBehaviour
{
    [Header("Item requis")]
    public string requiredItemId = "key";

    [Header("Références")]
    public GameObject doorVisual;   // ce qui bloque (sprite + collider)

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null)
        {
            Debug.LogWarning("[Door] pas de PlayerInventory sur le joueur.");
            return;
        }

        // ici on utilise TON API
        if (inv.Has(requiredItemId))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("[Door] joueur n'a pas: " + requiredItemId);
        }
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

        Debug.Log("[Door] porte ouverte ✅");
    }
}
