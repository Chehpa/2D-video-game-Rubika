using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Nom utilisé par les portails (ex: FromRoom1, Start, etc.)")]
    public string id = "Start";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.15f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.2f, $"Spawn: {id}");
    }
}
