// UnlockColliderByInventory.cs
using UnityEngine;
public class UnlockColliderByInventory : MonoBehaviour
{
    public ItemId requiredItem;
    public FlagId setFlagOnUnlock = 0;
    public GameObject[] enableOnUnlock;
    public GameObject[] disableOnUnlock;

    bool unlocked;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (unlocked) return;
        if (!other.CompareTag("Player")) return;
        if (!GameStateHost.I.HasItem(requiredItem)) return;

        unlocked = true;
        if (setFlagOnUnlock != 0) GameStateHost.I.SetFlag(setFlagOnUnlock);
        foreach (var go in enableOnUnlock) if (go) go.SetActive(true);
        foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
    }
}
