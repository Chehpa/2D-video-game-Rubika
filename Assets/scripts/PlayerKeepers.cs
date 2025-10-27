using UnityEngine;

public class PlayerKeeper : MonoBehaviour
{
    private static PlayerKeeper _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Un Player existe déjà → on détruit ce doublon.
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);   // Le Player survit aux changements de scène.
    }
}
