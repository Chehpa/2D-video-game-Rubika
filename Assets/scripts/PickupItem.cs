// PickupItem.cs
using UnityEngine;
public class PickupItem : MonoBehaviour
{
    public ItemId item;
    public GameObject[] disableOnPickup;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameStateHost.I.AddItem(item);
        foreach (var go in disableOnPickup) if (go) go.SetActive(false);
        Destroy(gameObject);
    }
}
