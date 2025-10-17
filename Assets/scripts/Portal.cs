using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [Header("Destination")]
    public string destinationScene;   // ex: "Room2"
    public string destinationSpawnId; // ex: "FromRoom1"

    [Header("Option")]
    public bool requireHairPin = false;

    public string Prompt => requireHairPin ? "Open door (E)" : "Go (E)";

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (!col) col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact(PlayerInventory inv)
    {
        if (requireHairPin && !inv.Has(ItemType.HairPin))
        {
            Debug.Log("Locked. Need HairPin.");
            return;
        }
        if (string.IsNullOrEmpty(destinationScene))
        {
            Debug.LogWarning("Portal: destinationScene is empty.");
            return;
        }
        SceneLoader.Load(destinationScene, destinationSpawnId);
    }
}
