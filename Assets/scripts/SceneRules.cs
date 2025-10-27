using UnityEngine;

public class SceneRules : MonoBehaviour
{
    [Header("Behaviour of the persistent Player in this scene")]
    public bool keepPlayerActive = false;     // Win: ON
    public Transform anchor;                  // Optional position for the Player
    public bool killPlayerOnLoad = false;     // GameOver: ON
}
