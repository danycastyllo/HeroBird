using UnityEditor;
using UnityEngine;

public class AssetReferenceFinder : MonoBehaviour
{
    [MenuItem("Tools/Find Asset References")]
    public static void FindAssetReferences()
    {
        // Mostrar ventana de selección de asset
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Por favor, selecciona un asset en el proyecto.");
            return;
        }

        Debug.Log($"Buscando referencias para el asset: {path}");

        // Obtener todos los posibles activos que puedan referenciar este asset
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        // Verificar dónde se utiliza
        foreach (string asset in allAssets)
        {
            if (AssetDatabase.IsValidFolder(asset)) continue; // Ignorar carpetas

            string[] dependencies = AssetDatabase.GetDependencies(asset, true);
            foreach (string dependency in dependencies)
            {
                if (dependency == path)
                {
                    Debug.Log($"El asset está referenciado en: {asset}");
                    break;
                }
            }
        }

        Debug.Log("Búsqueda completada.");
    }
}
