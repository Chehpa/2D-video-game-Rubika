using UnityEngine;

/// Affiche/masque un objet selon l'inventaire du joueur (IDs string).
/// Place ce script sur l'objet à activer/désactiver (ou renseigne Target).
public class SpawnByInventory : MonoBehaviour
{
    public enum RuleMode
    {
        HideIfHas,      // cache si le joueur POSSEDE l'item
        HideIfNotHas,   // cache si le joueur NE POSSEDE PAS l'item
        ShowIfHas,      // montre seulement si le joueur a l'item
        ShowIfNotHas    // montre seulement si le joueur n'a PAS l'item
    }

    [Header("Condition")]
    [Tooltip("ID d'inventaire (ex: Glass, HairPin, Rope, …)")]
    public string itemId = "HairPin";
    public RuleMode mode = RuleMode.HideIfHas;

    [Header("Cible (optionnel)")]
    [Tooltip("Si vide, c'est ce GameObject qui est activé/désactivé")]
    public GameObject target;

    [Header("Timing")]
    [Tooltip("Petit délai pour laisser l'inventaire s'initialiser")]
    public float startDelay = 0.01f;

    void Awake()
    {
        if (target == null) target = gameObject;
    }

    void OnEnable()
    {
        if (startDelay > 0f) Invoke(nameof(Refresh), startDelay);
        else Refresh();
    }

    void OnValidate()
    {
        if (target == null) target = gameObject;
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
            Apply(HasItemInScene());
#endif
    }

    void Refresh() => Apply(HasItemInScene());

    bool HasItemInScene()
    {
        PlayerInventory inv = null;

#if UNITY_2023_1_OR_NEWER
        inv = Object.FindFirstObjectByType<PlayerInventory>(FindObjectsInactive.Include);
#else
        inv = Object.FindObjectOfType<PlayerInventory>();
#endif
        if (inv == null) return false;
        return inv.Has(itemId);
    }

    void Apply(bool has)
    {
        if (target == null) return;

        bool show = true;
        switch (mode)
        {
            case RuleMode.HideIfHas: show = !has; break;
            case RuleMode.HideIfNotHas: show = has; break;
            case RuleMode.ShowIfHas: show = has; break;
            case RuleMode.ShowIfNotHas: show = !has; break;
        }
        target.SetActive(show);
    }

    // Helpers compat (anciens noms éventuels)
    public string Item => itemId;
    public void SetItem(string id) { itemId = id; Refresh(); }
}
