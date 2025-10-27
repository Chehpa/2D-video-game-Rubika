using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    // --- Singleton persistant ---
    public static GameTimer Instance;
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("Config")]
    public string firstLevelName = "Room1";     // 1�re sc�ne de jeu
    public string winSceneName = "Win";
    public string gameOverSceneName = "GameOver";
    public float initialSeconds = 120f;        // dur�e d�une run

    [Header("State (read-only)")]
    [SerializeField] private float remaining;   // secondes restantes
    [SerializeField] private bool isRunning;

    // Pour demander un reset propre depuis le Menu
    private static bool pendingResetOnNextFirstLevel = false;

    public float Remaining => Mathf.Max(0f, remaining);
    public bool IsRunning => isRunning;

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Update()
    {
        if (!isRunning) return;

        remaining -= Time.deltaTime; // (prend le Time Scale normal)
        if (remaining <= 0f)
        {
            remaining = 0f;
            isRunning = false;
            // si on n�est pas d�j� en GO/Win -> aller � GameOver
            var s = SceneManager.GetActiveScene().name;
            if (s != gameOverSceneName && s != winSceneName)
                SceneManager.LoadScene(gameOverSceneName);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        string n = scene.name;

        // Stopper sur Win / GameOver (affich� mais fig�)
        if (n == winSceneName || n == gameOverSceneName)
        {
            isRunning = false;
            return;
        }

        // D�marrer une nouvelle "run" quand on (re)vient du menu sur la 1�re sc�ne
        if (n == firstLevelName && pendingResetOnNextFirstLevel)
        {
            RestartRun();
            pendingResetOnNextFirstLevel = false;
        }
        // 1er lancement dans l��diteur (play direct sur Room1)
        else if (n == firstLevelName && remaining <= 0f && !isRunning)
        {
            RestartRun();
        }
    }

    // Red�marrer le timer pour une nouvelle run
    public void RestartRun()
    {
        remaining = Mathf.Max(1f, initialSeconds);
        isRunning = true;
    }

    // Bouton Menu va appeler ceci avant de charger Room1
    public static void PrepareNewRun()
    {
        pendingResetOnNextFirstLevel = true;
    }
}
