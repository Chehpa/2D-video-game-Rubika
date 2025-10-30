using UnityEngine;

public class ShelfDigit : MonoBehaviour
{
    public int slot = 1;     // 1..3
    public int digit = 7;    // 0..9
    public KeyCode key = KeyCode.E;

    [TextArea] public string message = "La reliure révèle un chiffre : {digit}";
    bool inside;

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) inside = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) inside = false; }

    void Update()
    {
        if (!inside || !Input.GetKeyDown(key)) return;

        if (HUDTextBar.I) HUDTextBar.I.TypeWrite(message.Replace("{digit}", digit.ToString()));
        if (Room2CodeState.I != null) Room2CodeState.I.SetDigit(slot, digit);

        switch (slot)
        {
            case 1: GameStateHost.I.SetFlag(FlagId.F_R2_Word1); break;
            case 2: GameStateHost.I.SetFlag(FlagId.F_R2_Word2); break;
            case 3: GameStateHost.I.SetFlag(FlagId.F_R2_Word3); break;
        }

        if (Room2CodeState.I != null && Room2CodeState.I.HasAll())
        {
            GameStateHost.I.SetFlag(FlagId.F_R2_AllDigits);
            GameStateHost.I.SetFlag(FlagId.F_R2_DoorOpen);
            if (HUDTextBar.I) HUDTextBar.I.SetPersistent("La porte s’ouvre.");
        }
    }
}
