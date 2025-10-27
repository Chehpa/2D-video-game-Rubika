using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [Tooltip("Nom exact de la scène menu")]
    public string menuSceneName = "MainMenu";
    public bool destroyPlayerOnGo = true;

    public void GoMenu()
    {
        if (destroyPlayerOnGo) DestroyPlayer();
        var sl = SceneLoader.Instance; if (sl != null) sl.pendingSpawnId = "";
        SceneManager.LoadScene(menuSceneName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoMenu();
    }

    private void DestroyPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) Destroy(player);
    }
}
