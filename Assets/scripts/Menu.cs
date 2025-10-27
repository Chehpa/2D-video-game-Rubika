using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Level to load on Play/Restart")]
    public string levelToLoad = "Room1";

#if UNITY_EDITOR
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
#else
    public void StartGame() => SceneManager.LoadScene(levelToLoad);
    public void QuitGame() => Application.Quit();
#endif
}
