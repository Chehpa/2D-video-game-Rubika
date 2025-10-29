using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DockArea2D : MonoBehaviour
{
    public string requiredTag = "Chair";
    public bool IsDocked { get; private set; }

    void Reset() { var c = GetComponent<Collider2D>(); if (c) c.isTrigger = true; }

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag(requiredTag)) IsDocked = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag(requiredTag)) IsDocked = false; }
}
