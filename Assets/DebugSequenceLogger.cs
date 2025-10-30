using UnityEngine;

public class DebugSequenceLogger : MonoBehaviour
{
    void OnEnable() { GameStateHost.I.OnFlagSet += OnFlag; }
    void OnDisable() { if (GameStateHost.I != null) GameStateHost.I.OnFlagSet -= OnFlag; }
    void OnFlag(FlagId f) { Debug.Log($"[SEQ] + {f}"); }
}
