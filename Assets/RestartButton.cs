using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public Transform player;
    public Vector3 restartPosition;
    public TimerUI timer;

    public void Restart()
    {
        if (player != null)
        {
            player.position = restartPosition;

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        if (timer != null)
        {
            timer.ResetTimer();
        }
    }
}
