using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class UnlockRope : MonoBehaviour
{
    public DockArea2D dock;
    void Awake() { TryAutoWire(); }
    void Reset() { TryAutoWire(); }

    void TryAutoWire()
    {
        if (dock != null) return;
        dock = GetComponent<DockArea2D>();
        if (dock != null) return;
        dock = GetComponentInParent<DockArea2D>(true);
        if (dock != null) return;
        var all = Resources.FindObjectsOfTypeAll<DockArea2D>();
        dock = all.FirstOrDefault();
    }

    public void TryCut()
    {
        if (dock == null) { Debug.LogWarning("[UnlockRope] Aucune DockArea2D trouvée.", this); return; }
        if (dock.CanCutNow()) dock.ForceCut();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dock == null) return;
        if (other.GetComponent<PushableKinematic2D>() == null) return;
        TryCut();
    }
}
