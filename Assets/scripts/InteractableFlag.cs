using UnityEngine;

public class InteractableFlag : MonoBehaviour, IInteractable
{
    [SerializeField] string flagName = "SomeFlag";
    [SerializeField] string prompt = "Interact (E)";
    [SerializeField] bool destroyAfter = false;   // optionnel

    public string Prompt => prompt;

    public void Interact(PlayerInventory inv)
    {
        GameState.Set(flagName);
        if (destroyAfter) Destroy(gameObject);
        enabled = false; // évite de “reposer” le flag
    }
}
