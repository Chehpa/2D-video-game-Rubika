using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
        if (pr != null)
        {
            pr.Respawn();
        }
        else
        {
            // fallback: si pas de respawn sur le joueur
            other.transform.position = Vector3.zero;
        }
    }
}
