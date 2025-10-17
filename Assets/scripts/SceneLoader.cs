using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    // ID du SpawnPoint demandé par le portail
    public static string PendingSpawnId { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Charge la scène par nom et enregistre l'ID de spawn à utiliser dans la scène suivante.</summary>
    public void Load(string sceneName, string spawnIdInNextScene = null)
    {
        PendingSpawnId = spawnIdInNextScene;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>Recharge la scène courante (utile pour le "R").</summary>
    public void ReloadCurrent()
    {
        var current = SceneManager.GetActiveScene().name;
        // On garde le dernier spawn demandé s'il y en avait un, sinon pas de contrainte.
        SceneManager.LoadScene(current, LoadSceneMode.Single);
    }
}
