using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    [Header("Destination")]
    public string destinationScene = "Room2";
    public string destinationSpawnName = "FromRoom1";

    [Header("Condition (inventaire)")]
    public bool requireItem = true;
    [Tooltip("ID requis dans l’inventaire (ex: HairPin)")]
    public string requiredItemId = "HairPin";
    public bool consumeItem = false;

    [Header("UI")]
    public string needText = "Door: it's locked… need a hair pin.";

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Vérifie l’inventaire si requis
        if (requireItem)
        {
            var inv = other.GetComponentInParent<PlayerInventory>() ?? FindObjectOfType<PlayerInventory>();
            if (inv == null)
            {
                Debug.LogWarning("[Portal] Aucun PlayerInventory trouvé.");
                return;
            }

            if (!inv.Has(requiredItemId))
            {
                if (!string.IsNullOrEmpty(needText))
                    Debug.Log($"[Portal] {needText}");
                return;
            }

            if (consumeItem) inv.Remove(requiredItemId);
        }

        // Informe le chargeur de scène du prochain spawn (si présent dans le projet)
        TrySetNextSpawnName(destinationSpawnName);

        // Charge la scène
        if (!string.IsNullOrEmpty(destinationScene))
            SceneManager.LoadScene(destinationScene);
        else
            Debug.LogWarning("[Portal] destinationScene est vide.");
    }

    /// <summary>
    /// Essaie, via réflexion, de régler un point de spawn sur un éventuel SceneLoader.
    /// Recherche les membres statiques suivants (dans cet ordre):
    ///  - Field/Property: NextSpawnPointName / nextSpawnPointName
    ///  - Method: SetNextSpawn(string) / SetSpawn(string)
    /// </summary>
    private static void TrySetNextSpawnName(string spawnName)
    {
        if (string.IsNullOrEmpty(spawnName)) return;

        try
        {
            var sceneLoaderType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == "SceneLoader");

            if (sceneLoaderType == null) return;

            // Fields
            var f = sceneLoaderType.GetField("NextSpawnPointName",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? sceneLoaderType.GetField("nextSpawnPointName",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null) { f.SetValue(null, spawnName); return; }

            // Properties
            var p = sceneLoaderType.GetProperty("NextSpawnPointName",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? sceneLoaderType.GetProperty("nextSpawnPointName",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.CanWrite) { p.SetValue(null, spawnName); return; }

            // Methods
            var m = sceneLoaderType.GetMethod("SetNextSpawn",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? sceneLoaderType.GetMethod("SetSpawn",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (m != null) { m.Invoke(null, new object[] { spawnName }); }
        }
        catch { /* silencieux pour éviter les crashs en build */ }
    }
}
