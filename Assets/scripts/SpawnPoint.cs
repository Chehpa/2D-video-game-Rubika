using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Nom que le portail donnera pour cibler ce point.")]
    public string id = "Default";

    [Tooltip("Point de repli si aucun id ne correspond.")]
    public bool isDefault = false;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.4f, 0.4f, 0.4f));
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.35f, id);
    }
#endif
}
