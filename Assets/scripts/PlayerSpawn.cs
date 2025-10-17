using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si un portail a demandé un spawn précis, on l'utilise.
        var spawnId = SceneLoader.PendingSpawnId;
        if (string.IsNullOrEmpty(spawnId)) return;

        var all = FindObjectsOfType<SpawnPoint>();
        var target = all.FirstOrDefault(s => s.id == spawnId);
        if (target != null)
        {
            transform.position = target.transform.position;
        }
    }
}
