using UnityEditor;
using UnityEngine;

public class AssetReferenceFinder
{
    [MenuItem("HeroBird/Find References for Selected Asset")]
    public static void FindAssetReferences()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Selecciona un asset en el panel Project primero.");
            return;
        }

        Debug.Log($"=== Buscando referencias para: {path} ===");

        int count = 0;
        foreach (string asset in AssetDatabase.GetAllAssetPaths())
        {
            if (AssetDatabase.IsValidFolder(asset)) continue;
            if (asset == path) continue;

            foreach (string dependency in AssetDatabase.GetDependencies(asset, true))
            {
                if (dependency == path)
                {
                    Debug.Log($"Referenciado en: {asset}");
                    count++;
                    break;
                }
            }
        }

        if (count == 0)
            Debug.LogWarning($"Ningún asset referencia a: {path} — candidato a eliminar");
        else
            Debug.Log($"Total de referencias encontradas: {count}");
    }
}