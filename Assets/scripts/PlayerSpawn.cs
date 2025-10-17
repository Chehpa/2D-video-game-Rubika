using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(100)]
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

    private void Start()
    {
        PlaceAtSpawn();
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        PlaceAtSpawn();
    }

    private void PlaceAtSpawn()
    {
        string wanted = (SceneLoader.Instance != null) ? SceneLoader.Instance.pendingSpawnId : "";

        var points = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint target = null;

        if (!string.IsNullOrEmpty(wanted))
        {
            foreach (var p in points) { if (p.id == wanted) { target = p; break; } }
        }
        if (target == null)
        {
            foreach (var p in points) { if (p.isDefault) { target = p; break; } }
        }
        if (target == null && points.Length > 0) target = points[0];

        if (target != null)
        {
            transform.position = target.transform.position;
            if (SceneLoader.Instance != null) SceneLoader.Instance.pendingSpawnId = "";
        }
        else
        {
            Debug.LogWarning("PlayerSpawn: aucun SpawnPoint dans la scène.");
        }
    }
}
