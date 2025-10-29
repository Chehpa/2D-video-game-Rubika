using UnityEngine;

public class SpawnByFlag : MonoBehaviour
{
    public string requiredFlag = "F_RopeCut";
    public bool showIfSet = true;

    void OnEnable() => Refresh();
    void Start() => Refresh();
#if UNITY_EDITOR
    void OnValidate() => Refresh();
#endif
    void Refresh()
    {
        bool has = GameState.IsSet(requiredFlag);
        gameObject.SetActive(showIfSet ? has : !has);
    }
}
