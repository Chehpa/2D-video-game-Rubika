using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(100)]
public class PlayerSpawn : MonoBehaviour
{
    private Rigidbody2D rb;
    private TopDownController2_5D topDown;
    private PlayerInteract interact;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        topDown = GetComponent<TopDownController2_5D>();
        interact = GetComponent<PlayerInteract>();
    }

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

    private void SetPlayerActiveInWorld(bool active)
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>(true)) sr.enabled = active;
        foreach (var col in GetComponentsInChildren<Collider2D>(true)) col.enabled = active;
        if (rb) rb.simulated = active;
        if (topDown) topDown.enabled = active;
        if (interact) interact.enabled = active;
    }

    private void PlaceAtSpawn()
    {
        string wanted = (SceneLoader.Instance != null) ? SceneLoader.Instance.pendingSpawnId : "";

        var points = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint target = null;

        if (!string.IsNullOrEmpty(wanted))
        {
            foreach (var p in points)
                if (p.id == wanted) { target = p; break; }
        }
        if (target == null)
        {
            foreach (var p in points)
                if (p.isDefault) { target = p; break; }
        }
        if (target == null && points.Length > 0) target = points[0];

        if (target != null)
        {
            transform.position = target.transform.position;
            SetPlayerActiveInWorld(true);
            if (SceneLoader.Instance != null) SceneLoader.Instance.pendingSpawnId = "";
            return;
        }

        // Pas de SpawnPoint : on regarde les règles de la scène
        var rules = Object.FindFirstObjectByType<SceneRules>();
        if (rules != null)
        {
            if (rules.killPlayerOnLoad)
            {
                Destroy(gameObject);
                return;
            }

            if (rules.keepPlayerActive)
            {
                if (rules.anchor) transform.position = rules.anchor.position;
                SetPlayerActiveInWorld(true);
                return;
            }
        }

        // Par défaut pour les scènes UI sans règles : cacher/désactiver
        SetPlayerActiveInWorld(false);
        transform.position = new Vector3(9999, 9999, 0);
    }
}
