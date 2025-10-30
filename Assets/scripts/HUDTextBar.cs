using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDTextBar : MonoBehaviour
{
    public static HUDTextBar I;
    public CanvasGroup panel;
    public Text uiText;
    public float charInterval = 0.03f;
    public float autoHideDelay = 1.8f;
    public KeyCode skipKey = KeyCode.E;

    Coroutine running; bool skipping;

    void Awake()
    {
        I = this;
        if (panel) panel.alpha = 0f;
        if (uiText) uiText.text = "";
    }

    public void Show(string msg, float? hideDelay = null)
    {
        Run(TypeWriteRoutine(msg, hideDelay ?? autoHideDelay));
    }
    public void SetPersistent(string msg)
    {
        StopRunning();
        if (uiText) uiText.text = msg;
        Fade(1f);
    }
    public void Clear()
    {
        StopRunning();
        if (uiText) uiText.text = "";
        Fade(0f);
    }
    public void TypeWrite(string msg, float? hideDelay = null)
    {
        Run(TypeWriteRoutine(msg, hideDelay ?? autoHideDelay));
    }

    void Run(IEnumerator co) { StopRunning(); running = StartCoroutine(co); }
    void StopRunning() { if (running != null) { StopCoroutine(running); running = null; } skipping = false; }

    IEnumerator TypeWriteRoutine(string msg, float hideDelay)
    {
        Fade(1f); if (uiText) uiText.text = "";
        for (int i = 0; i < msg.Length; i++)
        {
            if (Input.GetKeyDown(skipKey)) { skipping = true; }
            if (skipping) { if (uiText) uiText.text = msg; break; }
            if (uiText) uiText.text += msg[i];
            yield return new WaitForSeconds(charInterval);
        }
        if (hideDelay > 0f)
        {
            float t = hideDelay;
            while (t > 0f) { if (Input.GetKeyDown(skipKey)) break; t -= Time.deltaTime; yield return null; }
            Fade(0f); if (uiText) uiText.text = "";
        }
        running = null; skipping = false;
    }

    void Fade(float a)
    {
        if (!panel) return;
        panel.alpha = a;
        panel.blocksRaycasts = a > 0.5f;
        panel.interactable = a > 0.5f;
    }
}
