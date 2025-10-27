using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [HideInInspector] public string pendingSpawnId = "";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void Load(string sceneName, string spawnId = "")
    {
        if (Instance != null) Instance.pendingSpawnId = spawnId;
        SceneManager.LoadScene(sceneName);
    }
}
