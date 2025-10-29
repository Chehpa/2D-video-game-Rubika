using UnityEngine;

/// Chaise poussable en 2D (cinématique).
/// - Ne bouge jamais seule (pas de gravité).
/// - Se déplace seulement quand le joueur est en contact + input.
/// - Mouvement contraint le long d’un AXE (par défaut l’axe X).
/// - Limites min/max autour de la position de départ.
/// - Option: exiger un flag (ex: "F_HandsFree") pour autoriser la poussée.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PushableKinematic2D : MonoBehaviour
{
    [Header("Axe de déplacement")]
    [Tooltip("Axe de mouvement. (1,0)=X seulement, (0,1)=Y seulement, ou oblique (ex: 1,0.2).")]
    public Vector2 axis = Vector2.right;

    [Tooltip("Demi-longueur du rail autour de la position de départ (ex: 3 => -3..+3).")]
    public float halfRange = 3f;

    [Header("Poussée")]
    [Tooltip("Vitesse de glisse pendant la poussée")]
    public float speed = 2.5f;

    [Tooltip("Tag du joueur")]
    public string playerTag = "Player";

    [Tooltip("Seuil sous lequel on ignore l’input (-1..1)")]
    public float inputDeadZone = 0.1f;

    [Header("Pré-requis (optionnel)")]
    [Tooltip("Laisser vide pour toujours autoriser. Sinon exiger ce flag (ex: F_HandsFree).")]
    public string requiredFlag = "";

    Rigidbody2D _rb;
    Collider2D _col;
    Transform _player;
    bool _touching;
    Vector2 _start;
    Vector2 _axisN;

    void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var c = GetComponent<Collider2D>();
        c.isTrigger = false; // obstacle réel
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;

        _start = _rb.position;
        _axisN = (axis.sqrMagnitude < 1e-6f) ? Vector2.right : axis.normalized;
        if (halfRange < 0f) halfRange = Mathf.Abs(halfRange);
    }

    bool HasFlag()
    {
        if (string.IsNullOrEmpty(requiredFlag)) return true;
        return GameState.IsSet(requiredFlag); // on s’appuie sur ton GameState
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.CompareTag(playerTag))
        {
            _touching = true;
            _player = c.collider.transform;
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.collider.CompareTag(playerTag))
        {
            _touching = false;
            _player = null;
        }
    }

    void FixedUpdate()
    {
        if (!_touching || _player == null) return;
        if (!HasFlag()) return; // mains pas libres -> pas de poussée

        float h = Input.GetAxisRaw("Horizontal"); // ←/→ ou A/D
        if (Mathf.Abs(h) < inputDeadZone) return;

        // Pousser seulement (pas tirer) : si le joueur est "à gauche" le long de l’axe,
        // on accepte l’input vers +axis; s’il est "à droite", on accepte vers -axis.
        Vector2 toObj = _rb.position - (Vector2)_player.position;
        bool playerIsLeftOnAxis = Vector2.Dot(toObj, _axisN) > 0f;
        float dir = 0f;
        if (playerIsLeftOnAxis && h > 0f) dir = +1f;
        if (!playerIsLeftOnAxis && h < 0f) dir = -1f;
        if (dir == 0f) return;

        // Coordonnée projetée sur l’axe
        Vector2 rel = _rb.position - _start;
        float t = Vector2.Dot(rel, _axisN);

        // Avance et clamp
        t += dir * speed * Time.fixedDeltaTime;
        t = Mathf.Clamp(t, -halfRange, +halfRange);

        Vector2 target = _start + _axisN * t;
        _rb.MovePosition(target);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector2 start = Application.isPlaying ? _start : (Vector2)transform.position;
        Vector2 n = (axis.sqrMagnitude < 1e-6f) ? Vector2.right : axis.normalized;
        Vector3 A = start - n * halfRange;
        Vector3 B = start + n * halfRange;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(A, B);
    }
#endif
}
