using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask interactableLayer;   // coche "Interactable" dans l’Inspector
    public Transform interactorOrigin;    // glisse "InteractOrigin" ici
    public float radius = 0.6f;

    private PlayerInventory inv;

    void Awake()
    {
        inv = GetComponent<PlayerInventory>();
    }

    void Update()
    {
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
        var hit = Physics2D.OverlapCircle(interactorOrigin.position, radius, interactableLayer);
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
