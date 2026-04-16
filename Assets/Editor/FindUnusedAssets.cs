using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class UnusedAssetsFinder
{
    // Carpetas que nunca se reportan como no utilizadas
    private static readonly string[] excludedPaths = new string[]
    {
        "Assets/Editor/",
        "Assets/Plugins/",
        "Assets/MobileDependencyResolver/",
        "Assets/StreamingAssets/",
        "Assets/Resources/",
    };

    // Extensiones que nunca se reportan (configuración de build, docs)
    private static readonly string[] excludedExtensions = new string[]
    {
        ".gradle", ".md", ".txt", ".json", ".xml",
        ".pdb", ".dll", ".asmdef", ".asmref"
    };

    [MenuItem("HeroBird/Find Unused Assets")]
    public static void FindUnusedAssets()
    {
        Debug.Log("=== HeroBird — Buscando assets no utilizados ===");

        HashSet<string> usedAssets = new HashSet<string>();

        // 1. Dependencias de Escenas
        foreach (string guid in AssetDatabase.FindAssets("t:Scene"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            foreach (string dep in AssetDatabase.GetDependencies(path, true))
                usedAssets.Add(dep);
        }

        // 2. Dependencias de Prefabs
        foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            foreach (string dep in AssetDatabase.GetDependencies(path, true))
                usedAssets.Add(dep);
        }

        // 3. Dependencias de ScriptableObjects (CharacterData, AuraData, ShopItemData)
        foreach (string guid in AssetDatabase.FindAssets("t:ScriptableObject"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            foreach (string dep in AssetDatabase.GetDependencies(path, true))
                usedAssets.Add(dep);
        }

        // 4. Dependencias de Materiales
        foreach (string guid in AssetDatabase.FindAssets("t:Material"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            foreach (string dep in AssetDatabase.GetDependencies(path, true))
                usedAssets.Add(dep);
        }

        // Comparar contra todos los assets del proyecto
        List<string> unusedAssets = new List<string>();
        foreach (string asset in AssetDatabase.GetAllAssetPaths())
        {
            if (!asset.StartsWith("Assets/")) continue;
            if (AssetDatabase.IsValidFolder(asset)) continue;
            if (usedAssets.Contains(asset)) continue;
            if (IsExcluded(asset)) continue;

            unusedAssets.Add(asset);
        }

        // Agrupar resultados por tipo
        List<string> sprites   = new List<string>();
        List<string> audio     = new List<string>();
        List<string> scripts   = new List<string>();
        List<string> others    = new List<string>();

        foreach (string asset in unusedAssets)
        {
            string ext = System.IO.Path.GetExtension(asset).ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".psd")
                sprites.Add(asset);
            else if (ext == ".mp3" || ext == ".wav" || ext == ".ogg")
                audio.Add(asset);
            else if (ext == ".cs")
                scripts.Add(asset);
            else
                others.Add(asset);
        }

        // Mostrar resultados agrupados
        Debug.Log($"Assets no utilizados: {unusedAssets.Count} total");
        Debug.Log($"  Sprites/Texturas: {sprites.Count}");
        Debug.Log($"  Audio: {audio.Count}");
        Debug.Log($"  Scripts: {scripts.Count} (verificar en VSCode, el scanner no es confiable para .cs)");
        Debug.Log($"  Otros: {others.Count}");

        if (sprites.Count > 0)
        {
            Debug.Log("--- SPRITES NO UTILIZADOS ---");
            foreach (string s in sprites) Debug.Log(s);
        }
        if (audio.Count > 0)
        {
            Debug.Log("--- AUDIO NO UTILIZADO ---");
            foreach (string s in audio) Debug.Log(s);
        }
        if (scripts.Count > 0)
        {
            Debug.Log("--- SCRIPTS (verificar manualmente en VSCode) ---");
            foreach (string s in scripts) 
                Debug.Log($"[VERIFICAR MANUALMENTE] {s}");
        }
        if (others.Count > 0)
        {
            Debug.Log("--- OTROS ---");
            foreach (string s in others) Debug.Log(s);
        }
    }

    private static bool IsExcluded(string path)
    {
        foreach (string excluded in excludedPaths)
            if (path.StartsWith(excluded)) return true;

        string ext = System.IO.Path.GetExtension(path).ToLower();
        foreach (string excludedExt in excludedExtensions)
            if (ext == excludedExt) return true;

        return false;
    }
}