using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Active/d�sactive des objets quand un flag est (ou n'est pas) pos�
/// dans GameState. Rafra�chit au Start et quand un flag change.
/// </summary>
public class GateByFlag : MonoBehaviour
{
    [Header("Condition")]
    [Tooltip("Nom du flag dans GameState")]
    public string requiredFlag = "F_HandsFree";

    [Tooltip("Si vrai: d�verrouill� quand le flag EST pos�. Si faux: d�verrouill� quand le flag N'EST PAS pos�.")]
    public bool requireFlagSet = true;

    [Header("Effets � l'ouverture")]
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
