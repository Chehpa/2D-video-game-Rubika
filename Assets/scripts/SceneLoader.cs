using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [HideInInspector] public string pendingSpawnId = "";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, string spawnId)
    {
        pendingSpawnId = spawnId ?? "";
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // Pratique: appel statique possible.
    public static void Load(string sceneName, string spawnId)
    {
        if (Instance == null)
        {
            var go = new GameObject("_Systems");
            Instance = go.AddComponent<SceneLoader>();
            DontDestroyOnLoad(go);
        }
        Instance.LoadScene(sceneName, spawnId);
    }
}
