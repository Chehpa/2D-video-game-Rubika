using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UnlockRope : MonoBehaviour
{
    [Header("Zones & conditions")]
    public Collider2D useZone;          // zone trigger au niveau de la corde (sur la chaise)
    public string playerTag = "Player";
    public DockArea2D chairDock;        // DockArea2D pr�s du pendu (chaise � la bonne place)
    public string requiredFlag = "F_HandsFree"; // mains libres n�cessaires

    [Header("Effets")]
    public GameObject[] disableOnUnlock;  // sprite+colliders de la corde
    public GameObject[] enableOnUnlock;   // sprite corde coup�e (optionnel)
    public string flagToSet = "F_RopeCut";

    Collider2D _selfCol;

    void Reset() { _selfCol = GetComponent<Collider2D>(); if (_selfCol) _selfCol.isTrigger = true; }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // 1) mains libres ?
        if (!GameState.IsSet(requiredFlag)) return;

        // 2) chaise dock�e ?
        if (chairDock != null && !chairDock.IsDocked) return;

        // 3) joueur debout sur la chaise (dans la zone haute) ?
        if (useZone != null && !useZone.IsTouching(other)) return;

        DoUnlock();
    }

    void DoUnlock()
    {
        if (disableOnUnlock != null) foreach (var go in disableOnUnlock) if (go) go.SetActive(false);
        if (enableOnUnlock != null) foreach (var go in enableOnUnlock) if (go) go.SetActive(true);

        if (!string.IsNullOrEmpty(flagToSet)) GameState.Set(flagToSet);

        if (_selfCol != null) _selfCol.enabled = false; // �viter les doubles d�clenchements
        Debug.Log("[Rope] Corde coup�e.");
    }
}
