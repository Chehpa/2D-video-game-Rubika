using System.Collections;
using UnityEngine;

// Put this on any pickup / object that should appear or not depending on inventory.
public class SpawnByInventory : MonoBehaviour
{
    public enum Mode
    {
        HideIfOwned,     // Hide the object if the player already owns the item (default)
        ShowOnlyIfOwned  // Show the object only if the player already owns the item
    }

    [Header("Rule")]
    public ItemType item;           // e.g. HairPin
    public Mode mode = Mode.HideIfOwned;

    [Tooltip("If null, this GameObject is toggled. Otherwise only this target is toggled.")]
    public GameObject target;

    [Tooltip("Tiny delay so the Player/Inventory exists after scene load.")]
    public float startDelay = 0.01f;

    private void Reset()
    {
        if (target == null) target = gameObject;
    }

    private IEnumerator Start()
    {
        if (target == null) target = gameObject;

        // Wait one frame (or a tiny delay) to let the persistent Player be present.
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);
        else yield return null;

        var inv = FindInventory();
        if (inv == null)
        {
            Debug.LogWarning("[SpawnByInventory] No PlayerInventory found.");
            yield break;
        }

        bool has = inv.Has(item);
        bool show = (mode == Mode.HideIfOwned) ? !has : has;
        target.SetActive(show);
    }

    private PlayerInventory FindInventory()
    {
        PlayerInventory inv = null;
#if UNITY_2023_1_OR_NEWER || UNITY_2022_2_OR_NEWER
        inv = Object.FindFirstObjectByType<PlayerInventory>();
#else
            inv = Object.FindObjectOfType<PlayerInventory>();
#endif
        return inv;
    }
}
