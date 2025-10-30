using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class DialogueNPC : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] sentences;

    [Header("UI")]
    public GameObject dialogueUI;           // le panel
    public TextMeshProUGUI dialogueText;    // le texte
    public GameObject interactHint;         // “E pour parler”

    private int index = 0;
    private bool playerInRange = false;
    private bool isTalking = false;

    private PlayerMovement2D playerMove;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
                StartDialogue();
            else
                NextSentence();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        index = 0;

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true);
            dialogueText.text = sentences.Length > 0 ? sentences[0] : "";
        }

        if (playerMove != null)
            playerMove.SetCanMove(false);

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void NextSentence()
    {
        index++;

        if (index >= sentences.Length)
            EndDialogue();
        else
            dialogueText.text = sentences[index];
    }

    void EndDialogue()
    {
        isTalking = false;

        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        if (playerMove != null)
            playerMove.SetCanMove(true);

        if (interactHint != null && playerInRange)
            interactHint.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerMove = other.GetComponent<PlayerMovement2D>();

        if (!isTalking && interactHint != null)
            interactHint.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (isTalking)
            EndDialogue();

        if (interactHint != null)
            interactHint.SetActive(false);
    }
}
