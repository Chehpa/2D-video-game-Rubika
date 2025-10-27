using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class UnlockColliderByInventory : MonoBehaviour
{
    [Tooltip("Item n�cessaire pour activer l'interaction (ex: GlassShard pour la corde)")]
    public ItemType requiredItem;

    private PlayerInventory inv;
    private Collider2D col;

    private IEnumerator Start()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;   // on reste en trigger pour l'interaction
        col.enabled = false;  // verrouill� au d�but

        // Attendre que le Player + Inventory existent (apr�s un load)
        while ((inv = UnityEngine.Object.FindFirstObjectByType<PlayerInventory>()) == null)
            yield return null;

        // Boucle: activer/d�sactiver le collider selon l'inventaire
        while (true)
        {
            bool has = inv.Has(requiredItem);
            col.enabled = has;   // devient interactable d�s que le joueur a l'item
            yield return null;   // on v�rifie chaque frame (simple et suffisant ici)
        }
    }
}
