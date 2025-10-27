using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Coche la couche Interactable ici")]
    public LayerMask interactableLayer;

    [Tooltip("Un enfant du Player, plac� automatiquement si manquant")]
    public Transform interactorOrigin;

    [Tooltip("Rayon de d�tection autour de l�origin")]
    public float radius = 0.8f;

    [Tooltip("D�calage de l�origin vers l�avant du perso")]
    public float forwardOffset = 0.6f;

    [Header("Input")]
    public KeyCode key = KeyCode.E;

    private PlayerInventory inv;
    private Rigidbody2D rb;
    private TopDownController2_5D topDown;

    void Reset()
    {
        // Cr�e automatiquement un enfant "InteractOrigin" si absent
        if (!interactorOrigin)
        {
            var go = new GameObject("InteractOrigin");
            go.transform.SetParent(transform, false);
            interactorOrigin = go.transform;
        }
    }

    void Awake()
    {
        inv = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
        topDown = GetComponent<TopDownController2_5D>();

        // S�curit� si tu as ajout� le script sans passer par Reset()
        if (!interactorOrigin)
        {
            var existing = transform.Find("InteractOrigin");
            if (existing) interactorOrigin = existing;
            else
            {
                var go = new GameObject("InteractOrigin");
                go.transform.SetParent(transform, false);
                interactorOrigin = go.transform;
            }
        }
    }

    void Update()
    {
        // 1) Met � jour l�offset de l�origin devant le joueur
        if (interactorOrigin)
        {
            Vector2 facing = Vector2.right; // d�faut
            if (topDown != null && topDown.Facing.sqrMagnitude > 0.001f)
                facing = topDown.Facing;
            else if (rb != null && rb.linearVelocity.sqrMagnitude > 0.001f)
                facing = rb.linearVelocity.normalized;

            interactorOrigin.localPosition = (Vector3)(facing.normalized * forwardOffset);
        }

        // 2) Sur pression de E, cherche le meilleur interactable et interagit
        if (Input.GetKeyDown(key))
        {
            var target = FindBest();
            if (target != null)
            {
                Debug.Log($"[Interact] {target.Prompt}");
                target.Interact(inv);
            }
        }
    }

    private IInteractable FindBest()
    {
        if (!interactorOrigin) return null;

        // Cherche TOUS les colliders dans le rayon, puis prend le plus proche
        var hits = Physics2D.OverlapCircleAll(interactorOrigin.position, radius, interactableLayer);
        if (hits == null || hits.Length == 0) return null;

        IInteractable best = null;
        float bestDist = float.MaxValue;
        Vector2 origin = interactorOrigin.position;

        foreach (var h in hits)
        {
            if (!h) continue;

            // R�cup�re IInteractable via GetComponent (interface sur un MonoBehaviour)
            IInteractable inter = h.GetComponent<IInteractable>();
            if (inter == null)
            {
                var mb = h.GetComponent<MonoBehaviour>();
                inter = mb as IInteractable;
            }
            if (inter == null) continue;

            float d = (h.transform.position - (Vector3)origin).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = inter;
            }
        }

        return best;
    }

    void OnDrawGizmosSelected()
    {
        if (!interactorOrigin) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactorOrigin.position, radius);
    }
}
