using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    readonly HashSet<string> _flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public event Action<string> OnFlagChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static bool IsSet(string name) => Instance != null && Instance._flags.Contains(name);

    public static void Set(string name)
    {
        if (Instance == null) return;
        if (Instance._flags.Add(name))
        {
            Instance.OnFlagChanged?.Invoke(name);
            Debug.Log($"[Flag] +{name}");
        }
    }

    public static void Clear(string name)
    {
        if (Instance == null) return;
        if (Instance._flags.Remove(name))
        {
            Instance.OnFlagChanged?.Invoke(name);
            Debug.Log($"[Flag] -{name}");
        }
    }

    public static void ClearAll()
    {
        if (Instance == null) return;
        Instance._flags.Clear();
        Instance.OnFlagChanged?.Invoke("*");
        Debug.Log("[Flag] cleared all");
    }
}
