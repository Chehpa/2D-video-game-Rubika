using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        // spawn de d�part = position actuelle
        spawnPoint = transform.position;
    }

    public void SetCheckpoint(Vector3 newPoint)
    {
        spawnPoint = newPoint;
        Debug.Log("[Respawn] Nouveau checkpoint: " + newPoint);
    }

    public void Respawn()
    {
        transform.position = spawnPoint;

        // on coupe la vitesse si jamais il tombait
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
