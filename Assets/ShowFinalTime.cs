using UnityEngine;
using TMPro;

public class ShowFinalTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        if (timeText == null) return;

        float t = TimerStorage.lastTime;
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        timeText.text = "Time: " + m.ToString("00") + ":" + s.ToString("00");
    }
}
