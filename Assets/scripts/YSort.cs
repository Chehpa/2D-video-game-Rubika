using UnityEngine;

/// <summary>
/// Trie le SpriteRenderer par la coordonnée Y (plus bas = au-dessus).
/// Pose-le sur tout sprite (murs, props, PNJ, caisses, etc.).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    public bool updateEveryFrame = false; // ON si l'objet bouge
    public int orderOffset = 0;           // ajuste si besoin
    public int multiplier = 100;          // 100 = résolution du tri

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Apply();
    }

    void LateUpdate()
    {
        if (updateEveryFrame) Apply();
    }

    void OnValidate()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        Apply();
    }

    private void Apply()
    {
        sr.sortingOrder = -(int)(transform.position.y * multiplier) + orderOffset;
    }
}
