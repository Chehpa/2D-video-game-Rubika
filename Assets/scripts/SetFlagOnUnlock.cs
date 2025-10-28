using UnityEngine;

public class SetFlagOnUnlock : MonoBehaviour
{
    [Tooltip("Nom du flag GameState à poser quand on appelle DoSet()")]
    public string flagName = "F_HandsFree";

    // Appelle cette méthode depuis un UnityEvent (ex: OnUnlock d'un autre composant)
    public void DoSet()
    {
        GameState.Set(flagName);
        Debug.Log($"[Flag] set by SetFlagOnUnlock: {flagName}");
    }
}
