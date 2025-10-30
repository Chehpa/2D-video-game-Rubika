using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;  // optionnel
    public bool destroyOnTouch = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
        if (pr != null)
        {
            Vector3 point = respawnPoint != null ? respawnPoint.position : transform.position;
            pr.SetCheckpoint(point);
        }

        if (destroyOnTouch)
        {
            gameObject.SetActive(false);
        }
    }
}
