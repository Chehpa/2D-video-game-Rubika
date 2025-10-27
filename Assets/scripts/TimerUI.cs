using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Wiring")]
    [Tooltip("Référence au TextMeshProUGUI qui affiche le temps (ex: TimerText)")]
    public TextMeshProUGUI label;

    [Header("Timer")]
    [Tooltip("Valeur de départ (en secondes)")]
    public int startSeconds = 120;
    [Tooltip("Si coché: compte à rebours. Sinon: compte vers le haut.")]
    public bool countDown = true;
    [Tooltip("Démarrer automatiquement à l'entrée en jeu")]
    public bool autoStart = true;

    [Header("Persistence & Scènes")]
    [Tooltip("Garder ce HUD entre les scènes (un seul exemplaire)")]
    public bool persistAcrossScenes = true;
    [Tooltip("Met automatiquement le timer en pause dans ces scènes")]
    public bool stopOnWinOrGameOver = true;
    public string winSceneName = "Win";
    public string gameOverSceneName = "GameOver";

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color timeUpColor = Color.red;

    // --- interne ---
    private static TimerUI Instance;          // pour éviter les doublons si persistant
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

        // Réagit au changement de scène (pause sur Win/GameOver)
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

        // Arrivé à 0 en mode compte à rebours
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
        // Affichage mm:ss (arrondi supérieur pour un rebours plus naturel)
        int t = Mathf.Max(0, (int)Mathf.Ceil(time));
        int mm = t / 60;
        int ss = t % 60;

        if (label)
        {
            label.text = $"{mm:00}:{ss:00}";
            // Rouge quand on est à 0 en mode compte à rebours
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
