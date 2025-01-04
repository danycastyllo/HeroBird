using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class UnusedAssetsFinder : MonoBehaviour
{
    [MenuItem("Tools/Find Unused Assets")]
    public static void FindUnusedAssets()
    {
        Debug.Log("Buscando recursos no utilizados...");
        
        // Lista de todos los activos en el proyecto
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        // Conjunto de activos utilizados explícitamente
        HashSet<string> usedAssets = new HashSet<string>();

        // Buscar todos los prefabs, escenas y materiales utilizados en el proyecto
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");

        // Recolectar dependencias de escenas
        foreach (string guid in scenePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] dependencies = AssetDatabase.GetDependencies(path, true);
            foreach (string dependency in dependencies)
            {
                usedAssets.Add(dependency);
            }
        }

        // Recolectar dependencias de prefabs
        foreach (string guid in prefabPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string[] dependencies = AssetDatabase.GetDependencies(path, true);
            foreach (string dependency in dependencies)
            {
                usedAssets.Add(dependency);
            }
        }

        // Comparar todos los activos con los utilizados
        List<string> unusedAssets = new List<string>();
        foreach (string asset in allAssets)
        {
            if (asset.StartsWith("Assets/") && !usedAssets.Contains(asset) && !AssetDatabase.IsValidFolder(asset))
            {
                unusedAssets.Add(asset);
            }
        }

        // Mostrar resultados
        Debug.Log($"Total de recursos en el proyecto: {allAssets.Length}");
        Debug.Log($"Total de recursos utilizados: {usedAssets.Count}");
        Debug.Log($"Total de recursos no utilizados: {unusedAssets.Count}");
        foreach (string unused in unusedAssets)
        {
            Debug.Log("Recurso no utilizado: " + unused);
        }
    }
}
