using UnityEngine;
using TMPro;

public class HUDKeyDisplayMulti : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public string[] keyIds;

    void Update()
    {
        if (keyText == null) return;

        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        if (inv == null)
        {
            keyText.text = "No inv.";
            return;
        }

        string result = "";
        for (int i = 0; i < keyIds.Length; i++)
        {
            string id = keyIds[i];
            if (string.IsNullOrEmpty(id)) continue;

            bool has = inv.Has(id);
            // petit affichage moche mais efficace
            result += id + ": " + (has ? "✔" : "✖") + "\n";
        }

        keyText.text = result;
    }
}
