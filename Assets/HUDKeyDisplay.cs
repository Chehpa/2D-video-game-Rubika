using UnityEngine;
using TMPro;

public class HUDKeyDisplay : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public TextMeshProUGUI keyText;
    public string itemId = "key";

    void Update()
    {
        if (playerInventory == null || keyText == null) return;

        bool hasKey = playerInventory.HasItem(itemId);
        keyText.text = hasKey ? "Key: ✔" : "Key: ✖";
    }
}
