using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnablePhysicsOnFlag : MonoBehaviour
{
    public string requiredFlag = "F_HandsFree";
    Rigidbody2D _rb;

    void Awake() { _rb = GetComponent<Rigidbody2D>(); Apply(); GameState.Instance.OnFlagChanged += _ => Apply(); }
    void OnDestroy() { if (GameState.Instance != null) GameState.Instance.OnFlagChanged -= _ => Apply(); }

    void Apply()
    {
        bool ok = GameState.IsSet(requiredFlag);
        if (_rb != null) _rb.simulated = ok; // bloqué au début, activé quand le flag arrive
    }
}
