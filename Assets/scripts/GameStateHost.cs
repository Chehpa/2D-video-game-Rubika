using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FlagId
{
    // Room1
    F_HandsFree, F_RopeCut, F_DoorOpen,

    // Room2
    F_R2_Word1, F_R2_Word2, F_R2_Word3,
    F_R2_AllDigits, F_R2_DoorOpen,

    // Room3 (si tu enchaines le projet)
    F_R3_Candle1, F_R3_Candle2, F_R3_Candle3,
    F_R3_AllCandles, F_R3_GateOpen,

    // Optionnels / communs
    F_PowerOn, F_SafeOpen,
    F_HasHairPin, F_HasKey_Rusty
}

public enum ItemId { None, GlassShard, HairPin, Key_Rusty, Soul_Red, Soul_Green, Soul_Blue }

public class GameStateHost : MonoBehaviour
{
    public static GameStateHost I { get; private set; }
    readonly HashSet<FlagId> flags = new();
    readonly HashSet<ItemId> items = new();

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
    public void SetFlag(FlagId f) { if (flags.Add(f)) OnFlagSet?.Invoke(f); }

    public bool HasItem(ItemId i) => items.Contains(i);
    public void AddItem(ItemId i) { if (i != ItemId.None && items.Add(i)) OnItemAdded?.Invoke(i); }
    public void RemoveItem(ItemId i) { if (i != ItemId.None) items.Remove(i); }

    void BroadcastAll()
    {
        foreach (var f in flags) OnFlagSet?.Invoke(f);
        foreach (var i in items) OnItemAdded?.Invoke(i);
    }
}
