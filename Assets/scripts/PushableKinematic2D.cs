using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PushableKinematic2D : MonoBehaviour
{
    [Header("Réglages")]
    public float pushSpeed = 2.5f;
    public LayerMask blockMask; // Ground/World

    Vector2 pendingMove;

    public void Push(Vector2 dir)
    {
        pendingMove += dir * pushSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (pendingMove == Vector2.zero) return;

        // Move "sec": pas de RB2D, gravité = 0 global. On simule.
        Vector3 target = transform.position + (Vector3)pendingMove;

        // Option: raycast court vers target pour éviter de traverser un mur
        if (!Physics2D.OverlapBox(target, GetSize(), 0f, blockMask))
        {
            transform.position = target;
        }

        pendingMove = Vector2.zero;
    }

    Vector2 GetSize()
    {
        var col = GetComponent<Collider2D>();
        return col.bounds.size;
    }
}
