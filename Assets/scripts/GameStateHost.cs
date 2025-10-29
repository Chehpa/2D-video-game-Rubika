// GameStateHost.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FlagId { F_HandsFree, F_RopeCut, F_DoorOpen, F_PowerOn, F_SafeOpen, F_HasHairPin, F_HasKey_Rusty }
public enum ItemId { None, GlassShard, HairPin, Key_Rusty }

public class GameStateHost : MonoBehaviour
{
    public static GameStateHost I { get; private set; }

    private readonly HashSet<FlagId> flags = new();
    private readonly HashSet<ItemId> items = new();

    public event Action<FlagId> OnFlagSet;
    public event Action<ItemId> OnItemAdded;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (_, __) => BroadcastAll();
    }

    public bool HasFlag(FlagId f) => flags.Contains(f);
    public void SetFlag(FlagId f)
    {
        if (flags.Add(f)) OnFlagSet?.Invoke(f);
    }

    public bool HasItem(ItemId i) => items.Contains(i);
    public void AddItem(ItemId i)
    {
        if (i == ItemId.None) return;
        if (items.Add(i)) OnItemAdded?.Invoke(i);
    }

    void BroadcastAll()
    {
        foreach (var f in flags) OnFlagSet?.Invoke(f);
        foreach (var i in items) OnItemAdded?.Invoke(i);
    }
}
