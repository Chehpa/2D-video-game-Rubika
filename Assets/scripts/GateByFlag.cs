using UnityEngine;

public class GateByFlag : MonoBehaviour
{
    public FlagId requiredFlag;
    public GameObject[] enableIfTrue;
    public GameObject[] disableIfTrue;

    void OnEnable()
    {
        if (GameStateHost.I == null) return;
        GameStateHost.I.OnFlagSet += OnFlag;
        Apply();
    }
    void OnDisable()
    {
        if (GameStateHost.I != null) GameStateHost.I.OnFlagSet -= OnFlag;
    }
    void OnFlag(FlagId f) { if (f == requiredFlag) Apply(); }
    void Apply()
    {
        bool ok = GameStateHost.I.HasFlag(requiredFlag);
        foreach (var go in enableIfTrue) if (go) go.SetActive(ok);
        foreach (var go in disableIfTrue) if (go) go.SetActive(!ok);
    }
}
