using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorSwap : MonoBehaviour, IInteractable
{
    [Header("Lock")]
    public bool requireHairPin = true;   // Exiger la HairPin pour OUVRIR
    public bool allowToggle = false;     // Autoriser la FERMETURE (E à nouveau)

    [Header("References")]
    [SerializeField] private GameObject closedGO;          // enfant "Closed"
    [SerializeField] private GameObject openGO;            // enfant "Open"
    [SerializeField] private Collider2D blockingCollider;  // collider de "Closed"
    [SerializeField] private SpriteRenderer parentSR;      // SR du parent (si existe)

    private bool isOpen;

    public string Prompt
        => isOpen
           ? (allowToggle ? "Close [E]" : "")
           : (requireHairPin ? "Pick the lock [E]" : "Open [E]");

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        var tClosed = transform.Find("Closed");
        var tOpen = transform.Find("Open");
        if (!closedGO && tClosed) closedGO = tClosed.gameObject;
        if (!openGO && tOpen) openGO = tOpen.gameObject;

        if (!blockingCollider && closedGO)
            blockingCollider = closedGO.GetComponent<Collider2D>();

        if (!parentSR)
            parentSR = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (parentSR) parentSR.enabled = false; // masque le SR du parent si jamais
        SetState(false);                         // démarre fermée
    }

    public void Interact(PlayerInventory inv)
    {
        if (!isOpen)
        {
            bool canOpen = !requireHairPin || (inv != null && inv.Has(ItemType.HairPin));
            if (canOpen)
            {
                SetState(true);
                Debug.Log("[Door] Opened.");
            }
            else
            {
                Debug.Log("[Door] It's locked… need a hair pin.");
            }
        }
        else if (allowToggle)
        {
            SetState(false);
            Debug.Log("[Door] Closed.");
        }
    }

    private void SetState(bool open)
    {
        isOpen = open;

        if (closedGO) closedGO.SetActive(!open);
        if (openGO)
        {
            openGO.SetActive(open);

            // Sécurise l’affichage d’Open
            var srOpen = openGO.GetComponent<SpriteRenderer>();
            if (srOpen)
            {
                srOpen.enabled = open;
                var c = srOpen.color; c.a = 1f; srOpen.color = c;

                // Copie le tri de Closed → Open (si présent)
                if (closedGO)
                {
                    var srClosed = closedGO.GetComponent<SpriteRenderer>();
                    if (srClosed)
                    {
                        srOpen.sortingLayerID = srClosed.sortingLayerID;
                        srOpen.sortingOrder = srClosed.sortingOrder;
                    }
                    openGO.transform.position = closedGO.transform.position;
                }
            }
        }

        if (blockingCollider) blockingCollider.enabled = !open; // bloque seulement fermée
    }

    [ContextMenu("Force Open")] private void ForceOpen() => SetState(true);
    [ContextMenu("Force Close")] private void ForceClose() => SetState(false);
}
