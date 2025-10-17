using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask interactableLayer;   // coche "Interactable"
    public Transform interactorOrigin;    // glisse "InteractOrigin"
    public float radius = 0.6f;
    public float forwardOffset = 0.6f;    // distance devant le perso

    private PlayerInventory inv;
    private Rigidbody2D rb;
    private TopDownController2_5D topDown;

    void Awake()
    {
        inv = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
        topDown = GetComponent<TopDownController2_5D>();
    }

    void Update()
    {
        // Place automatiquement l'origin devant le perso
        if (interactorOrigin)
        {
            Vector2 facing = Vector2.right;
            if (topDown != null && topDown.Facing.sqrMagnitude > 0.001f)
                facing = topDown.Facing;
            else if (rb != null && rb.linearVelocity.sqrMagnitude > 0.001f)
                facing = rb.linearVelocity.normalized;

            interactorOrigin.localPosition = (Vector3)(facing.normalized * forwardOffset);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            var target = FindNearest();
            if (target != null)
            {
                Debug.Log($"[Interact] {target.Prompt}");
                target.Interact(inv);
            }
        }
    }

    private IInteractable FindNearest()
    {
        if (!interactorOrigin) return null;

        var hit = Physics2D.OverlapCircle(
            interactorOrigin.position, radius, interactableLayer);

        if (!hit) return null;

        if (hit.TryGetComponent<IInteractable>(out var inter)) return inter;

        var mono = hit.GetComponent<MonoBehaviour>();
        return mono as IInteractable;
    }

    void OnDrawGizmosSelected()
    {
        if (!interactorOrigin) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactorOrigin.position, radius);
    }
}
