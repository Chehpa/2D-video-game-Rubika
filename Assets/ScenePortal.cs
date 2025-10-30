using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public string sceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }
}
