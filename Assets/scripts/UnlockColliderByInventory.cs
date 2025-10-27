using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class UnlockColliderByInventory : MonoBehaviour
{
    [Tooltip("Item nécessaire pour activer l'interaction (ex: GlassShard pour la corde)")]
    public ItemType requiredItem;

    private PlayerInventory inv;
    private Collider2D col;

    private IEnumerator Start()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;   // on reste en trigger pour l'interaction
        col.enabled = false;  // verrouillé au début

        // Attendre que le Player + Inventory existent (après un load)
        while ((inv = UnityEngine.Object.FindFirstObjectByType<PlayerInventory>()) == null)
            yield return null;

        // Boucle: activer/désactiver le collider selon l'inventaire
        while (true)
        {
            bool has = inv.Has(requiredItem);
            col.enabled = has;   // devient interactable dès que le joueur a l'item
            yield return null;   // on vérifie chaque frame (simple et suffisant ici)
        }
    }
}
