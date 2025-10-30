using UnityEngine;

public class Room2CodeState : MonoBehaviour
{
    public static Room2CodeState I;

    public bool has1, has2, has3;
    public int d1, d2, d3;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDigit(int slot, int value)
    {
        value = Mathf.Clamp(value, 0, 9);
        switch (slot)
        {
            case 1: d1 = value; has1 = true; break;
            case 2: d2 = value; has2 = true; break;
            case 3: d3 = value; has3 = true; break;
        }
    }

    public bool HasAll() => has1 && has2 && has3;
    public string CodeString() => $"{d1}-{d2}-{d3}";
}
