using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly HashSet<string> _items = new HashSet<string>();

    public bool HasItem(string id) => !string.IsNullOrEmpty(id) && _items.Contains(id);
    public void AddItem(string id) { if (!string.IsNullOrEmpty(id) && _items.Add(id)) Debug.Log($"[Inv] +{id}"); }
    public void RemoveItem(string id) { if (!string.IsNullOrEmpty(id) && _items.Remove(id)) Debug.Log($"[Inv] -{id}"); }

    // alias compat
    public bool Has(string id) => HasItem(id);
    public void Add(string id) => AddItem(id);
    public void Remove(string id) => RemoveItem(id);
}
