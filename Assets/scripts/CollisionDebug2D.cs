using UnityEngine;

public class CollisionDebug2D : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D c) { Debug.Log("ENTER with " + c.collider.name); }
    void OnCollisionStay2D(Collision2D c) { }
    void OnCollisionExit2D(Collision2D c) { Debug.Log("EXIT with " + c.collider.name); }
}
