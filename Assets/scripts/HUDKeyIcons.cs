using UnityEngine;
using UnityEngine.UI;

public class HUDKeyIcons : MonoBehaviour
{
    [System.Serializable]
    public class KeySlot
    {
        public string keyId;
        public Image icon;
        public Color haveColor = Color.white;
        public Color missingColor = new Color(1f, 1f, 1f, 0.35f);  // pas 0
    }

    public KeySlot[] keys;

    void Update()
    {
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        if (inv == null) return;

        for (int i = 0; i < keys.Length; i++)
        {
            var slot = keys[i];
            if (slot == null || slot.icon == null || string.IsNullOrEmpty(slot.keyId)) continue;

            bool has = inv.Has(slot.keyId);
            slot.icon.color = has ? slot.haveColor : slot.missingColor;
        }
    }
}
