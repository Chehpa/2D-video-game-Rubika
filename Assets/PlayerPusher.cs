using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerPusher : MonoBehaviour
{
    public string horizontalAxis = "Horizontal";
    public float contactPushFactor = 1f;
    public bool requireHandsFree = true;

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true; // ce script vit sur un trigger
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Blindage contre GameStateHost absent
        if (requireHandsFree && (GameStateHost.I == null || !GameStateHost.I.HasFlag(FlagId.F_HandsFree)))
            return;

        var pushable = other.GetComponent<PushableKinematic2D>();
        if (pushable == null) return;

        float h = Input.GetAxisRaw(horizontalAxis);
        if (Mathf.Abs(h) < 0.1f) return;

        pushable.Push(new Vector2(h * contactPushFactor, 0f));
    }
}
