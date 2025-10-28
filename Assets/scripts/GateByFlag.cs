using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Active/désactive des objets quand un flag est (ou n'est pas) posé
/// dans GameState. Rafraîchit au Start et quand un flag change.
/// </summary>
public class GateByFlag : MonoBehaviour
{
    [Header("Condition")]
    [Tooltip("Nom du flag dans GameState")]
    public string requiredFlag = "F_HandsFree";

    [Tooltip("Si vrai: déverrouillé quand le flag EST posé. Si faux: déverrouillé quand le flag N'EST PAS posé.")]
    public bool requireFlagSet = true;

    [Header("Effets à l'ouverture")]
    public GameObject[] enableOnUnlock;
    public GameObject[] disableOnUnlock;
    public Collider2D[] disableCollidersOnUnlock;

    public UnityEvent onUnlock;

    private bool _isUnlocked;

    private void OnEnable()
    {
        if (GameState.Instance != null)
            GameState.Instance.OnFlagChanged += OnAnyFlagChanged;
    }

    private void OnDisable()
    {
        if (GameState.Instance != null)
            GameState.Instance.OnFlagChanged -= OnAnyFlagChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnAnyFlagChanged(string _)
    {
        Refresh();
    }

    private void Refresh()
    {
        bool hasFlag = GameState.IsSet(requiredFlag);
        bool shouldUnlock = requireFlagSet ? hasFlag : !hasFlag;

        if (_isUnlocked == shouldUnlock) return;
        _isUnlocked = shouldUnlock;

        if (_isUnlocked)
        {
            foreach (var go in enableOnUnlock) if (go) go.SetActive(true);
            foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
            foreach (var c in disableCollidersOnUnlock) if (c) c.enabled = false;
            onUnlock?.Invoke();
        }
    }
}
