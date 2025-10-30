using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Déplacement")]
    public float speed = 3f;
    public Transform[] waypoints; // Points A et B
    private int currentPoint = 0;

    [Header("Player Kill")]
    public string playerTag = "Player";
    public string gameOverScene = "GameOver";

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // arrivé au point ?
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
                currentPoint = 0;

            // flip horizontal
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // ici soit on reload la scène, soit on envoie sur GameOver
            SceneManager.LoadScene(gameOverScene);
        }
    }
}
