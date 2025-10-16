using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly HashSet<ItemType> items = new();

    public bool Has(ItemType t) => items.Contains(t);

    public void Add(ItemType t)
    {
        if (items.Add(t))
            Debug.Log($"[Inventory] Picked up: {t}");
    }

    public void ClearAll() => items.Clear();
}
