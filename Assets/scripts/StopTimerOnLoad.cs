using UnityEngine;

public class StopTimerOnLoad : MonoBehaviour
{
    void Start()
    {
        TimerUI timer = FindObjectOfType<TimerUI>();
        if (timer != null)
        {
            timer.StopTimer();
        }
    }
}
