using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Timer")]
    public float startTime = 120f;
    public TextMeshProUGUI timerText;

    float currentTime;
    bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
        }

        // à chaque frame on sauve la valeur pour les autres scènes
        TimerStorage.lastTime = currentTime;

        UpdateText();
    }

    void UpdateText()
    {
        if (timerText == null) return;

        int m = Mathf.FloorToInt(currentTime / 60f);
        int s = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = m.ToString("00") + ":" + s.ToString("00");
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        UpdateText();
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
