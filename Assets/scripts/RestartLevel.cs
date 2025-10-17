using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public KeyCode key = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(key))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
