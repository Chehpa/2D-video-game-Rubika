using UnityEngine;

/// Zone de dock sous le pendu (compat UnlockRope).
/// Déclenche la coupe quand la chaise entre dans la zone.
[RequireComponent(typeof(Collider2D))]
public class DockArea2D : MonoBehaviour
{
    [Header("Références à toggler")]
    public GameObject ropeObject;     // objet corde à désactiver
    public GameObject hairPinPickup;  // épingle à activer

    [Header("Conditions")]
    public bool requireHandsFree = true; // mains libérées obligatoires

    bool done;

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }

    // ---------- API compat (appelée par UnlockRope) ----------
    public bool IsBlocked()
    {
        if (done) return true;
        if (requireHandsFree && !GameStateHost.I.HasFlag(FlagId.F_HandsFree)) return true;
        return false;
    }

    public bool CanCutNow() => !IsBlocked();

    public void ForceCut()
    {
        if (!done) CutRope();
    }

    // ---------- Logique locale ----------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (done) return;
        if (other.GetComponent<PushableKinematic2D>() == null) return; // on veut la chaise
        if (IsBlocked()) return;
        CutRope();
    }

    void CutRope()
    {
        done = true;

        GameStateHost.I.SetFlag(FlagId.F_RopeCut);

        if (ropeObject) ropeObject.SetActive(false);
        if (hairPinPickup) hairPinPickup.SetActive(true);
    }
}
