using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                 // Laisse vide dans l’Inspector
    public Vector3 offset = new Vector3(0, 0, -10f);
    public float smooth = 10f;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryBind();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        TryBind();
    }

    void TryBind()
    {
        if (target == null)
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) target = playerGO.transform;
        }
    }

    void LateUpdate()
    {
        if (!target) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * smooth);
    }
}
