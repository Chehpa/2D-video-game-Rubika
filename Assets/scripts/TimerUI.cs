using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Wiring")]
    [Tooltip("R�f�rence au TextMeshProUGUI qui affiche le temps (ex: TimerText)")]
    public TextMeshProUGUI label;

    [Header("Timer")]
    [Tooltip("Valeur de d�part (en secondes)")]
    public int startSeconds = 120;
    [Tooltip("Si coch�: compte � rebours. Sinon: compte vers le haut.")]
    public bool countDown = true;
    [Tooltip("D�marrer automatiquement � l'entr�e en jeu")]
    public bool autoStart = true;

    [Header("Persistence & Sc�nes")]
    [Tooltip("Garder ce HUD entre les sc�nes (un seul exemplaire)")]
    public bool persistAcrossScenes = true;
    [Tooltip("Met automatiquement le timer en pause dans ces sc�nes")]
    public bool stopOnWinOrGameOver = true;
    public string winSceneName = "Win";
    public string gameOverSceneName = "GameOver";

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color timeUpColor = Color.red;

    // --- interne ---
    private static TimerUI Instance;          // pour �viter les doublons si persistant
    private float time;                        // temps courant en secondes
    private bool running;                      // le timer tourne ?

    void Awake()
    {
        // Singleton persistant (optionnel)
        if (persistAcrossScenes)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (!label) label = GetComponentInChildren<TextMeshProUGUI>(true);
        if (label) label.color = normalColor;

        ResetTimer(autoStart);

        // R�agit au changement de sc�ne (pause sur Win/GameOver)
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (!running) return;

        if (countDown)
            time -= Time.deltaTime;
        else
            time += Time.deltaTime;

        // Arriv� � 0 en mode compte � rebours
        if (countDown && time <= 0f)
        {
            time = 0f;
            running = false;
            UpdateLabel();

            // Charger GameOver si un nom est fourni
            if (!string.IsNullOrEmpty(gameOverSceneName))
                SceneManager.LoadScene(gameOverSceneName);

            return;
        }

        UpdateLabel();
    }

    private void UpdateLabel()
    {
        // Affichage mm:ss (arrondi sup�rieur pour un rebours plus naturel)
        int t = Mathf.Max(0, (int)Mathf.Ceil(time));
        int mm = t / 60;
        int ss = t % 60;

        if (label)
        {
            label.text = $"{mm:00}:{ss:00}";
            // Rouge quand on est � 0 en mode compte � rebours
            label.color = (countDown && t <= 0) ? timeUpColor : normalColor;
        }
    }

    private void OnSceneChanged(Scene from, Scene to)
    {
        if (!stopOnWinOrGameOver) return;

        var name = to.name;
        if (name == winSceneName || name == gameOverSceneName)
        {
            running = false;           // pause (il reste visible)
            UpdateLabel();
        }
    }

    // --- API publique pratique ---
    public void StartTimer() { running = true; }
    public void StopTimer() { running = false; }
    public void ResetTimer(bool autoRun = false)
    {
        time = Mathf.Max(0, startSeconds);
        running = autoRun;
        UpdateLabel();
    }

    // Pour ajuster dynamiquement si besoin
    public void SetSeconds(float seconds, bool autoRun)
    {
        time = Mathf.Max(0f, seconds);
        running = autoRun;
        UpdateLabel();
    }

    public float CurrentSeconds() => time;
}
